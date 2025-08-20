using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace FlappyBoiz.Scripts.Core.Services.Authentication
{
    public class AnonymousAuthenticationService : IAuthenticationService
    {
        public async UniTask<bool> AuthenticateAsync(CancellationToken ct = default)
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

        private static void LogSignedInPlayer(string message)
        {
            Debug.Log(message);
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
        }
    }
}
