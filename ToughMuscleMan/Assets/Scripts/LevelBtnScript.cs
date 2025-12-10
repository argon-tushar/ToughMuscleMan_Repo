using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBtnScript : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnSelectedLevel()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();

        PlayerPrefs.SetFloat("FillLevelUi", 0);

        int loadlevel = int.Parse(this.gameObject.name);
        int Startlevel = PlayerPrefs.GetInt("Level");

        PlayerPrefs.SetInt("Level", loadlevel);
        PlayerPrefs.SetInt((Startlevel).ToString(), 2);
        PlayerPrefs.SetInt(this.gameObject.name, 1);
        
        SceneManager.LoadScene("UiScene");
    }
}
