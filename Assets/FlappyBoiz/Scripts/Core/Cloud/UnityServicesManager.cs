using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.LevelPlay;
using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace FlappyBoiz.Scripts.Core.Cloud
{
    public class UnityServicesManager
    {
        public async UniTask Init(CancellationToken cancellation = new())
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
                return;

#if UNITY_EDITOR
            LevelPlay.SetMetaData("is_test_suite", "enable");
            LevelPlay.LaunchTestSuite();
#endif

            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
        }

        public async UniTask<bool> Authenticate(CancellationToken cancellation = new())
        {
            // Check if a cached player already exists by checking if the session token exists
            // if (!AuthenticationService.Instance.SessionTokenExists)
            // {
            //     // if not, then do nothing
            //     return true;
            // }

            if (AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log($"Player already signed in [{AuthenticationService.Instance.PlayerId}]");

                return true;
            }

#if UNITY_ANDROID
            if (await SignInWithGooglePlayGamesAsync())
            {
                // If the flow reaches here, we are signed in with Google.
                LogSignedInPlayer("Player signed in with Google Play Games.");

                return true;
            }


            // This can happen if the user cancels, or if there's a configuration error.
            Debug.LogWarning("Google Play Games sign-in failed or was cancelled. Falling back to anonymous sign-in.");
#endif
            return await SignInAnonymouslyAsync();
        }

        private static void LogSignedInPlayer(string message)
        {
            Debug.Log(message);
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
        }

        private async UniTask<bool> SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (!AuthenticationService.Instance.IsSignedIn) return false;

                LogSignedInPlayer("Player signed in anonymously.");

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Anonymous sign-in failed: {e.Message}");

                return false;
            }
        }

#if UNITY_ANDROID
        private async UniTask<bool> SignInWithGooglePlayGamesAsync()
        {
            // This is the manual flow using the PlayGamesPlatform plugin.
            // 1. Configure and activate the platform.
            // 2. Authenticate the user to get a server auth code.
            // 3. Use that code to sign into Unity Authentication.

            PlayGamesPlatform.Activate();

            var tcs = new UniTaskCompletionSource<string>();

            // The PlayGamesPlatform uses callbacks. We wrap it in a UniTask
            // to use it with async/await.
            PlayGamesPlatform.Instance.Authenticate(status =>
            {
                if (status == SignInStatus.Success)
                {
                    Debug.Log("Auto Google Play Games sign-in successful. Requesting server auth code.");
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => tcs.TrySetResult(code));
                }
                else
                {
                    PlayGamesPlatform.Instance.ManuallyAuthenticate(status =>
                    {
                        if (status == SignInStatus.Success)
                        {
                            Debug.Log("Manual Google Play Games sign-in successful. Requesting server auth code.");
                            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => tcs.TrySetResult(code));
                        }
                        else
                        {
                            tcs.TrySetException(
                                new Exception($"Google Play Games sign-in failed with status: {status}"));
                        }
                    });
                }
            });

            try
            {
                // Wait for the authentication and server code retrieval to complete.
                string authCode = await tcs.Task;

                if (string.IsNullOrEmpty(authCode))
                    throw new Exception("Google Play Games returned invalid server side access code");

                // Sign in to Unity Authentication using the Google auth code.
                Debug.Log($"Updating Unity Auth for user: {PlayGamesPlatform.Instance.GetUserDisplayName()}");

                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);

                return true;
            }
            catch (RequestFailedException e)
            {
                Debug.LogError($"Unity Authentication failed ({e.ErrorCode}): {e.Message}");

                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Google Play Games sign-in process failed: {e.Message}");

                return false;
            }
        }
#endif
    }
}