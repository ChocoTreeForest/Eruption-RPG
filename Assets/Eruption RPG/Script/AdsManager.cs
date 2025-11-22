using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    string interId = "ca-app-pub-3940256099942544/1033173712"; // 테스트 광고 id
    public InterstitialAd interstitialAd;

    public void Start()
    {
        // Initialize Google Mobile Ads Unity Plugin.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        LoadInterstitialAd();
    }

    public void LoadInterstitialAd()
    {

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Interstitial ad failed to load" + error);
                return;
            }

            print("Interstitial ad loaded !!" + ad.GetResponseInfo());

            interstitialAd = ad;
            InterstitialEvent(interstitialAd);
        });

    }

    public void ShowInterstitialAd()
    {

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            return;
        }

        // 광고가 없으면 로드하고 다음부터 표시
        LoadInterstitialAd();
    }

    public void InterstitialEvent(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {
            // 광고 뜨면 BGM 즉시 끄기
            foreach (var player in AudioManager.Instance.bgmPlayers)
            {
                if (player.isPlaying)
                    player.volume = 0f;
            }
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            // 광고 닫히면 BGM 다시 키우기
            foreach (var player in AudioManager.Instance.bgmPlayers)
            {
                if (player.isPlaying)
                    player.volume = AudioManager.Instance.bgmVolume;
            }

            // 광고 닫히면 타이틀로 이동
            if (PlayerStatus.Instance.gameOver)
            {
                StartCoroutine(GameOverUIManager.Instance.ReturnToTitle());
            }
            else
            {
                StartCoroutine(MenuUIManager.Instance.ReturnToTitle());
            }

            // 광고 새로 로드
            var adRequest = new AdRequest();
            InterstitialAd.Load(interId, adRequest, (InterstitialAd newAd, LoadAdError error) =>
            {
                if (error != null || newAd == null)
                {
                    return;
                }

                interstitialAd = newAd;
                InterstitialEvent(interstitialAd);
            });
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // 광고 새로 로드
            var adRequest = new AdRequest();
            InterstitialAd.Load(interId, adRequest, (InterstitialAd newAd, LoadAdError error) =>
            {
                if (error != null || newAd == null)
                {
                    return;
                }

                interstitialAd = newAd;
                InterstitialEvent(interstitialAd);
            });
        };
    }
}
