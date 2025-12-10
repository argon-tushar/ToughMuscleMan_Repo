using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {


    public static MusicManager instance;
    public AudioClip gameMusic;
    private AudioSource audioSource;

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
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 1f;
        audioSource.clip = gameMusic;

        audioSource.Play();
    }
    void Update()
    {
        if (PlayerPrefs.GetInt("Musicjgfjfjjk") == 1)
        {
            audioSource.volume = 1f;
        }
        else
        {
            audioSource.volume = 0;
        }
    }
}
