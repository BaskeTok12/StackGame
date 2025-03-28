using CodeBase.Block_Controller;
using GoogleMobileAds.Api;
using UnityEngine;

namespace GoogleMobileAds.Scripts
{
    public class ADManager : MonoBehaviour
    {
        [Header("Banners")]
        [SerializeField] private InterAd interstitialAd;
    
        [Header("Add conditions")]
        [SerializeField] private int missesToAddLimit;
        private int _missesCount;
        private void Awake()
        {
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize(initStatus => { });
        }
        private void OnEnable()
        {
            CubeController.OnMiss += TryToShowAdd;
        }

        private void OnDisable()
        {
            CubeController.OnMiss -= TryToShowAdd;
        }

        private void TryToShowAdd()
        {
            _missesCount += 1;

            if (_missesCount >= missesToAddLimit)
            {
                interstitialAd.ShowInterstitialAdd();
                _missesCount = 0;
            }
        }
    }
}
