using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager manager;
    [SerializeField] bool isInit;

    [Header("-------> TEST IDS")]
    [SerializeField] string testid_app = "ca-app-pub-3940256099942544~3347511713";
    [SerializeField] string testid_banner = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] string testid_inter = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] string testid_reward = "ca-app-pub-3940256099942544/5224354917";
    [SerializeField] string testid_rewardinter = "ca-app-pub-3940256099942544/5354046379";
    [SerializeField] string testid_appopen = "ca-app-pub-3940256099942544/9257395921";
    [SerializeField] string testid_native = "ca-app-pub-3940256099942544/2247696110";

    [Header("-------> REAL IDS")]
    [SerializeField] string realid_banner;
    [SerializeField] string realid_inter;
    [SerializeField] string realid_reward;
    [SerializeField] string realid_rewardinter;
    [SerializeField] string realid_appopen;
    [SerializeField] string realid_native;

    private void Awake() => manager = this;

    private void Start()
    {
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    public void initSDK()
    {
        realid_banner = ARGManager.manager.data.admob_BannerId;
        realid_inter = ARGManager.manager.data.admob_InterId;
        realid_reward = ARGManager.manager.data.admob_RewardedVideo_AdId;
        realid_rewardinter = ARGManager.manager.data.admob_RewardedInterId;
        realid_appopen = ARGManager.manager.data.admob_AppOpenId;
        realid_native = ARGManager.manager.data.admob_NativeId;

        if (ARGManager.manager.data.testMode)
        {
            List<string> testDeviceIds = new List<string>();
            testDeviceIds.Add("161D9458B3A9448CEC0E4034E1B6D520");
            testDeviceIds.Add("4A65370603333CA695AE1F0666BCA038");
            RequestConfiguration requestConfiguration = new RequestConfiguration
            {
                TestDeviceIds = testDeviceIds
            };
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }

        nativeads_show_count = ARGManager.manager.data.nativeads_show_count;

        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                ArgUtil.log("Google Mobile Ads initialization failed.", Keys.ColorRed);
                return;
            }
            ArgUtil.log("Google Mobile Ads initialization success.", Keys.ColorGreen);
            isInit = true;
            appopen_LoadAd();
            native_LoadAd();
        });
    }

    #region Banner---------------------

    private BannerView _bannerView;
    [Header("-------> Banner")]
    [SerializeField] bool isloading_Banner;

    public void banner_LoadAd()
    {
        if (isloading_Banner) return;
        banner_DestroyAd();
        isloading_Banner = true;

        AdSize adsize =
                ARGManager.manager.data.admob_banner_isadaptive ?
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth) :
                AdSize.Banner;

        string id = ARGManager.manager.data.testMode ? testid_banner : realid_banner;
        _bannerView = new BannerView(id, adsize,
            ARGManager.manager.data.isBannerBottom ? AdPosition.Bottom : AdPosition.Top);

        banner_ListenToAdEvents();

        var adRequest = new AdRequest();
        if (ARGManager.manager.data.admob_banner_iscollapsible) adRequest.Extras.Add("collapsible", "bottom");

        ArgUtil.log("Admob Loading banner ad...");
        _bannerView.LoadAd(adRequest);
    }

    public void banner_ShowAd()
    {
        if (_bannerView != null)
        {
            if (!AdManager.manager.canShowBannerOnPanel())
            {
                ArgUtil.log("Admob Can not show banner on current panel.");
                banner_HideAd();
                return;
            }

            ArgUtil.log("Admob Showing banner view.");
            AdManager.manager.showBannerBG();
            _bannerView.Show();
            AdManager.manager.current_Banner = Keys._ADMOB;
        }
        else
        {
            if (!isloading_Banner)
            {
                banner_LoadAd();
            }
        }
    }

    public void banner_HideAd()
    {
        if (_bannerView != null)
        {
            ArgUtil.log("Admob Hiding banner view.");
            _bannerView.Hide();
            AdManager.manager.hideBannerBG();
        }
    }

    public void banner_DestroyAd()
    {
        AdManager.manager.hideBannerBG();
        if (_bannerView != null)
        {
            ArgUtil.log("Admob Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    private void banner_ListenToAdEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            ArgUtil.log("Admob Banner Loaded : " + _bannerView.GetResponseInfo(), Keys.ColorGreen);
            isloading_Banner = false;
            banner_ShowAd();
        };

        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            _bannerView = null;
            ArgUtil.log("Admob Banner view failed to load an ad with error : " + error, Keys.ColorRed);
            isloading_Banner = false;
        };
    }

    #endregion

    #region AppOpen----------------------------------------------------------------------

    private AppOpenAd _appOpenAd;
    [Header("-------> APP OPEN")]
    [SerializeField] bool isloading_appopen;
    public bool isShowing_Appopen;


    public void appopen_LoadAd()
    {
        if (IAPManager.manager.hasVideoPurchase) return;
        if (ARGManager.manager.isTiredModeOn()) return;
        if (!isInit) return;
        if (isloading_appopen) return;

        isShowing_Appopen = false;
        isloading_appopen = true;
        appopen_DestroyAd();

        ArgUtil.log("Loading app open ad.", Keys.ColorYellow);
        var adRequest = new AdRequest();

        AppOpenAd.Load(ARGManager.manager.data.testMode ? testid_appopen : realid_appopen, adRequest, (AppOpenAd ad, LoadAdError error) =>
        {
            isloading_appopen = false;
            if (error != null)
            {
                isShowing_Appopen = false;
                ArgUtil.log("App open ad failed to load an ad with error : " + error, Keys.ColorRed);
                return;
            }

            if (ad == null)
            {
                isShowing_Appopen = false;
                ArgUtil.log("Unexpected error: App open ad load event fired with null ad and null error.", Keys.ColorRed);
                return;
            }

            ArgUtil.log("App open ad loaded with response : " + ad.GetResponseInfo(), Keys.ColorGreen);
            _appOpenAd = ad;
            appopen_RegisterEventHandlers(ad);

            if (ARGManager.manager.data.show_appopen_onlaunch)
            {
                ARGManager.manager.data.show_appopen_onlaunch = false;
                if (ARGManager.manager.SplashPanel.activeSelf) appopen_ShowAd();
            }
        });
    }

    public void appopen_ShowAd()
    {
        if (IAPManager.manager.hasVideoPurchase) return;
        if (ARGManager.manager.isTiredModeOn()) return;
        if (isShowing_inter) return;
        if (isShowing_Reward) return;
        if (_appOpenAd != null && _appOpenAd.CanShowAd())
        {
            isShowing_Appopen = true;
            ArgUtil.log("Showing app open ad.", Keys.ColorYellow);
            _appOpenAd.Show();
        }
        else
        {
            isShowing_Appopen = false;
            if (!isloading_appopen) appopen_LoadAd();
            ArgUtil.log("App open ad is not ready yet.", Keys.ColorRed);
        }
    }

    public void appopen_DestroyAd()
    {
        if (_appOpenAd != null)
        {
            isShowing_Appopen = false;
            ArgUtil.log("Destroying app open ad.");
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }
    }

    private void OnAppStateChanged(AppState state)
    {
        ArgUtil.log("App State changed to : " + state);
        if (state == AppState.Foreground)
        {
            appopen_ShowAd();
        }
    }

    private void appopen_RegisterEventHandlers(AppOpenAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            isShowing_Appopen = false;
            ArgUtil.log("App open ad full screen content closed.");
            appopen_LoadAd();
        };

        ad.OnAdFullScreenContentFailed += (error) =>
        {
            isShowing_Appopen = false;
            ArgUtil.log($"App open ad full screen content failed to show\nMessage : {error.ToString()}.", Keys.ColorRed);
        };


    }

    #endregion

    #region Interstitial-----------------------------------------

    private InterstitialAd interAd;
    [Header("-------> Interstitial")]
    [SerializeField] bool isloading_inter;
    public bool isShowing_inter;

    public void inter_LoadAd()
    {
        if (isloading_inter) return;

        if (interAd != null)
        {
            ArgUtil.log("Admob Destroying interstitial ad.");
            interAd.Destroy();
            interAd = null;
        }

        isloading_inter = true;
        isShowing_inter = false;

        ArgUtil.log("Admob Loading interstitial ad");
        var adRequest = new AdRequest();
        InterstitialAd.Load(ARGManager.manager.data.testMode ? testid_inter : realid_inter, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            isloading_inter = false;
            if (error != null)
            {
                AdManager.manager.requestInter(AdManager.manager.inter_provider);
                ArgUtil.log("Admob Interstitial ad failed to load an ad with error : " + error, Keys.ColorRed);
                return;
            }

            if (ad == null)
            {
                AdManager.manager.requestInter(AdManager.manager.inter_provider);
                ArgUtil.log("Admob Unexpected error: Interstitial load event fired with null ad and null error.", Keys.ColorRed);
                return;
            }

            AdManager.manager.startInterInterval();
            AdManager.manager.interFiller = inter_ShowAd;
            ArgUtil.log("Admob Interstitial ad loaded with response : " + ad.GetResponseInfo(), Keys.ColorGreen);
            interAd = ad;
            inter_RegisterEventHandlers(ad);

            if (AdManager.manager.loadAndShow) AdManager.manager.interFiller?.Invoke();
        });
    }

    public void inter_ShowAd()
    {
        if (interAd != null && interAd.CanShowAd())
        {
            ARGManager.manager.hideProcessWaiter();
            AdManager.manager.interFiller = null;
            isShowing_inter = true;
            ArgUtil.log("Admob Showing interstitial ad.");
            interAd.Show();
        }
        else
        {
            AdManager.manager.afterInter();
            ArgUtil.log("Admob Interstitial ad is not ready yet.", Keys.ColorYellow);
        }
    }

    private void inter_RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            ARGManager.manager.getsetAfterInterClosed++;
            isShowing_inter = false;
            AdManager.manager.afterInter();
            ArgUtil.log("Admob Interstitial ad full screen content closed.", Keys.ColorYellow);
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            isShowing_inter = false;
            ArgUtil.log("Admob Interstitial ad failed to open full screen content with error : " + error, Keys.ColorRed);
            AdManager.manager.afterInter();
        };
    }

    #endregion

    #region Rewarded ADS------------------------------

    private RewardedAd rewardedAd;
    [Header("-------> Rewarded")]
    public bool isShowing_Reward;
    [SerializeField] bool hasReward;
    [SerializeField] string lastRewardShowedType;

    public void decide_And_Load_Reward()
    {
        if (ARGManager.manager.data.showAdmob_rewardedinter) reward_Inter_LoadAd();
        else
        {
            if (ARGManager.manager.data.alternate_Admob_Rw_RwInter_Ads)
            {
                if (lastRewardShowedType.Equals(Keys._REWARD_INTER) || lastRewardShowedType == "")
                {
                    lastRewardShowedType = Keys._REWARD;
                    reward_LoadAd();
                }
                else
                {
                    lastRewardShowedType = Keys._REWARD_INTER;
                    reward_Inter_LoadAd();
                }
            }
            else reward_LoadAd();
        }
    }

    #region RW Regular --------------------------------------------------------------------

    public void reward_LoadAd()
    {
        hasReward = false;
        if (rewardedAd != null)
        {
            ArgUtil.log("Admob Destroying rewarded ad.");
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        ArgUtil.log("Admob Loading rewarded ad.");

        var adRequest = new AdRequest();
        RewardedAd.Load(ARGManager.manager.data.testMode ? testid_reward : realid_reward, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                AdManager.manager.requestReward(AdManager.manager.reward_provider);
                ArgUtil.log("Admob Rewarded ad failed to load an ad with error : " + error, Keys.ColorRed);
                return;
            }
            if (ad == null)
            {
                AdManager.manager.requestReward(AdManager.manager.reward_provider);
                ArgUtil.log("Admob Unexpected error: Rewarded load event fired with null ad and null error.", Keys.ColorRed);
                return;
            }

            ArgUtil.log("Admob Rewarded ad loaded with response : " + ad.GetResponseInfo(), Keys.ColorGreen);
            rewardedAd = ad;
            reward_RegisterEventHandlers(ad);
            reward_ShowAd();
        });
    }

    public void reward_ShowAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            ARGManager.manager.hideProcessWaiter();
            isShowing_Reward = true;
            ArgUtil.log("Admob Showing rewarded ad.");
            rewardedAd.Show((Reward reward) =>
            {
                hasReward = true;
                ArgUtil.log(String.Format("Admob Rewarded ad granted a reward: {0} {1}", reward.Amount, reward.Type), Keys.ColorGreen);
            });
        }
        else
        {
            ARGManager.manager.hideProcessWaiter();
            ArgUtil.log("Admob Rewarded ad is not ready yet.", Keys.ColorYellow);
            ToastManager.manager.showToastMsg(ARGManager.manager.data.msg_NoReward);
        }
    }

    private void reward_RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            lastRewardShowedType = Keys._REWARD;
            isShowing_Reward = false;
            ArgUtil.log("Admob Rewarded ad full screen content closed.");
            if (hasReward) AdManager.manager.afterReward();
            hasReward = false;
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            isShowing_Reward = false;
            ArgUtil.log("Admob Rewarded ad failed to open full screen content with error : " + error, Keys.ColorRed);
        };
    }

    #endregion

    #region RW Inter --------------------------------------------------------------------

    private RewardedInterstitialAd rewardedInterAd;

    public void reward_Inter_LoadAd()
    {
        hasReward = false;
        if (rewardedInterAd != null)
        {
            ArgUtil.log("Admob Destroying rewarded inter ad.");
            rewardedInterAd.Destroy();
            rewardedInterAd = null;
        }

        ArgUtil.log("Admob Loading rewarded inter ad.");

        var adRequest = new AdRequest();
        RewardedInterstitialAd.Load(ARGManager.manager.data.testMode ? testid_rewardinter : realid_rewardinter, adRequest, (RewardedInterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                AdManager.manager.requestReward(AdManager.manager.reward_provider);
                ArgUtil.log("Admob Rewarded inter ad failed to load an ad with error : " + error, Keys.ColorRed);
                return;
            }
            if (ad == null)
            {
                AdManager.manager.requestReward(AdManager.manager.reward_provider);
                ArgUtil.log("Admob Unexpected error: Rewarded inter load event fired with null ad and null error.", Keys.ColorRed);
                return;
            }

            ArgUtil.log("Admob Rewarded inter ad loaded with response : " + ad.GetResponseInfo(), Keys.ColorGreen);
            rewardedInterAd = ad;
            reward_Inter_RegisterEventHandlers(ad);
            reward_Inter_ShowAd();
        });
    }

    public void reward_Inter_ShowAd()
    {
        if (rewardedInterAd != null && rewardedInterAd.CanShowAd())
        {
            ARGManager.manager.hideProcessWaiter();
            isShowing_Reward = true;
            ArgUtil.log("Admob Showing rewarded inter ad.");
            rewardedInterAd.Show((Reward reward) =>
            {
                hasReward = true;
                ArgUtil.log(String.Format("Admob Rewarded inter ad granted a reward: {0} {1}", reward.Amount, reward.Type), Keys.ColorGreen);
            });
        }
        else
        {
            ARGManager.manager.hideProcessWaiter();
            ArgUtil.log("Admob Rewarded inter ad is not ready yet.", Keys.ColorYellow);
            ToastManager.manager.showToastMsg(ARGManager.manager.data.msg_NoReward);
        }
    }

    private void reward_Inter_RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            lastRewardShowedType = Keys._REWARD_INTER;
            isShowing_Reward = false;
            ArgUtil.log("Admob Rewarded inter ad full screen content closed.");
            if (hasReward) AdManager.manager.afterReward();
            hasReward = false;
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            isShowing_Reward = false;
            ArgUtil.log("Admob Rewarded inter ad failed to open full screen content with error : " + error, Keys.ColorRed);
            ToastManager.manager.showToastMsg(ARGManager.manager.data.msg_NoReward);
        };
    }

    #endregion

    #endregion

    #region Native Overlay ADS----------------------------

    private NativeOverlayAd nativeOverlayAd;
    [Header("-------> Native Overlay")]
    public int nativeads_show_count;
    public bool isNativeLoaded, isShowingNativeAd;
    DateTime lastNativeLoadTime;

    NativeAdOptions Option = new NativeAdOptions
    {
        AdChoicesPlacement = AdChoicesPlacement.BottomLeftCorner,
        MediaAspectRatio = MediaAspectRatio.Any,
        VideoOptions = new VideoOptions()
        {
            ClickToExpandRequested = true,
            CustomControlsRequested = true,
            StartMuted = false
        }
    };

    public NativeTemplateStyle Style = new NativeTemplateStyle
    {
        TemplateId = NativeTemplateId.Medium,
        MainBackgroundColor = Color.white
    };

    public void native_LoadAd()
    {
        isNativeLoaded = false;

        if (IAPManager.manager.hasBannerPurchase) return;
        if (!ARGManager.manager.data.canShowNativeAd) return;
        if (ARGManager.manager.isTiredModeOn()) return;

        if (nativeOverlayAd != null)
        {
            ArgUtil.log("------------------Destroying Native Overlay ad.");
            nativeOverlayAd.Destroy();
            nativeOverlayAd = null;
        }

        ArgUtil.log("-----------------Loading native overlay ad.");
        var adRequest = new AdRequest();
        //Style.TemplateId = ARGManager.manager.data.admob_native_size.Equals(Keys._SMALL) ? NativeTemplateId.Small : NativeTemplateId.Medium;

        NativeOverlayAd.Load(ARGManager.manager.data.testMode ? testid_native : ARGManager.manager.data.admob_NativeId, adRequest, Option, (NativeOverlayAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                ArgUtil.log("----------------------Native Overlay ad failed to load an ad with error : " + error, Keys.ColorRed);
                return;
            }
            if (ad == null)
            {
                ArgUtil.log("------------------Unexpected error: Native Overlay ad load event fired with null ad and null error.", Keys.ColorRed);
                return;
            }

            ArgUtil.log("--------------------Native Overlay ad loaded with response : " + ad.GetResponseInfo(), Keys.ColorGreen);
            nativeOverlayAd = ad;
            isNativeLoaded = true;
            lastNativeLoadTime = DateTime.Now;
        });
    }

    public void showNativeAd()
    {
        if (!canshowNativeAd()) return;
        if (nativeOverlayAd != null)
        {
            AdManager.manager.hideBanner();
            if (nativeads_show_count == ARGManager.manager.data.nativeads_show_count)
            {
                ArgUtil.log("-----------------Rendering Native Overlay ad.");
                nativeOverlayAd.RenderTemplate(Style, ARGManager.manager.data.showAtBottom_NativeAd ? AdPosition.Bottom : AdPosition.Top);
                isShowingNativeAd = true;
            }
            else
            {
                ArgUtil.log("-----------------Showing Native Overlay ad.");
                nativeOverlayAd.Show();
                isShowingNativeAd = true;
            }
            nativeads_show_count--;
        }
    }

    public void native_HideAd()
    {
        if (nativeOverlayAd != null)
        {
            isShowingNativeAd = false;
            ArgUtil.log("---------------------Hiding Native Overlay ad.");
            nativeOverlayAd.Hide();
            AdManager.manager.showBanner();

            if (nativeads_show_count < 0)
            {
                if (lastNativeLoadTime != null)
                {
                    TimeSpan diff = DateTime.Now - lastNativeLoadTime;
                    if (diff.TotalSeconds > ARGManager.manager.data.admob_native_reload_interval)
                    {
                        nativeads_show_count = ARGManager.manager.data.nativeads_show_count;
                        native_LoadAd();
                    }
                }
            }
        }
    }

    public bool canshowNativeAd()
    {
        if (IAPManager.manager.hasBannerPurchase) return false;
        if (!isNativeLoaded) return false;
        if (!ARGManager.manager.data.canShowNativeAd) return false;
        if (ARGManager.manager.isTiredModeOn()) return false;

        return true;
    }

    public void forceDestroy_native()
    {
        if (nativeOverlayAd != null)
        {
            ArgUtil.log("------------------Force Destroying Native Overlay ad.");
            nativeOverlayAd.Destroy();
            nativeOverlayAd = null;
        }
    }

    #endregion
}