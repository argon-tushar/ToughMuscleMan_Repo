using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public Button RemovedAdsBtn;
    public Button soundbutton;
    public Sprite on, off;
    public Button musicbutton;
    public Sprite onbutton, offbutton;
    public Button VibrationBtn;
    public Sprite onvibration, offvibration;
    public Text leveltextHome, CointextSetting, CoinScoreHomeText, LevelTextSetting, CoinScoreStoreText, CoinScoreSatisfiedCharacterText;
    public Sprite GymBtn1, GymBtn2, GymBtn3, GymBtn4;
    public GameObject GymBtn;

    public GameObject HomeCanvas, SettingCanvas, LevelCanvas, StoreCanvas, SatisfiedCharacterCanvas;
    public RectTransform SettingPopup, SettingTop, SettingMiddle;
    public List<GameObject> BgObj = new List<GameObject>();
    public GameObject Face1, Face2, Face3;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        IAPManager.manager.RegisterRemoveAdsButton(RemovedAdsBtn);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (PlayerPrefs.GetString("LevelAkvar") == "")
        {
            PlayerPrefs.SetFloat("FillLevelUi", 0);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("CharacterLevel", 1);
            PlayerPrefs.SetInt("1", 1);
            PlayerPrefs.SetInt("Soundoikopkopopl", 1);
            PlayerPrefs.SetInt("Musicjgfjfjjk", 1);
            PlayerPrefs.SetInt("Vibrationlkdsdkl", 1);
            PlayerPrefs.SetFloat("WinFirstCount", 100 / 10);
            PlayerPrefs.SetFloat("WinFirstNow", 0);
            PlayerPrefs.SetFloat("Coin", 0);
            PlayerPrefs.SetString("LevelAkvar", "Abcd");
        }

        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1) soundbutton.GetComponent<Image>().sprite = on;
        else soundbutton.GetComponent<Image>().sprite = off;

        if (PlayerPrefs.GetInt("Musicjgfjfjjk") == 1) musicbutton.GetComponent<Image>().sprite = onbutton;
        else musicbutton.GetComponent<Image>().sprite = offbutton;

        if (PlayerPrefs.GetInt("Vibrationlkdsdkl") == 1) VibrationBtn.GetComponent<Image>().sprite = onvibration;
        else VibrationBtn.GetComponent<Image>().sprite = offvibration;

        CallStoreManager();
        SatisfiedCharacter();
    }

    public void SatisfiedCharacter()
    {
        if (PlayerPrefs.GetInt("Level") > 10 && PlayerPrefs.GetInt("Level") < 21)
        {
            Face1.SetActive(false);
            Face2.SetActive(true);
            Face3.SetActive(true);


        }
        else if (PlayerPrefs.GetInt("Level") > 20 && PlayerPrefs.GetInt("Level") < 30)
        {
            Face1.SetActive(false);
            Face2.SetActive(false);
            Face3.SetActive(true);

        }
        else if (PlayerPrefs.GetInt("Level") > 30)
        {
            Face1.SetActive(false);
            Face2.SetActive(false);
            Face3.SetActive(false);

        }
        else if (PlayerPrefs.GetInt("Level") <= 10 && PlayerPrefs.GetInt("Level") >= 0)
        {
            Face1.SetActive(true);
            Face2.SetActive(true);
            Face3.SetActive(true);

        }
    }

    public void CallStoreManager()
    {
        for (int i = 0; i < BgObj.Count; i++)
        {

            string abc = BgObj[i].name;

            if (PlayerPrefs.GetString(abc) == "isa")
            {
                BgObj[i].transform.GetChild(0).gameObject.SetActive(false);
                BgObj[i].transform.GetChild(1).gameObject.SetActive(false);
                BgObj[i].transform.GetChild(2).gameObject.SetActive(false);
                BgObj[i].transform.GetChild(3).gameObject.SetActive(true);
                BgObj[i].transform.GetChild(4).gameObject.SetActive(false);

                if (abc == PlayerPrefs.GetString("BackgroundNo"))
                {
                    BgObj[i].transform.GetChild(0).gameObject.SetActive(false);
                    BgObj[i].transform.GetChild(1).gameObject.SetActive(true);
                    BgObj[i].transform.GetChild(2).gameObject.SetActive(true);
                    BgObj[i].transform.GetChild(3).gameObject.SetActive(false);
                    BgObj[i].transform.GetChild(3).gameObject.SetActive(false);
                }

            }
        }

    }
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (SettingCanvas.activeSelf == true)
            {
                OffSetting();
            }
            else if (LevelCanvas.activeSelf == true)
            {
                OffLevel();
            }
            else if (SettingCanvas.activeSelf == false && LevelCanvas.activeSelf == false && StoreCanvas.activeSelf == false && SatisfiedCharacterCanvas.activeSelf == false)
            {
                Application.Quit();
            }
            else if (StoreCanvas.activeSelf == true)
            {
                OffStore();
            }
            else if (SatisfiedCharacterCanvas.activeSelf == true)
            {
                OffSatisfiedCharacter();
            }
        }


        leveltextHome.text = "LEVEL " + PlayerPrefs.GetInt("Level").ToString();
        LevelTextSetting.text = "LEVEL " + PlayerPrefs.GetInt("Level").ToString();
        CointextSetting.text = Mathf.Round(PlayerPrefs.GetFloat("Coin")).ToString();
        CoinScoreHomeText.text = Mathf.Round(PlayerPrefs.GetFloat("Coin")).ToString();
        CoinScoreStoreText.text = Mathf.Round(PlayerPrefs.GetFloat("Coin")).ToString();
        CoinScoreSatisfiedCharacterText.text = Mathf.Round(PlayerPrefs.GetFloat("Coin")).ToString();
    }

    public void OnSetting()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();

        if (AdmobManager.manager.canshowNativeAd())
        {
            SettingPopup.SetParent(SettingTop, false);
            AdmobManager.manager.showNativeAd();
        }
        else SettingPopup.SetParent(SettingMiddle, false);

        SettingCanvas.SetActive(true);
        HomeCanvas.SetActive(false);
    }

    public void OffSetting()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();
        HomeCanvas.SetActive(true);
        SettingCanvas.SetActive(false);

        AdmobManager.manager.native_HideAd();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("UiScene");
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

    public void Onsound()
    {
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 0)
        {
            soundbutton.GetComponent<Image>().sprite = on;
            PlayerPrefs.SetInt("Soundoikopkopopl", 1);
        }
        else
        {

            soundbutton.GetComponent<Image>().sprite = off;
            PlayerPrefs.SetInt("Soundoikopkopopl", 0);
        }
    }

    public void Onmusic()
    {

        if (PlayerPrefs.GetInt("Musicjgfjfjjk") == 0)
        {
            musicbutton.GetComponent<Image>().sprite = onbutton;
            PlayerPrefs.SetInt("Musicjgfjfjjk", 1);

        }
        else
        {
            musicbutton.GetComponent<Image>().sprite = offbutton;
            PlayerPrefs.SetInt("Musicjgfjfjjk", 0);
        }
    }

    public void Onvibration()
    {

        if (PlayerPrefs.GetInt("Vibrationlkdsdkl") == 0)
        {
            VibrationBtn.GetComponent<Image>().sprite = onvibration;
            PlayerPrefs.SetInt("Vibrationlkdsdkl", 1);

        }
        else
        {
            VibrationBtn.GetComponent<Image>().sprite = offvibration;
            PlayerPrefs.SetInt("Vibrationlkdsdkl", 0);
        }
    }

    public void OnLeaderBoard()
    {
        Debug.Log("LeaderBoard");
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

    public void OnAchivement()
    {
        Debug.Log("Achivement");
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

    public void OnPrivacyPolicy()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();
        Application.OpenURL(ARGManager.manager.data.privacyPolicyURL);
    }

    public void OnLevel()
    {
        HomeCanvas.SetActive(false);
        LevelCanvas.SetActive(true);
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

    public void OffLevel()
    {
        HomeCanvas.SetActive(true);
        LevelCanvas.SetActive(false);
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

    public void OffStore()
    {
        StoreCanvas.SetActive(false);
        HomeCanvas.SetActive(true);
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

    public void OnStore()
    {
        HomeCanvas.SetActive(false);
        StoreCanvas.SetActive(true);
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

    public void OnSatisfiedCharacter()
    {
        SatisfiedCharacterCanvas.SetActive(true);
        HomeCanvas.SetActive(false);
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

    public void OffSatisfiedCharacter()
    {
        SatisfiedCharacterCanvas.SetActive(false);
        HomeCanvas.SetActive(true);
        FindObjectOfType<SoundManager>().Onbuttonclick();
    }

}
