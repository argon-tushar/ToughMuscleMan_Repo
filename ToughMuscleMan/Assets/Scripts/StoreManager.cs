using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{

    public List<Sprite> Background = new List<Sprite>();
    public GameObject BackGroundHome,BackGroundSetting;

    void Start()
    {
        if (PlayerPrefs.GetString("OnFirstBg") == "")
        {
            PlayerPrefs.SetString("BackgroundNo", "Theme-0");
            PlayerPrefs.SetString(PlayerPrefs.GetString("BackgroundNo"), "isa");


        }
        BackGroundHome.GetComponent<Image>().sprite = Background[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];
        BackGroundSetting.GetComponent<Image>().sprite = Background[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];
         
    }

    void Update()
    {
        
    }
}
