using System;
using UnityEngine;

[Serializable]
public class DataModel
{
    [HideInInspector]
    public string GameSpecific = "----------------------------------- Game Specific ------------------------------------";
    [Header("<color=#B5FF9B>" + "Game Specific" + "<color>")]
    public bool Show_ShopButton;
    public bool ShowInterOnStoreClose;

    [HideInInspector]
    public string IAPSetting = ">>>>>>>>>>  It could be : monthly, weekly, lifetime => any of them case sensitive <<<<<<<<<<<<";
    [Header("<color=#B5FF9B>" + "IN APP PURCHASE" + "<color>")]
    public string IAPDuration;
    public bool hasIAP;

    [HideInInspector]
    public string ADS_MAIN_SETTING = "----------------------------------- ADS MAIN SETTING ------------------------------------";
    [Header("<color=#B5FF9B>" + "ADs Main Settings" + "<color>")]
    public bool isCheat;
    public bool testMode;
    public bool can_ShowAds;
    public int reviewCounter;

    [HideInInspector]
    public string TIRED = "----------------------------------- TIRED PANEL SETTING ------------------------------------";
    [Header("<color=#B5FF9B>" + "Tired Panel" + "<color>")]
    public int showAdsTiredPopAfter;
    public int tiredmod_ads_disabletime;

    [HideInInspector]
    public string AD_Provider = "----------------------------------- ADs Provide ------------------------------------";
    [Header("<color=#B5FF9B>" + "AD Provider" + "<color>")]
    public string provider_AppOpen;
    public string provider_Banner;
    public string provider_InterAd;
    public string provider_RewardedAd;

    [HideInInspector]
    public string Banner_Setting = "----------------------------------- Banner_Setting ------------------------------------";
    [Header("<color=#B5FF9B>" + "Banner Settings" + "<color>")]
    public bool isBannerBottom;
    public int maxBannerAttempt;
    public float stopbanner_request_afterfails_time;
    public bool showBannerBG;
    public string banner_bg_color, banner_close_icon_color;
    public float banner_bg_transparency;
    public string banner_bordercolor;
    public float banner_border_transparency, banner_border_size;
    public float banner_bg_height;

    [HideInInspector]
    public string Inter_Settings = "----------------------------------- Inter_Settings ------------------------------------";
    [Header("<color=#B5FF9B>" + "Inter Settings" + "<color>")]
    public bool is_Alternate_Interstitial_Ads;
    public float interIntervalTime;
    public int inter_show_after_level;
    public int forceInterSkipNumbers;
    public int numOfLevelsToSkip_inter;

    [HideInInspector]
    public string Reward_Settings = "----------------------------------- Reward_Settings ------------------------------------";
    [Header("<color=#B5FF9B>" + "Reward Settings" + "<color>")]
    public bool showAlternate_Rewarded_Ads;

    [HideInInspector]
    public string Admob_Settings = "----------------------------------- Admob_Settings ------------------------------------";
    [Header("<color=#B5FF9B>" + "AdMob Settings" + "<color>")]
    public bool show_appopen_onlaunch;
    public bool admob_banner_isadaptive;
    public bool admob_banner_iscollapsible;
    public bool showAdmob_rewardedinter;
    public bool alternate_Admob_Rw_RwInter_Ads;

    [HideInInspector]
    public string NativeOptions = "----------------------------------- Native Ads Setting ------------------------------------";
    [Header("<color=#B5FF9B>" + "Native Ads Setting" + "<color>")]
    public bool canShowNativeAd;
    public bool showAtBottom_NativeAd;
    public string admob_native_size;
    public float admob_native_reload_interval;
    public int nativeads_show_count;

    [HideInInspector]
    public string Admob_IDs = "----------------------------------- Admob_IDs ------------------------------------";
    [Header("<color=#B5FF9B>" + "AdMob IDs" + "<color>")]
    public string admob_App_ID;
    public string admob_AppOpenId;
    public string admob_BannerId;
    public string admob_InterId;
    public string admob_RewardedInterId;
    public string admob_RewardedVideo_AdId;
    public string admob_NativeId;

    [HideInInspector]
    public string Unity_IDs_Android = "----------------------------------- Unity IDs ------------------------------------";
    [Header("<color=#B5FF9B>" + "Unity IDs" + "<color>")]
    public string unity_GameID;
    public string unity_unitID_banner;
    public string unity_unitID_inter;
    public string unity_unitID_reward;

    [HideInInspector]
    public string Others = "----------------------------------- Others ------------------------------------";
    [Header("<color=#B5FF9B>" + "Others" + "<color>")]
    public string privacyPolicyURL;
    public bool showlogs;
    public bool testconsent;

    [HideInInspector]
    public string Version = "----------------------------------- Version ------------------------------------";
    [Header("<color=#B5FF9B>" + "Version" + "<color>")]
    public string currentversion;
    public bool canshowUpdate;
    public bool mustUpdate;
    public string myStoreLink;

    [HideInInspector]
    public string ToastMessage = "----------------------------------- Toast Message ------------------------------------";
    [Header("<color=#B5FF9B>" + "Toast Message" + "<color>")]
    public float toastmsg_hidetime;
    public float toastmsgPos;
    public string toastmsg_bgcolor;
    public float toastmsg_bgalpha;
    public string toastmsg_textcolor;
    public string toastmsg_bordercolor;
    public float toastmsg_borderalpha;
    public float toastmsg_bordersize_x;
    public float toastmsg_bordersize_y;

    [HideInInspector]
    public string Messages = "----------------------------------- Messages ------------------------------------";
    [Header("<color=#B5FF9B>" + "Messages" + "<color>")]
    public string msg_NoInternetConnection;
    public string msg_NoReward;
}