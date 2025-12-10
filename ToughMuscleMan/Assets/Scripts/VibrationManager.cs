using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    private MoreMountains.NiceVibrations.RegularPresetsDemoManager vibro;

    private void Start()
    {
        vibro = FindObjectOfType<MoreMountains.NiceVibrations.RegularPresetsDemoManager>();
    }
    public void FirstLite()
    {
        //selection
        if (PlayerPrefs.GetInt("Vibrationlkdsdkl") == 1)
        {
            vibro.SelectionButton();
        }
    }

    public void SecondLite()
    {
        //rigid
        if (PlayerPrefs.GetInt("Vibrationlkdsdkl") == 1)
        {
            vibro.RigidButton();
        }
    }

    public void ThirdLite()
    {
        //light
        if (PlayerPrefs.GetInt("Vibrationlkdsdkl") == 1)
        {
            vibro.LightButton();
        }
    }

    public void FourthLite()
    {
        //medium
        if (PlayerPrefs.GetInt("Vibrationlkdsdkl") == 1)
        {
            vibro.MediumButton();
        }
    }
}
