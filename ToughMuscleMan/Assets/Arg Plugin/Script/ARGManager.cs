using Firebase.RemoteConfig;
using Firebase;
using System;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;


#if UNITY_ANDROID
using Google.Play.Review;
#endif

#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class ARGManager : MonoBehaviour
{
    public static ARGManager manager;
    public bool can_start_game;
    public GameObject SplashPanel;
    public DataModel data;

    private void Awake()
    {
        manager = this;
        DontDestroyOnLoad(manager);
    }

    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    IEnumerator BeginGame()
    {
        yield return new WaitUntil(() => can_start_game);

        yield return new WaitForSeconds(1f);
        Loader.manager.doTransit(StartGame);
    }

    void StartGame()
    {
        AdManager.manager.showBanner();
        SplashPanel.SetActive(false);

        //Start Code here
        SceneManager.LoadScene("HomeScene");
    }

    #region FETCH IN EDITOR---------------------------------

    [ContextMenu("FIREBASE DATA/Fetch Data")]
    public void fetchFirebaseData()
    {
        InitFirebaseAndFetch();
    }

    [ContextMenu("FIREBASE DATA/Copy Data")]
    public void copyData()
    {
        ArgUtil.log("Data copy done.");
        GUIUtility.systemCopyBuffer = JsonUtility.ToJson(data) ?? string.Empty;
    }

    [ContextMenu("FIREBASE DATA/Clear Data")]
    public void clearData()
    {
        ArgUtil.log("Data clear done.");
        data = null;
    }

    private FirebaseApp app;
    private async Task InitFirebaseAndFetch()
    {
        try
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                ArgUtil.log("Firebase init success", Keys.ColorGreen);
                var defaults = new System.Collections.Generic.Dictionary<string, object>
                {
                    { Keys.param_firebase_data, "{}"}
                };

                await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
                await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
                await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();

                string json = FirebaseRemoteConfig.DefaultInstance.GetValue(Keys.param_firebase_data).StringValue;
                ArgUtil.log(json, Keys.ColorGold);
                try
                {
                    data = JsonUtility.FromJson<DataModel>(json);
                }
                catch (Exception ex)
                {
                    ArgUtil.log("Json load failed\n\n" + ex, Keys.ColorRed);
                }
            }
            else
            {
                ArgUtil.log("Firebase init fail, check log to get more details.", Keys.ColorRed);
            }
        }
        catch (Exception ex)
        {
            ArgUtil.log("Remote Config fetch / parse failed " + ex, Keys.ColorRed);
        }
    }

    #endregion

    #region Process waiter------------------------

    [Header("Process waiter")]
    [SerializeField] CanvasGroup ProcessPanel;
    [SerializeField] TextMeshProUGUI txtProcessMsg;

    public void showProcessWaiter(string msg = "")
    {
        AdmobManager.manager.native_HideAd();
        txtProcessMsg.SetText(msg);
        DOTween.Kill(ProcessPanel);
        ProcessPanel.gameObject.SetActive(true);
        ProcessPanel.DOFade(1f, 0.3f);
    }

    public void hideProcessWaiter()
    {
        StartCoroutine(asyncHideProcessWaiter());
    }

    IEnumerator asyncHideProcessWaiter()
    {
        yield return null;
        DOTween.Kill(ProcessPanel);
        ProcessPanel.DOFade(0f, 0.3f).OnComplete(() => ProcessPanel.gameObject.SetActive(false));
    }

    #endregion

    #region Inapp Review----------------------

#if UNITY_ANDROID
    private ReviewManager reviewManager;
#endif

    int getsetInappReviewCounter
    {
        get => PlayerPrefs.GetInt(Keys.InappReviewCounter, 0);
        set => PlayerPrefs.SetInt(Keys.InappReviewCounter, value);
    }

    public void ask_for_Inappreview()
    {
        getsetInappReviewCounter++;
        ArgUtil.log("InAppReview Counter " + getsetInappReviewCounter + " == " + data.reviewCounter);
        if (getsetInappReviewCounter == data.reviewCounter)
        {
#if UNITY_ANDROID
            StartCoroutine(co_InAppReview());
#else
            Device.RequestStoreReview();
#endif
        }
    }

    private IEnumerator co_InAppReview()
    {
        yield return null;
#if UNITY_ANDROID

        reviewManager = new ReviewManager();
        ArgUtil.log("InAppReview Requesting...", Keys.ColorYellow);

        var request = reviewManager.RequestReviewFlow();
        yield return request;

        if (request.Error != ReviewErrorCode.NoError)
        {
            ArgUtil.log("InAppReview Request failed: " + request.Error, Keys.ColorRed);
            yield break;
        }

        var reviewInfo = request.GetResult();

        var launch = reviewManager.LaunchReviewFlow(reviewInfo);
        yield return launch;

        if (launch.Error != ReviewErrorCode.NoError)
        {
            ArgUtil.log("InAppReview Launch failed: " + launch.Error, Keys.ColorRed);
            yield break;
        }
#else
        ArgUtil.log("InAppReview In-app review only works on Android devices (Play build)", Keys.ColorYellow);
#endif
    }

    #endregion

    #region Tired Panel-----------------------

    [Header("Process waiter")]
    [SerializeField] CanvasGroup TiredPanel;
    [SerializeField] RectTransform subpanelTired, subpanelWatchAds;
    [SerializeField] int interAdsShowCount;

    public void checkAndShowTiredPanel()
    {
        if (interAdsShowCount == data.showAdsTiredPopAfter) showTiredPanel(subpanelTired);
        else if (interAdsShowCount == (data.showAdsTiredPopAfter * 2)) showTiredPanel(subpanelWatchAds);
    }

    public void OnclickViewAllPlans()
    {
        OnButtonClickSound();
        hideTiredPanel(subpanelTired);
        Loader.manager.doTransit(() =>
        {
            IAPManager.manager.ShowPopup_RemoveAds();
        });
    }

    void showTiredPanel(RectTransform subpanel)
    {
        subpanel.gameObject.SetActive(true);
        TiredPanel.interactable = false;
        TiredPanel.gameObject.SetActive(true);
        TiredPanel.DOFade(1f, 0.4f).SetUpdate(true);
        subpanel.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.OutBack, 2).SetUpdate(true);
        subpanel.DORotate(new Vector3(0, 0, 10), 0.3f).SetUpdate(true).OnComplete(() =>
        {
            subpanel.DORotate(new Vector3(0, 0, 0), 0.2f).SetUpdate(true);
            TiredPanel.interactable = true;
        });
    }

    public void Onclick_BtnLater(RectTransform subpanel)
    {
        OnButtonClickSound();
        hideTiredPanel(subpanelTired);
    }

    public void hideTiredPanel(RectTransform subpanel)
    {
        TiredPanel.interactable = false;
        TiredPanel.DOFade(0f, 0.1f).SetDelay(0.4f).SetUpdate(true).OnComplete(() =>
        {
            TiredPanel.gameObject.SetActive(false);
        });
        subpanel.DOAnchorPos(new Vector2(500f, 0f), 0.5f).SetEase(Ease.InBack, 2).SetUpdate(true);
        subpanel.DORotate(new Vector3(0, 0, 10f), 0.4f).SetUpdate(true).OnComplete(() =>
        {
            subpanel.DORotate(Vector3.zero, 0.1f).SetUpdate(true);
            subpanel.gameObject.SetActive(false);
        });
    }

    public void OnclickWatchAds_TiredPopup()
    {
        OnButtonClickSound();
        AdManager.manager.showRewardedAD(OnSuccessTiredWatchAd);
    }

    public void OnSuccessTiredWatchAd()
    {
        hideTiredPanel(subpanelWatchAds);
        getsetTiredWatchAdsTime = DateTime.Now.ToString("o");
        AdManager.manager.hideBanner();
    }

    public bool isTiredModeOn()
    {
        if (getsetTiredWatchAdsTime.Equals(Keys._NONE)) return false;

        DateTime storetime = DateTime.Parse(getsetTiredWatchAdsTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        TimeSpan timediff = DateTime.Now - storetime;
        if (timediff.TotalMinutes > data.tiredmod_ads_disabletime)
        {
            getsetTiredWatchAdsTime = Keys._NONE;
            return false;
        }

        ArgUtil.log("TIred Mode is still on " + timediff.TotalSeconds);
        return true;
    }

    string getsetTiredWatchAdsTime
    {
        get => PlayerPrefs.GetString(Keys.TiredWatchAdsTime, Keys._NONE);
        set => PlayerPrefs.SetString(Keys.TiredWatchAdsTime, value);
    }

    public int getsetAfterInterClosed
    {
        get => interAdsShowCount;
        set
        {
            interAdsShowCount = value;
            Invoke(nameof(checkAndShowTiredPanel), 0.5f);
        }
    }

    #endregion

    public void OnButtonClickSound()
    {
        //SoundManager.Instance.BtnClickSound();
    }
}