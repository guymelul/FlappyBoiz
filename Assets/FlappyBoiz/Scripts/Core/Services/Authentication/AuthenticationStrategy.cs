using UnityEngine;

namespace FlappyBoiz.Scripts.Core.Services.Authentication
{
    public class AuthenticationStrategy
    {
        public IAuthenticationService GetAuthenticationService()
        {
#if UNITY_ANDROID
            return new GooglePlayGamesAuthenticationService();
#else
            return new AnonymousAuthenticationService();
#endif
        }
    }
}
