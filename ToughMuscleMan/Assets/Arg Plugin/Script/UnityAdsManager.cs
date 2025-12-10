using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string gameID;
    [SerializeField] string unitID_banner;
    [SerializeField] string unitID_inter;
    [SerializeField] string unitID_reward;

    public static UnityAdsManager manager;
    [SerializeField] bool isInit;

    void Awake() => manager = this;

    private void RunOnAndroidUiThread(Action action)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                    try { action?.Invoke(); } catch (Exception e) { Debug.LogException(e); }
                }));
            }
        }
        catch (Exception e)
        {
            // If anything goes wrong, fallback to invoking directly (better than dropping call),
            // but log so you can catch threading issues.
            Debug.LogWarning("runOnUiThread failed, invoking action directly.");
            Debug.LogException(e);
            action?.Invoke();
        }
#else
        action?.Invoke();
#endif
    }


    #region Initialization -------------------------

    public void initSDK()
    {
        gameID = ARGManager.manager.data.unity_GameID;
        unitID_banner = ARGManager.manager.data.unity_unitID_banner;
        unitID_inter = ARGManager.manager.data.unity_unitID_inter;
        unitID_reward = ARGManager.manager.data.unity_unitID_reward;

        // Call initialize on Unity main thread (safe), SDK will internally use Android UI thread where needed
        Advertisement.Initialize(gameID, ARGManager.manager.data.testMode, this);
    }

    public void OnInitializationComplete()
    {
        isInit = true;
        ArgUtil.log("Unity SDK init done.", Keys.ColorGreen);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        ArgUtil.log($"Unity SDK init failed: {error} - {message}", Keys.ColorRed);
    }

    #endregion

    #region Banner-------------------------

    [Header("-------> Banner")]
    [SerializeField] bool isloading_Banner;

    public void banner_LoadAd()
    {
        if (isloading_Banner) return;
        isloading_Banner = true;

        RunOnAndroidUiThread(() =>
        {
            AdManager.manager.hideBannerBG();
            Advertisement.Banner.SetPosition(ARGManager.manager.data.isBannerBottom ? BannerPosition.BOTTOM_CENTER : BannerPosition.TOP_CENTER);

            var options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            ArgUtil.log("Unity Loading banner ad...");
            Advertisement.Banner.Load(unitID_banner, options);
        });
    }

    public void banner_ShowAd()
    {
        RunOnAndroidUiThread(() =>
        {
            bool loaded = Advertisement.Banner.isLoaded;

            if (loaded)
            {
                if (!AdManager.manager.canShowBannerOnPanel())
                {
                    ArgUtil.log("Unity Can not show banner on current panel.");
                    banner_HideAd();
                    return;
                }

                ArgUtil.log("Unity Banner Showing");

                AdManager.manager.showBannerBG();
                Advertisement.Banner.Show(unitID_banner);
                AdManager.manager.current_Banner = Keys._UNITY;
            }
            else
            {
                if (!isloading_Banner) banner_LoadAd();
            }
        });
    }

    public void banner_HideAd()
    {
        ArgUtil.log("Unity Banner Hide", Keys.ColorCyan);

        RunOnAndroidUiThread(() =>
        {
            AdManager.manager.hideBannerBG();
            Advertisement.Banner.Hide();
        });
    }

    private void OnBannerLoaded()
    {
        ArgUtil.log("Unity Banner Loaded", Keys.ColorYellow);
        isloading_Banner = false;
        banner_ShowAd();
    }

    private void OnBannerError(string message)
    {
        ArgUtil.log("Unity Banner failed to load\n" + message, Keys.ColorRed);
        isloading_Banner = false;
    }

    #endregion

    #region Interstitial-----------------------------------------

    private InterstitialAd interAd;
    [Header("-------> Interstitial")]
    [SerializeField] bool isloading_inter;
    [SerializeField] bool isAvailable_inter;
    public bool isShowing_inter;

    public void inter_LoadAd()
    {
        if (isloading_inter) return;
        isloading_inter = true;
        isAvailable_inter = false;
        isShowing_inter = false;

        ArgUtil.log("Unity inter loading");
        Advertisement.Load(unitID_inter, this);
    }

    public void inter_ShowAd()
    {
        if (isAvailable_inter)
        {
            ARGManager.manager.hideProcessWaiter();
            ArgUtil.log("Unity inter showing");
            isShowing_inter = true;
            Advertisement.Show(unitID_inter, this);
        }
        else
        {
            AdManager.manager.afterInter();
        }
    }

    #endregion

    #region Rewarded------------------

    [Header("-------> Rewarded")]
    public bool isShowing_Reward;

    public void reward_LoadAd()
    {
        ArgUtil.log("Unity reward loading");
        Advertisement.Load(unitID_reward, this);
    }

    public void reward_ShowAd()
    {
        isShowing_Reward = true;
        ARGManager.manager.hideProcessWaiter();
        ArgUtil.log("Unity reward showing");
        Advertisement.Show(unitID_reward, this);
    }

    #endregion

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(unitID_inter))
        {
            isShowing_inter = false;
            isAvailable_inter = true;
            isloading_inter = false;
            AdManager.manager.startInterInterval();
            AdManager.manager.interFiller = inter_ShowAd;
            ArgUtil.log("Unity Inter Loaded", Keys.ColorGreen);
            if (AdManager.manager.loadAndShow) AdManager.manager.interFiller?.Invoke();
        }
        else if (adUnitId.Equals(unitID_reward))
        {
            ArgUtil.log("Unity Reward Loaded", Keys.ColorGreen);
            reward_ShowAd();
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        if (adUnitId.Equals(unitID_inter))
        {
            isShowing_inter = false;
            isAvailable_inter = false;
            isloading_inter = false;
            ArgUtil.log("Unity Inter Load Failed\n" + message + "\n" + error.ToString(), Keys.ColorRed);
            AdManager.manager.requestInter(AdManager.manager.inter_provider);
        }
        else if (adUnitId.Equals(unitID_reward))
        {
            ArgUtil.log("Unity Reward Load Failed\n" + message + "\n" + error.ToString(), Keys.ColorRed);
            AdManager.manager.requestReward(AdManager.manager.reward_provider);
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        if (adUnitId.Equals(unitID_inter))
        {
            isShowing_inter = false;
            isAvailable_inter = false;
            isloading_inter = false;
            ArgUtil.log("Unity Inter Showing Failed\n" + message + "\n" + error.ToString(), Keys.ColorRed);
            AdManager.manager.afterInter();
        }
        else if (adUnitId.Equals(unitID_reward))
        {
            ArgUtil.log("Unity Reward Showing Failed\n" + message + "\n" + error.ToString(), Keys.ColorRed);
            ToastManager.manager.showToastMsg(ARGManager.manager.data.msg_NoReward);
        }
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(unitID_inter))
        {
            ARGManager.manager.getsetAfterInterClosed++;
            isShowing_inter = false;
            isAvailable_inter = false;
            isloading_inter = false;
            ArgUtil.log("Unity Inter Closed", Keys.ColorYellow);
            AdManager.manager.afterInter();
        }
        else if (adUnitId.Equals(unitID_reward))
        {
            ArgUtil.log("Unity Reward Closed ---> " + showCompletionState.ToString(), Keys.ColorYellow);
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdManager.manager.afterReward();
            }
        }
    }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }
}
