using FlappyBoiz.Scripts.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.Managers
{
    public enum SocialPlatform
    {
        Anonymous,
        GooglePlayGames,
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameEventInvoker))]
    public class AuthenticationManager : MonoBehaviour
    {
        public static AuthenticationManager Instance { get; private set; }
        public bool IsAuthInProgress { get; private set; }

        private bool m_autoLoginFailed;

        public SocialPlatform ActiveSocialPlatform { get; private set; }

        [SerializeField]
        public StringVariable userDisplayName;

        private GameEventInvoker m_gameEventInvoker;

        private void Awake()
        {
            m_autoLoginFailed = false;
            IsAuthInProgress = true;

            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple authentication managers detected!");
                Destroy(gameObject);

                return;
            }

            Instance = this;
        }

        private void Start()
        {
            m_gameEventInvoker = GetComponent<GameEventInvoker>();

            // Try automatic login
            IsAuthInProgress = true;
            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate(OnGoogleSignInStatusChanged);
        }

        public void AnonymousLogin()
        {
            Social.Active = null;

#if UNITY_EDITOR
            Social.Active.Authenticate(Social.Active.localUser, _ => Debug.Log("Anonymous user logged in"));
#endif

            ActiveSocialPlatform = SocialPlatform.Anonymous;
            m_gameEventInvoker.Authenticated();
        }

        public void GooglePlayGamesLogin()
        {
            if (IsAuthInProgress) return;

            IsAuthInProgress = true;

            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.ManuallyAuthenticate(OnGoogleSignInStatusChanged);
        }

        private void OnGoogleSignInStatusChanged(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                userDisplayName.Value = PlayGamesPlatform.Instance.GetUserDisplayName();

                ActiveSocialPlatform = SocialPlatform.GooglePlayGames;
                m_gameEventInvoker.Authenticated();

                return;
            }

            IsAuthInProgress = false;
            Social.Active = null;

            // Only show message when manually authenticating with google
            if (m_autoLoginFailed)
            {
                PopupMessage.Instance.Show($"Google Sign In Failed");
            }

            m_autoLoginFailed = true;
        }
    }
}