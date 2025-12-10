using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADDoubeScoreTimer : MonoBehaviour
{
    private static ADDoubeScoreTimer instance;

    public static float timer;

    void Awake()
    {
        if (ADDoubeScoreTimer.instance != null)
        {
            Destroy(gameObject);    
        }
    }
    
    void Start()
    {

        DontDestroyOnLoad(gameObject);
        instance = this;
    }
        

    private void OnEnable()
    {
        

        if (PlayerPrefs.GetFloat("TimerDouble") > 0)
        {
            timer = PlayerPrefs.GetFloat("TimerDouble");
        }
        else
        {
            timer = 0;
        }
    }



    void Update()
    {
        
        if (timer >= 0)
        {

           timer -= Time.deltaTime;
           
           
            PlayerPrefs.SetFloat("TimerDouble", timer);
        }
        else if (timer <= 0)
        {
           
            timer = 0;
            PlayerPrefs.SetFloat("TimerDouble", timer);
            
        }


    }
}
