using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    string interId = "ca-app-pub-3940256099942544/1033173712"; // Å×½ºÆ® ±¤°í id
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
        }
        else
        {
            print("Intersititial ad not ready!!");
        }
    }

    public void InterstitialEvent(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");

            // ±¤°í ¶ß¸é BGM Áï½Ã ²ô±â
            foreach (var player in AudioManager.Instance.bgmPlayers)
            {
                if (player.isPlaying)
                    player.volume = 0f;
            }
        };

        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");

            // ±¤°í ´ÝÈ÷¸é BGM ´Ù½Ã Å°¿ì±â
            foreach (var player in AudioManager.Instance.bgmPlayers)
            {
                if (player.isPlaying)
                    player.volume = AudioManager.Instance.bgmVolume;
            }

            // ±¤°í ´ÝÈ÷¸é Å¸ÀÌÆ²·Î ÀÌµ¿
            if (PlayerStatus.Instance.gameOver)
            {
                StartCoroutine(GameOverUIManager.Instance.ReturnToTitle());
            }
            else
            {
                StartCoroutine(MenuUIManager.Instance.ReturnToTitle());
            }

            // ±¤°í »õ·Î ·Îµå
            var adRequest = new AdRequest();
            InterstitialAd.Load(interId, adRequest, (InterstitialAd newAd, LoadAdError error) =>
            {
                if (error != null || newAd == null)
                {
                    Debug.Log("Failed to reload interstitial ad: " + error);
                    return;
                }

                interstitialAd = newAd;
                InterstitialEvent(interstitialAd);
            });
        };

        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // ±¤°í »õ·Î ·Îµå
            var adRequest = new AdRequest();
            InterstitialAd.Load(interId, adRequest, (InterstitialAd newAd, LoadAdError error) =>
            {
                if (error != null || newAd == null)
                {
                    Debug.Log("Failed to reload interstitial ad: " + error);
                    return;
                }

                interstitialAd = newAd;
                InterstitialEvent(interstitialAd);
            });
        };
    }
}
