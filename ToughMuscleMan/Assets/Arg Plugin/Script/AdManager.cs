using Coffee.UIExtensions;
using DG.Tweening;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public static AdManager manager;

    private void Awake() => manager = this;

    private void Start()
    {
        getAllBannerIgnorePanel();
        SceneManager.activeSceneChanged += (a, b) => getAllBannerIgnorePanel();
    }

    #region Initialise ----------------------

    private static bool? isInitialized;

    public async void initAds(bool hasconsent)
    {
        Loader.manager.setLoader(UnityEngine.Random.Range(5, 10));
        DOVirtual.DelayedCall(UnityEngine.Random.Range(1, 3), () =>
        {
            DOVirtual.DelayedCall(UnityEngine.Random.Range(4, 7), () =>
            {
                Loader.manager.setLoader(15f);
                DOVirtual.DelayedCall(UnityEngine.Random.Range(1f, 2f), () =>
                {
                    ARGManager.manager.can_start_game = true;
                });
            });
        });

#if UNITY_IOS
        MobileAds.SetiOSAppPauseOnBackground(true);
#endif

        if (!hasconsent) return;
        if (isInitialized.HasValue) return;

        ArgUtil.log("Trying to init ADs...", Keys.ColorYellow);
        isInitialized = true;

        AdmobManager.manager.initSDK();
        UnityAdsManager.manager.initSDK();
        setBannerBGSettings();
    }

    #endregion

    #region Banner----------------

    [Header("------> Banner")]
    [SerializeField] List<IgnoreBannerShow> panel_BannerIgnore;
    public List<string> banner_provider;
    public string current_Banner = Keys._NONE;
    [SerializeField] GameObject bannerBG;
    [SerializeField] RectTransform bannerBGTop, bannerBGBottom;
    [SerializeField] List<Image> bannerbg_images, banner_close_btns;
    [SerializeField] List<UIShadow> bannerborderTop, bannerborderBottom;

    void getAllBannerIgnorePanel()
    {
        panel_BannerIgnore = FindObjectsByType<IgnoreBannerShow>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public bool canShowBannerOnPanel() => !panel_BannerIgnore.Exists(p => p != null && p.gameObject.activeInHierarchy);

    public void showBanner(List<string> provider = null)
    {
        if (IAPManager.manager.hasBannerPurchase) return;
        if (ARGManager.manager.isTiredModeOn()) return;
        if (ConnectionManager.notConnected()) return;
        if (AdmobManager.manager.isShowingNativeAd) return;
        if (!canShowBannerOnPanel())
        {
            ArgUtil.log("Banner ads can not show on current panel.");
            return;
        }

        if (provider == null) banner_provider = ARGManager.manager.data.provider_Banner.Split(",").ToList<string>();
        else banner_provider = provider;

        if (banner_provider.Count == 0) return;
        if (banner_provider[0] == Keys._NONE) return;

        if (banner_provider[0] == Keys._ADMOB)
        {
            banner_provider.RemoveAt(0);
            AdmobManager.manager.banner_ShowAd();
        }
        else if (banner_provider[0] == Keys._UNITY)
        {
            banner_provider.RemoveAt(0);
            UnityAdsManager.manager.banner_ShowAd();
        }
    }

    public void hideBanner()
    {
        if (current_Banner == Keys._NONE) return;

        if (current_Banner == Keys._ADMOB) AdmobManager.manager.banner_HideAd();
        else if (current_Banner == Keys._UNITY) UnityAdsManager.manager.banner_HideAd();
    }

    //------------- BANNER BG
    void setBannerBGSettings()
    {
        if (ARGManager.manager.data.showBannerBG)
        {
            if (ARGManager.manager.data.isBannerBottom) bannerBGBottom.gameObject.SetActive(true);
            else bannerBGTop.gameObject.SetActive(true);

            Color bgcolor = ArgUtil.getColor(ARGManager.manager.data.banner_bg_color, ARGManager.manager.data.banner_bg_transparency);
            bannerbg_images.ForEach(image => image.color = bgcolor);

            Color closebtncolor = ArgUtil.getColor(ARGManager.manager.data.banner_close_icon_color);
            banner_close_btns.ForEach(image => image.color = closebtncolor);

            Color bordercolor = ArgUtil.getColor(ARGManager.manager.data.banner_bordercolor, ARGManager.manager.data.banner_border_transparency);
            bannerborderBottom.ForEach(image =>
            {
                image.effectColor = bordercolor;
                image.effectDistance = new Vector2(0f, ARGManager.manager.data.banner_border_size);
            });
            bannerborderTop.ForEach(image =>
            {
                image.effectColor = bordercolor;
                image.effectDistance = new Vector2(0f, -ARGManager.manager.data.banner_border_size);
            });

            bannerBGTop.DOSizeDelta(new Vector2(bannerBGTop.sizeDelta.x, ARGManager.manager.data.banner_bg_height), 0.01f);
            bannerBGBottom.DOSizeDelta(new Vector2(bannerBGBottom.sizeDelta.x, ARGManager.manager.data.banner_bg_height), 0.01f);
        }
    }

    public void Onclick_BannerBGClose()
    {
        IAPManager.manager.OnclickBannerBGClose();
    }

    public void showBannerBG() => bannerBG.SetActive(ARGManager.manager.data.showBannerBG);

    public void hideBannerBG() => bannerBG.SetActive(false);

    #endregion

    #region Interstitial----------------------
    [Header("------> Interstitial")]
    public List<string> inter_provider;
    public bool isIntervalOver;
    public Action interFiller;
    public int forceInterSkipNumbers;
    public int interadd_levels_after;
    public bool loadAndShow;

    public void requestInter(List<string> provider = null)
    {
        if (IAPManager.manager.hasVideoPurchase) return;

        if (provider == null)
        {
            inter_provider = ARGManager.manager.data.provider_InterAd.Split(",").ToList<string>();
        }
        else
        {
            inter_provider = provider;
        }

        if (!canRequestInter())
        {
            if (loadAndShow) afterInter();
            return;
        }

        if (provider == null && ARGManager.manager.data.is_Alternate_Interstitial_Ads)
        {
            ARGManager.manager.data.provider_InterAd = alterProvider(inter_provider);
        }

        if (inter_provider[0] == Keys._ADMOB)
        {
            inter_provider.RemoveAt(0);
            AdmobManager.manager.inter_LoadAd();
        }
        else if (inter_provider[0] == Keys._UNITY)
        {
            inter_provider.RemoveAt(0);
            UnityAdsManager.manager.inter_LoadAd();
        }
    }

    Action inter_callback;
    public void showInter(Action callback, bool forceShow = false)
    {
        inter_callback = null;
        inter_callback = callback;

        if (ConnectionManager.notConnected() || IAPManager.manager.hasVideoPurchase)
        {
            afterInter();
            return;
        }

        if (interFiller != null) interFiller.Invoke();
        else
        {
            if (forceShow)
            {
                forceInterSkipNumbers++;
                if (forceInterSkipNumbers <= ARGManager.manager.data.forceInterSkipNumbers) afterInter();
                else
                {
                    forceInterSkipNumbers = 0;
                    ARGManager.manager.showProcessWaiter("Please wait...");
                    loadAndShow = true;
                    requestInter();
                }
            }
            else afterInter();
        }
    }

    public void afterInter()
    {
        StartCoroutine(async_afterInter());
    }

    IEnumerator async_afterInter()
    {
        yield return null;
        ArgUtil.log("After inter callback " + DateTime.Now.ToString(), Keys.ColorRose);
        loadAndShow = false;
        inter_callback?.Invoke();
        interFiller = null;
        ARGManager.manager.hideProcessWaiter();

    }

    bool canRequestInter()
    {
        ArgUtil.log("getset_numOfLevelsToSkip_inter ---------> " + getset_numOfLevelsToSkip_inter);

        if (ARGManager.manager.isTiredModeOn()) return false;
        if (!ARGManager.manager.data.can_ShowAds) return false;
        if (interFiller != null) return false;
        if (inter_provider.Count == 0) return false;
        if (inter_provider[0] == Keys._NONE) return false;
        if (getset_numOfLevelsToSkip_inter > 0) return false;
        if (!loadAndShow)
        {
            if (!isIntervalOver) return false;
            if (interadd_levels_after < ARGManager.manager.data.inter_show_after_level) return false;
        }

        interadd_levels_after = 0;
        return true;
    }

    public int getset_numOfLevelsToSkip_inter
    {
        get => PlayerPrefs.GetInt(nameof(ARGManager.manager.data.numOfLevelsToSkip_inter), ARGManager.manager.data.numOfLevelsToSkip_inter);
        set
        {
            PlayerPrefs.SetInt(nameof(ARGManager.manager.data.numOfLevelsToSkip_inter), value <= -1 ? -1 : value);
        }
    }

    public void startInterInterval()
    {
        isIntervalOver = false;
        CancelInvoke(nameof(releaseInterInterval));
        Invoke(nameof(releaseInterInterval), ARGManager.manager.data.interIntervalTime);
    }

    void releaseInterInterval() => isIntervalOver = true;

    public void setInterCountDown()
    {
        getset_numOfLevelsToSkip_inter--;
        interadd_levels_after++;
    }

    #endregion

    #region Rewarded----------------------
    [Header("------> Rewarded")]
    public List<string> reward_provider;

    Action reward_callback;
    public void showRewardedAD(Action callback)
    {
        if (ConnectionManager.notConnected())
        {
            ToastManager.manager.showToastMsg(ARGManager.manager.data.msg_NoInternetConnection);
            return;
        }

        reward_callback = null;
        reward_callback = callback;
        ARGManager.manager.showProcessWaiter("Please wait...");
        requestReward();
    }

    public void requestReward(List<string> provider = null)
    {
        reward_provider = (provider == null) ? ARGManager.manager.data.provider_RewardedAd.Split(",").ToList<string>() : provider;

        if (provider == null && ARGManager.manager.data.showAlternate_Rewarded_Ads)
        {
            ARGManager.manager.data.provider_RewardedAd = alterProvider(reward_provider);
        }

        if (reward_provider[0] == Keys._ADMOB)
        {
            reward_provider.RemoveAt(0);
            AdmobManager.manager.decide_And_Load_Reward();
        }
        else if (reward_provider[0] == Keys._UNITY)
        {
            reward_provider.RemoveAt(0);
            UnityAdsManager.manager.reward_LoadAd();
        }
        else if (reward_provider[0] == Keys._NONE)
        {
            ToastManager.manager.showToastMsg(ARGManager.manager.data.msg_NoReward);
            ARGManager.manager.hideProcessWaiter();
        }

    }

    public void afterReward()
    {
        StartCoroutine(async_afterReward());
    }

    IEnumerator async_afterReward()
    {
        yield return null;
        ARGManager.manager.hideProcessWaiter();
        ArgUtil.log("Reward Success " + DateTime.Now.ToString(), Keys.ColorRose);
        reward_callback?.Invoke();
    }

    #endregion

    public static bool IsAdsRunning
    {
        get
        {
            if (AdmobManager.manager.isShowing_inter) return true;
            if (UnityAdsManager.manager.isShowing_inter) return true;
            if (AdmobManager.manager.isShowing_Reward) return true;
            if (UnityAdsManager.manager.isShowing_Reward) return true;
            if (AdmobManager.manager.isShowing_Appopen) return true;
            return false;
        }
    }

    public string alterProvider(List<string> items)
    {
        if (items == null || items.Count < 2) return string.Join(",", items);

        int noneIndex = items.IndexOf(Keys._NONE);
        if (noneIndex == -1 || noneIndex == 0) return string.Join(",", items);

        int startIndex = noneIndex - 1;
        List<string> rotated = new List<string>();

        for (int i = startIndex; i < noneIndex; i++)
            rotated.Add(items[i]);

        for (int i = 0; i < startIndex; i++)
            rotated.Add(items[i]);

        rotated.Add(Keys._NONE);

        return string.Join(",", rotated);
    }
}