using GoogleMobileAds.Api;
using UnityEngine;

namespace GoogleMobileAds.Scripts
{
    public class InterAd : MonoBehaviour
    {
        private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";

        private InterstitialAd _interstitialAd;
    
        private void LoadInterstitialAd()
        {
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }
            Debug.Log("Loading the interstitial ad.");
        
            var adRequest = new AdRequest();

            InterstitialAd.Load(_adUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    _interstitialAd = ad;
                });
        }

        public void ShowInterstitialAdd()
        {
            LoadInterstitialAd();
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                _interstitialAd.Show();
            }
        }
    }
}
