using System;
using System.Collections;
using System.Collections.Generic;

using GoogleMobileAds.Api;
using Unity.VisualScripting;
using UnityEngine;

public class BannerManager : MonoBehaviour
{
    public static BannerManager Instance;
    private const string _adUnitIdCollapsible = "ca-app-pub-2397911547989551/9583082568";
    private BannerView _bannerViewCollapsible;
    int collapsible_banner_capping_time=20;
    int collapsible_banner_life_time;
    bool collapsible_banner_loaded;

    //private const string _adUnitIdNormal = "ca-app-pub-2913496970595341/2649320852";
    //private BannerView _bannerViewNormal;

    float currentTime = 0;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= collapsible_banner_capping_time)
        {
            currentTime = 0;
            LoadBannerAdCollapsible();
            //collapsible_banner_capping_time = UserData.collapsible_banner_capping_time;
            //collapsible_banner_life_time = UserData.collapsible_banner_life_time;
        }
        //if (collapsible_banner_loaded)
        //{
        //    collapsible_banner_loaded = false;
        //    Invoke(nameof(DestroyCollapsible), collapsible_banner_life_time);
        //}
    }
    private void Awake()
    {
        Instance = this;
    }

    //private IEnumerator Start()
    //{
    //    yield return new WaitUntil(() => AdsAdapter.instance.AMInitialized);
    //    collapsible_banner_capping_time = UserData.collapsible_banner_capping_time;
    //    collapsible_banner_life_time = UserData.collapsible_banner_life_time;
    //    LoadBannerAdCollapsible();
    //}
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadBannerAdCollapsible();
            // This callback is called once the MobileAds SDK is initialized.
        });
    }
    /// <summary>
    /// chuyển từ home => minigame
    /// chuyển từ minigame => home
    /// chuyển từ minigame A => minigame B
    ///// </summary>
    //public void LoadBannerTransferScreen()
    //{
    //    currentTime = 0;
    //    LoadBannerAdCollapsible();
    //}
    private void LoadBannerAdCollapsible()
    {
        Debug.Log("Load collapsible banner");
        DestroyBannerViewCollapsible();
        //var adsize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        _bannerViewCollapsible = new BannerView(_adUnitIdCollapsible, AdSize.Banner, AdPosition.Bottom);

        var adRequest = new AdRequest();
        // Create an extra parameter that aligns the bottom of the expanded ad to the bottom of the bannerView.
        adRequest.Extras.Add("collapsible", "bottom");
        _bannerViewCollapsible.LoadAd(adRequest);

        Guid myuuid = Guid.NewGuid();
        string myuuidAsString = myuuid.ToString();
        adRequest.Extras.Add("collapsible_request_id", myuuidAsString);
        ListenToAdEventsCollapsible();
    }
    #region Banner callback handlers

    private void ListenToAdEventsCollapsible()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerViewCollapsible.OnBannerAdLoaded += OnBannerAdLoadedCollapsible;
        _bannerViewCollapsible.OnBannerAdLoadFailed += OnBannerAdLoadFailedCollapsible;

        // Raised when the ad is estimated to have earned money.
        _bannerViewCollapsible.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerViewCollapsible.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerViewCollapsible.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerViewCollapsible.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerViewCollapsible.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    private void OnBannerAdLoadedCollapsible()
    {
        if (_bannerViewCollapsible != null)
        {
            Debug.Log("Banner view loaded an ad with response : " + _bannerViewCollapsible.GetResponseInfo());
        }

        //Debug.Log($"Ad Height: {_bannerViewCollapsible.GetHeightInPixels()}, width: {_bannerViewCollapsible.GetWidthInPixels()}");
        // _bannerViewCollapsible.hideAA
        collapsible_banner_loaded = true;
        Debug.Log("collapsible_banner_life_time: " + collapsible_banner_life_time);

    }

    void DestroyCollapsible()
    {
        Debug.Log("Replace collapsible banner by Max banner");
        DestroyBannerViewCollapsible();

        currentTime = 0;
    }

    private void OnBannerAdLoadFailedCollapsible(LoadAdError error)
    {
        Debug.LogError("Banner view failed to load an ad with error : " + error);
 

    }
    #endregion

    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void DestroyBannerViewCollapsible()
    {
        if (_bannerViewCollapsible != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerViewCollapsible.Destroy();
            _bannerViewCollapsible = null;
        }
    }


}