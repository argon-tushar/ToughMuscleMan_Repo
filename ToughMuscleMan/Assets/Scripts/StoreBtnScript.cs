using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreBtnScript : MonoBehaviour
{

    void Start()
    {

    }


    void Update()
    {

    }

    public void OnSelectBackground()
    {
        PlayerPrefs.SetInt("BuyValue", int.Parse(this.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text));

        if (PlayerPrefs.GetFloat("Coin") >= PlayerPrefs.GetInt("BuyValue"))
        {

            PlayerPrefs.SetString("BackgroundNo", this.gameObject.name);
            PlayerPrefs.SetString("OnFirstBg", "setBg");

            FindObjectOfType<StoreManager>().BackGroundHome.GetComponent<Image>().sprite = FindObjectOfType<StoreManager>().Background[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];
            FindObjectOfType<StoreManager>().BackGroundSetting.GetComponent<Image>().sprite = FindObjectOfType<StoreManager>().Background[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];


            PlayerPrefs.SetString(PlayerPrefs.GetString("BackgroundNo"), "isa");

            for (int i = 0; i < FindObjectOfType<HomeManager>().BgObj.Count; i++)
            {

                string abc = FindObjectOfType<HomeManager>().BgObj[i].name;

                if (PlayerPrefs.GetString(abc) == "isa")
                {
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(0).gameObject.SetActive(false);
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(1).gameObject.SetActive(false);
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(2).gameObject.SetActive(false);
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(3).gameObject.SetActive(true);
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(4).gameObject.SetActive(false);
                }
                else
                {
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(0).gameObject.SetActive(true);
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(1).gameObject.SetActive(false);
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(2).gameObject.SetActive(false);
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(3).gameObject.SetActive(false);
                    FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(4).gameObject.SetActive(true);
                }
            }

            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(2).gameObject.SetActive(true);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);


            FindObjectOfType<SoundManager>().OnBuyItem();
            PlayerPrefs.SetFloat("Coin", PlayerPrefs.GetFloat("Coin") - PlayerPrefs.GetInt("BuyValue"));
        }
        else
        {
            GameObject NoInternet = Instantiate(Resources.Load("NoenoughCoin") as GameObject);
            NoInternet.transform.SetParent(GameObject.Find("StoreCanvas/SafeArea").transform);

            Destroy(NoInternet, 1f);
            FindObjectOfType<SoundManager>().OnAlert();
        }

    }

    public void OnSelectBackgroundWatchAd()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();
        PlayerPrefs.SetString("TempBg", this.gameObject.name);
        AdManager.manager.showRewardedAD(AfterVideoBg);
    }

    public void AfterVideoBg()
    {
        string Bgvideo = PlayerPrefs.GetString("TempBg");
        GameObject BgNameVideo = GameObject.Find(Bgvideo);
        PlayerPrefs.SetString("BackgroundNo", Bgvideo);
        PlayerPrefs.SetString("OnFirstBg", "setBg");

        FindObjectOfType<StoreManager>().BackGroundHome.GetComponent<Image>().sprite = FindObjectOfType<StoreManager>().Background[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];
        FindObjectOfType<StoreManager>().BackGroundSetting.GetComponent<Image>().sprite = FindObjectOfType<StoreManager>().Background[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];


        PlayerPrefs.SetString(PlayerPrefs.GetString("BackgroundNo"), "isa");

        for (int i = 0; i < FindObjectOfType<HomeManager>().BgObj.Count; i++)
        {

            string abc = FindObjectOfType<HomeManager>().BgObj[i].name;

            if (PlayerPrefs.GetString(abc) == "isa")
            {
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(0).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(1).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(2).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(3).gameObject.SetActive(true);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            else
            {
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(0).gameObject.SetActive(true);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(1).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(2).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(4).gameObject.SetActive(true);
            }
        }

        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);
        this.transform.GetChild(2).gameObject.SetActive(true);
        this.transform.GetChild(3).gameObject.SetActive(false);
        this.transform.GetChild(4).gameObject.SetActive(false);
        FindObjectOfType<SoundManager>().OnBuyItem();
    }

    public void OnClickSelected()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();

        PlayerPrefs.SetString("BackgroundNo", this.gameObject.name);
        FindObjectOfType<StoreManager>().BackGroundHome.GetComponent<Image>().sprite = FindObjectOfType<StoreManager>().Background[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];
        FindObjectOfType<StoreManager>().BackGroundSetting.GetComponent<Image>().sprite = FindObjectOfType<StoreManager>().Background[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];


        for (int i = 0; i < FindObjectOfType<HomeManager>().BgObj.Count; i++)
        {

            string abc = FindObjectOfType<HomeManager>().BgObj[i].name;

            if (PlayerPrefs.GetString(abc) == "isa")
            {
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(0).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(1).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(2).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(3).gameObject.SetActive(true);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            else
            {

                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(0).gameObject.SetActive(true);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(1).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(2).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(3).gameObject.SetActive(false);
                FindObjectOfType<HomeManager>().BgObj[i].transform.GetChild(4).gameObject.SetActive(true);

            }
        }
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);
        this.transform.GetChild(2).gameObject.SetActive(true);
        this.transform.GetChild(3).gameObject.SetActive(false);
        this.transform.GetChild(4).gameObject.SetActive(false);


    }

}
