using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    private AudioSource audioSource;
    public AudioClip buttonclickclip;
    public AudioClip LevelWin;
    public AudioClip LevelLose;
    public AudioClip NetworkAlert;
    public AudioClip Coin5x;
    public AudioClip BuyItem;
    public AudioClip MeterStop;
    public List<AudioClip> ManSound = new List<AudioClip>();
    public bool SoundRepeat = false;
    int SelectedIndex = 0;



    void Awake()
    {
        if (SoundManager.instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f;

    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {
            audioSource.volume = 1;
        }
        else
        {
            audioSource.volume = 0;
        }
    }

    public void Onbuttonclick()
    {
        FindObjectOfType<VibrationManager>().FirstLite();
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {

            audioSource.clip = buttonclickclip;
            audioSource.Play();
        }
    }

    public void OnLevelWin()
    {
        FindObjectOfType<VibrationManager>().FourthLite();
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {
            audioSource.clip = LevelWin;
            audioSource.Play();
        }
    }
    public void OnLevelLose()
    {
        FindObjectOfType<VibrationManager>().FourthLite();
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {
            audioSource.clip = LevelLose;
            audioSource.Play();
        }
    }
    public void OnAlert()
    {
        FindObjectOfType<VibrationManager>().SecondLite();
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {
            audioSource.clip = NetworkAlert;
            audioSource.Play();
        }
    }
    public void OnCoin5x()
    {
        FindObjectOfType<VibrationManager>().SecondLite();
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {
            audioSource.clip = Coin5x;
            audioSource.Play();
        }
    }

    public void OnBuyItem()
    {
        FindObjectOfType<VibrationManager>().SecondLite();
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {
            audioSource.clip = BuyItem;
            audioSource.Play();
        }
    }

    public void OnMeterStop()
    {
        FindObjectOfType<VibrationManager>().SecondLite();
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {
            audioSource.clip = MeterStop;
            audioSource.Play();
        }
    }
    public void OnCharacterSound()
    {
        if (PlayerPrefs.GetInt("Soundoikopkopopl") == 1)
        {
            audioSource.clip = ManSound[SelectedIndex];
            audioSource.Play();
        }
    }

    public void OnManSound()
    {
        
        int RandomSound = Random.Range(1, 10);

        if (RandomSound % 9 == 0)
        {
            SelectedIndex = 0;
        }
        else if (RandomSound % 8 == 0)
        {
            SelectedIndex = 1;
        }
        else if (RandomSound % 7 == 0)
        {
            SelectedIndex = 2;
        }
        else if (RandomSound % 6 == 0)
        {
            SelectedIndex = 3;
        }
        else if (RandomSound % 5 == 0)
        {
            SelectedIndex = 4;
        }
        else if (RandomSound % 4 == 0)
        {
            SelectedIndex = 5;
        }
        else if (RandomSound % 3 == 0)
        {
            SelectedIndex = 6;
        }
        else if (RandomSound % 2 == 0)
        {
            SelectedIndex = 7;
        }
        else
        {
            SelectedIndex = 8;
        }
               
    }

}
