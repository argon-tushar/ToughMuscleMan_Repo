using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsentManager : MonoBehaviour
{
    public static ConsentManager manager;
    private void Awake() => manager = this;

    [SerializeField] Button btnprivacy;

    private void Start()
    {
        //ResetConsentInformation();
        if (btnprivacy != null)
        {
            btnprivacy.onClick.AddListener(UpdatePrivacyButton);
            btnprivacy.interactable = false;
        }
    }

    internal static List<string> TestDeviceIds = new List<string>()
        {
            AdRequest.TestDeviceSimulator,
#if UNITY_IPHONE
            "",
#elif UNITY_ANDROID
            "4A65370603333CA695AE1F0666BCA038"
#endif
        };

    public bool CanRequestAds => ConsentInformation.CanRequestAds();

    public void checkForConsentAndContinue()
    {
        Loader.manager.setLoader(5f);
        GatherConsent((bool error) =>
        {
            if (!error)
            {
                if (CanRequestAds) ArgUtil.log("Yes, you can request ads. You have consent for ads", Keys.ColorGreen);
                else ArgUtil.log("No, you can request ad, You have no-consent for ads", Keys.ColorRose);

                Loader.manager.setLoader(15f);
                AdManager.manager.initAds(CanRequestAds);
            }
        });
    }

    void GatherConsent(Action<bool> error)
    {
        ArgUtil.log("Gathering consent...", Keys.ColorYellow);

        var requestParameters = new ConsentRequestParameters
        {
            ConsentDebugSettings = new ConsentDebugSettings
            {
                DebugGeography = (ARGManager.manager.data.testconsent) ? DebugGeography.EEA : DebugGeography.Disabled,
                TestDeviceHashedIds = (ARGManager.manager.data.testconsent) ? TestDeviceIds : null
            }
        };

        ConsentInformation.Update(requestParameters, (FormError updateError) =>
        {
            UpdatePrivacyButton();

            if (updateError != null)
            {
                ArgUtil.log("Consent error...\n" + updateError.Message, Keys.ColorRed);
                error(true);
                return;
            }

            if (CanRequestAds)
            {
                error(false);
                return;
            }

            ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
            {
                ArgUtil.log("Loading consent form...", Keys.ColorWhite);
                UpdatePrivacyButton();
                if (showError != null)
                {
                    ArgUtil.log("Loading consent form error\n" + showError.Message, Keys.ColorRed);
                    error(true);
                }
                else error(false);
            });
        });
    }

    public void ShowPrivacyOptionsForm(Action<bool> error)
    {
        ArgUtil.log("Showing privacy option form", Keys.ColorWhite);

        ConsentForm.ShowPrivacyOptionsForm((FormError showError) =>
        {
            UpdatePrivacyButton();
            if (showError != null)
            {
                ArgUtil.log("Showing privacy option form error\n" + showError.Message, Keys.ColorRed);
                error(true);
            }
            else error(false);

        });
    }

    public void ResetConsentInformation()
    {
        ConsentInformation.Reset();
        UpdatePrivacyButton();
    }

    void UpdatePrivacyButton()
    {
        if (btnprivacy != null)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                btnprivacy.interactable = ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required;
                ArgUtil.log("Privacy button updated >>> " + (ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required));
            });
        }
    }

    public void Onclick_OpenPrivacyOptions()
    {
        ShowPrivacyOptionsForm((bool error) =>
        {
            if (!error)
            {
                if (CanRequestAds)
                {
                    ArgUtil.log("Yes, you can request ads. You have consent for ads", Keys.ColorGreen);
                }
                else
                {
                    ArgUtil.log("No, you can not request ad, You have no-consent for ads", Keys.ColorRose);
                }
            }
        });
    }

}