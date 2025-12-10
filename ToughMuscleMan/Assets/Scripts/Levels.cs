using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public List<GameObject> Alllevel = new List<GameObject>();
    public Sprite Lock, UnSelected, Selected;


    void Start()
    {

        

        for (int i = 0; i < Alllevel.Count; i++)
        {
            string le = (i + 1).ToString();
            int level = PlayerPrefs.GetInt(le);
            // Debug.Log(level);
            if (level == 0)
            {
                Alllevel[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = Lock;
                Alllevel[i].transform.GetChild(1).transform.GetComponent<Text>().gameObject.SetActive(false);
            }
            else if (level == 1)
            {
                Alllevel[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = Selected;
                Alllevel[i].transform.GetChild(1).transform.GetComponent<Text>().gameObject.SetActive(true);
            }
            else
            {
                Alllevel[i].transform.GetChild(0).transform.GetComponent<Image>().sprite = UnSelected;
                Alllevel[i].transform.GetChild(1).transform.GetComponent<Text>().gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
