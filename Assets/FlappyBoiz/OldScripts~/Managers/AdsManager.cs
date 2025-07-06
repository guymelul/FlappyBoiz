using GoogleMobileAds.Api;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FlappyBoiz.Scripts.Managers
{
    [DisallowMultipleComponent]
    public class AdsManager : MonoBehaviour
    {
        public static AdsManager Instance { get; private set; }

        [SerializeField]
        private StringEvent onGameStateChanged;

        private BannerView m_bannerView;
        private bool m_adsInitialized;
        private bool m_adShown;

#if UNITY_ANDROID
        private const string AdUnitId = "ca-app-pub-8740023635912497/5179168894";
#elif UNITY_IPHONE
        private const string AdUnitId = "ca-app-pub-3940256099942544/2934735716";
#elif UNITY_EDITOR
        private const string AdUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
        private const string AdUnitId = "unexpected_platform";
#endif

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple ADs managers detected!");
                Destroy(gameObject);

                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            onGameStateChanged.Register(OnGameStateChanged);
        }

        private void OnDisable()
        {
            onGameStateChanged.Unregister(OnGameStateChanged);
        }

        private void Start()
        {
            MobileAds.Initialize(OnAdsInitialization);
        }

        private void OnGameStateChanged(string state)
        {
            switch (state)
            {
                case GameStates.Playing:
                    HideAd();

                    break;
                default:
                    LoadAd();

                    break;
            }
        }

        private void OnAdsInitialization(InitializationStatus initStatus)
        {
            m_adsInitialized = true;
            Debug.Log("Google Mobile Ads initialized");
        }

        private void CreateBanner()
        {
            if (m_bannerView != null) return;

            Debug.Log($"Creating banner view. {Screen.safeArea}");
            m_bannerView = new BannerView(
                AdUnitId,
                AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth),
                AdPosition.Bottom
            );
            m_adShown = true;
            ListenToAdEvents();
        }

        private void HideAd()
        {
            if (!m_adsInitialized || m_bannerView == null) return;

            Debug.Log("Destroying banner view.");
            m_bannerView.Hide();
            m_adShown = false;
        }

        private void LoadAd()
        {
            if (!m_adsInitialized) return;

            // create an instance of a banner view first.
            if (m_bannerView != null)
            {
                if (!m_adShown) m_bannerView.Show();

                m_adShown = true;

                return;
            }

            CreateBanner();

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            m_bannerView?.LoadAd(adRequest);
        }

        private void ListenToAdEvents()
        {
            // Raised when an ad is loaded into the banner view.
            m_bannerView.OnBannerAdLoaded += () =>
            {
                Debug.Log("Banner view loaded an ad with response : " + m_bannerView.GetResponseInfo());
            };

            // Raised when an ad fails to load into the banner view.
            m_bannerView.OnBannerAdLoadFailed += error =>
            {
                Debug.LogError("Banner view failed to load an ad with error : " + error);
            };

            // Raised when the ad is estimated to have earned money.
            m_bannerView.OnAdPaid += adValue =>
            {
                Debug.Log($"Banner view paid {adValue.Value} {adValue.CurrencyCode}.");
            };

            // Raised when an impression is recorded for an ad.
            m_bannerView.OnAdImpressionRecorded += () => { Debug.Log("Banner view recorded an impression."); };

            // Raised when a click is recorded for an ad.
            m_bannerView.OnAdClicked += () => { Debug.Log("Banner view was clicked."); };

            // Raised when an ad opened full screen content.
            m_bannerView.OnAdFullScreenContentOpened += () => { Debug.Log("Banner view full screen content opened."); };

            // Raised when the ad closed full screen content.
            m_bannerView.OnAdFullScreenContentClosed += () => { Debug.Log("Banner view full screen content closed."); };
        }
    }
}