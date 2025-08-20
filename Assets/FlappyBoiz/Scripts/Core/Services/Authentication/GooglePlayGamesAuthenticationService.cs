#if UNITY_ANDROID
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace FlappyBoiz.Scripts.Core.Services.Authentication
{
    public class GooglePlayGamesAuthenticationService : IAuthenticationService
    {
        public async UniTask<bool> AuthenticateAsync(CancellationToken ct = default)
        {
            try
            {
                if (!await SignInWithGooglePlayGamesAsync())
                {
                    return false;
                }

                Debug.Log($"Signed in with Google Play Games. Player ID: {AuthenticationService.Instance.PlayerId}");

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Google Play Games sign-in failed: {e.Message}");

                return false;
            }
        }

        private async UniTask<bool> SignInWithGooglePlayGamesAsync()
        {
            PlayGamesPlatform.Activate();

            var tcs = new UniTaskCompletionSource<string>();

            PlayGamesPlatform.Instance.Authenticate(status =>
            {
                if (status == SignInStatus.Success)
                {
                    Debug.Log("Auto Google Play Games sign-in successful. Requesting server auth code.");
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => tcs.TrySetResult(code));
                }
                else
                {
                    PlayGamesPlatform.Instance.ManuallyAuthenticate(manualStatus =>
                    {
                        if (manualStatus == SignInStatus.Success)
                        {
                            Debug.Log("Manual Google Play Games sign-in successful. Requesting server auth code.");
                            PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => tcs.TrySetResult(code));
                        }
                        else if (manualStatus == SignInStatus.Canceled)
                        {
                            tcs.TrySetCanceled();
                        }
                        else
                        {
                            tcs.TrySetException(
                                new Exception($"Google Play Games sign-in failed with status: {manualStatus}"));
                        }
                    });
                }
            });

            try
            {
                string authCode = await tcs.Task;

                if (string.IsNullOrEmpty(authCode))
                    throw new Exception("Google Play Games returned invalid server side access code");

                Debug.Log($"Updating Unity Auth for user: {PlayGamesPlatform.Instance.GetUserDisplayName()}");

                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);

                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (RequestFailedException e)
            {
                Debug.LogError($"Unity authentication failed ({e.ErrorCode}): {e.Message}");

                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Google Play Games sign-in process failed: {e.Message}");

                return false;
            }
        }
    }
}
#endif