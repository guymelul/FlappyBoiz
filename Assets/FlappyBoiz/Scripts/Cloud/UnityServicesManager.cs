using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using VContainer.Unity;
#if UNITY_ANDROID && !UNITY_EDITOR
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace FlappyBoiz.Scripts.Cloud
{
    public class UnityServicesManager : IAsyncStartable
    {
        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
        }

        public async UniTask<bool> Authenticate(CancellationToken cancellation = new())
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            bool googleSignedIn = await SignInWithGooglePlayGamesAsync();
            if (googleSignedIn)
            {
                // If the flow reaches here, we are signed in with Google.
                LogSignedInPlayer("Player signed in with Google Play Games.");
                return true;
            }
            else
            {
                // This can happen if the user cancels, or if there's a configuration error.
                Debug.LogWarning("Google Play Games sign-in failed or was cancelled. Falling back to anonymous sign-in.");
            }
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

                if (AuthenticationService.Instance.IsSignedIn)
                {
                    LogSignedInPlayer("Player signed in anonymously.");

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Anonymous sign-in failed: {e.Message}");

                return false;
            }
        }

#if UNITY_ANDROID && !UNITY_EDITOR
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
                    Debug.Log("Google Play Games sign-in successful. Requesting server auth code.");
                    // If authentication is successful, we can request a server auth code.
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                    {
                        if (!string.IsNullOrEmpty(code))
                        {
                            tcs.TrySetResult(code);
                        }
                        else
                        {
                            tcs.TrySetException(new Exception("Google Play Games server auth code was null or empty."));
                        }
                    });
                }
                else
                {
                    tcs.TrySetException(new Exception($"Google Play Games sign-in failed with status: {status}"));
                }
            });

            try
            {
                // Wait for the authentication and server code retrieval to complete.
                string authCode = await tcs.Task;
                Debug.Log("Received Server Auth Code. Signing into Unity Authentication...");

                // Sign in to Unity Authentication using the Google auth code.
                await AuthenticationService.Instance.SignInWithGoogleAsync(authCode);
                return true;
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