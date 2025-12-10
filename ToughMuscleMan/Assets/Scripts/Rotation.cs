using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
   
    bool Clockwise = true;
    public bool StopArrov = false;
    public bool IsProgress = false;
    float ProgressValue = 0;
    float CoinValue = 0;
    float ProgressTime = 0.01f;

    public float Speed = 1.7f;

    void Start()
    {
        i = 0;
    }

    private void OnEnable()
    {
        transform.eulerAngles = new Vector3(0, 0, 57);

    }

    public GameObject Particles1;
    void Update()
    {
        
        if (StopArrov == false)
        {
            if (Mathf.Round(transform.eulerAngles.z) == 57)
            {
                Clockwise = true;
            }
            if (Mathf.Round(transform.eulerAngles.z) == 302)
            {
                Clockwise = false;
            }

            if (Clockwise)
            {
                transform.Rotate(new Vector3(0, 0, -Speed));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, Speed));
            }
        }




        if (Input.GetMouseButtonDown(0))
        {
            if (FindObjectOfType<UiManager>().UiCanvas.activeSelf == true&& EventSystem.current.currentSelectedGameObject == null)
            {
                if (StopArrov!=true)
                {
                    PlayerPrefs.SetString("handfinger", "handxyz");
                    if (FindObjectOfType<UiManager>().Hand.activeSelf == true)
                    {
                        FindObjectOfType<UiManager>().Hand.SetActive(false);
                    }
                    
                    FindObjectOfType<SoundManager>().OnMeterStop();

                    GameObject Particles = Instantiate(Resources.Load("StarDust") as GameObject);
                    Particles.transform.position = new Vector3(-0.03f, 2.96f, 0);
                    Destroy(Particles, 1f);


            


                    StopArrov = true;
                    StartCoroutine(Waiter());
                    StartCoroutine(arrowWaiter());
                    IsProgress = true;
                    
                    
                    if (Mathf.Round(transform.eulerAngles.z) <= 57 && Mathf.Round(transform.eulerAngles.z) > 41)
                    {
                        
                        StartCoroutine(FailedWaiter());
                        FindObjectOfType<UiManager>().MeterText.text = "0";
                    }
                    else if (Mathf.Round(transform.eulerAngles.z) < 42 && Mathf.Round(transform.eulerAngles.z) > 26.5)
                    {
                        StartCoroutine(OnProgress());
                        

                        Particles1 = Instantiate(Resources.Load("MagicChargeBlue") as GameObject);
                        
                        Particles1.transform.position = new Vector3(0f, 0f, 0);

                        
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 2;

                       
                            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
                            {
                                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                            }
                        
                        ProgressValue = 0.2f;
                        CoinValue = 0.010f;
                        FindObjectOfType<UiManager>().MeterText.text = "20";
                        StartCoroutine(SoundWaiter());

                    }
                    else if (Mathf.Round(transform.eulerAngles.z) < 26.6 && Mathf.Round(transform.eulerAngles.z) > 13.85)
                    {
                        StartCoroutine(OnProgress());
                        

                        Particles1 = Instantiate(Resources.Load("MagicChargeBlue") as GameObject);
                        
                        Particles1.transform.position = new Vector3(0f, 0f, 0);

                        
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 3;

                        
                            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
                            {
                                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                            }
                        
                        ProgressValue = 0.3f;
                        CoinValue = 0.015f;
                        FindObjectOfType<UiManager>().MeterText.text = "50";
                        StartCoroutine(SoundWaiter());
                    }
                    else if (Mathf.Round(transform.eulerAngles.z) < 13.86 && Mathf.Round(transform.eulerAngles.z) > 5.2)
                    {
                        StartCoroutine(OnProgress());
                        

                        Particles1 = Instantiate(Resources.Load("MagicChargeBlue") as GameObject);
                        
                        Particles1.transform.position = new Vector3(0f, 0f, 0);

                        
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 4;

                       
                            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
                            {
                                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                            }
                       
                        ProgressValue = 0.4f;
                        CoinValue = 0.020f;
                        FindObjectOfType<UiManager>().MeterText.text = "80";
                        StartCoroutine(SoundWaiter());
                    }
                    else if (Mathf.Round(transform.eulerAngles.z) < 5.3 && Mathf.Round(transform.eulerAngles.z) >= 0)
                    {
                        StartCoroutine(OnProgress());
                        

                        Particles1 = Instantiate(Resources.Load("MagicChargeBlue") as GameObject);
                        
                        Particles1.transform.position = new Vector3(0f, 0f, 0);

                        
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 6;

                       
                            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
                            {
                                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                            }
                        

                        ProgressValue = 0.5f;
                        CoinValue = 0.025f;
                        FindObjectOfType<UiManager>().MeterText.text = "100";
                        StartCoroutine(SoundWaiter());

                        
                    }
                    else if (Mathf.Round(transform.eulerAngles.z) <= 360 && Mathf.Round(transform.eulerAngles.z) > 353.6)
                    {
                        StartCoroutine(OnProgress());
                        

                        Particles1 = Instantiate(Resources.Load("MagicChargeBlue") as GameObject);
                        
                        Particles1.transform.position = new Vector3(0f, 0f, 0);

                        
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 6;

                        
                            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
                            {
                                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                            }
                       
                        ProgressValue = 0.5f;
                        CoinValue = 0.025f;
                        FindObjectOfType<UiManager>().MeterText.text = "100";
                        StartCoroutine(SoundWaiter());
                    }
                    else if (Mathf.Round(transform.eulerAngles.z) < 353.7 && Mathf.Round(transform.eulerAngles.z) > 345.8)
                    {
                        StartCoroutine(OnProgress());
                        

                        Particles1 = Instantiate(Resources.Load("MagicChargeBlue") as GameObject);
                        
                        Particles1.transform.position = new Vector3(0f, 0f, 0);

                        
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 4;

                        
                            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
                            {
                                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                            }
                       
                        ProgressValue = 0.4f;
                        CoinValue = 0.020f;
                        FindObjectOfType<UiManager>().MeterText.text = "80";
                        StartCoroutine(SoundWaiter());
                    }
                    else if (Mathf.Round(transform.eulerAngles.z) < 345.9 && Mathf.Round(transform.eulerAngles.z) > 333.3)
                    {
                        StartCoroutine(OnProgress());
                        

                        Particles1 = Instantiate(Resources.Load("MagicChargeBlue") as GameObject);
                        
                        Particles1.transform.position = new Vector3(0f, 0f, 0);

                        
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 3;

                        
                            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
                            {
                                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                            }
                        
                        ProgressValue = 0.3f;
                        CoinValue = 0.015f;
                        FindObjectOfType<UiManager>().MeterText.text = "50";
                        StartCoroutine(SoundWaiter());
                    }
                    else if (Mathf.Round(transform.eulerAngles.z) < 333.4 && Mathf.Round(transform.eulerAngles.z) > 318.5)
                    {
                        StartCoroutine(OnProgress());
                        

                        Particles1 = Instantiate(Resources.Load("MagicChargeBlue") as GameObject);
                        
                        Particles1.transform.position = new Vector3(0f, 0f, 0);

                        
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 2;

                        //if (PlayerPrefs.GetInt("Level") < 31)
                        //{
                            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
                            {
                                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                            }
                        //}
                        //else
                        //{
                        //    FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Ch30")) as RuntimeAnimatorController;
                        //}
                        ProgressValue = 0.2f;
                        CoinValue = 0.010f;
                        FindObjectOfType<UiManager>().MeterText.text = "20";
                        StartCoroutine(SoundWaiter());
                    }
                    else if (Mathf.Round(transform.eulerAngles.z) < 318.6 && Mathf.Round(transform.eulerAngles.z) >= 302)
                    {
                        
                        StartCoroutine(FailedWaiter());
                        FindObjectOfType<UiManager>().MeterText.text = "0";
                    }
                    scoreminusvalue =int.Parse(FindObjectOfType<UiManager>().MeterText.text);
                    Scoreminus = (float)scoreminusvalue;

                }
            }
        }
        
    }

    

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(4f);
        
        Destroy(Particles1);
        FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = null;
        IsProgress = false;
        
        CancelInvoke();

        
            if (FindObjectOfType<UiManager>().Character.transform.GetChild(0).name == ("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) + "(Clone)")
            {
                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = Instantiate(Resources.Load("Face" + PlayerPrefs.GetInt("CharacterLevel").ToString())) as RuntimeAnimatorController;
                FindObjectOfType<UiManager>().Character.transform.GetChild(0).GetComponent<Animator>().speed = 1;
            }
        

    }

    IEnumerator arrowWaiter()
    {
        yield return new WaitForSeconds(5f);
        StopArrov = false;
        FindObjectOfType<UiManager>().MeterText.text = "0";
    }
    int i = 0;
    IEnumerator FailedWaiter()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<UiManager>().LevelFailed.SetActive(true);
        FindObjectOfType<UiManager>().UiCanvas.SetActive(false);

        // Level Failed

       /* i++;
        if (i == 1)
        {
            FindObjectOfType<BannerAdScript>().Fullads();
        }*/

        

        FindObjectOfType<SoundManager>().OnLevelLose();

        float now = PlayerPrefs.GetFloat("WinFirstNow");

        int abcd = (int)now;
        FindObjectOfType<UiManager>().FailedText.text = abcd.ToString() + " %";
        FindObjectOfType<UiManager>().playerLose.GetComponent<Image>().fillAmount = now / 100;
        StopArrov = true;
        IsProgress = false;
    }

    int scoreminusvalue=0;
    float Scoreminus = 0;


    IEnumerator SoundWaiter()
    {
        

        if (IsProgress == true)
        {
           
            FindObjectOfType<SoundManager>().OnCharacterSound();

            yield return new WaitForSeconds(1-ProgressValue);
            StartCoroutine(SoundWaiter());
        }
        else
        {
            FindObjectOfType<SoundManager>().OnManSound(); 
           
        }

    }
    bool TextProgress = false;
    bool EmojiProgress = false;



    IEnumerator OnProgress()
    {
        
        if (IsProgress == true)
        {
            
            
            float Abcd = ProgressValue/(ProgressTime * 10000 * 4);
            
            PlayerPrefs.SetFloat("FillLevelUi", PlayerPrefs.GetFloat("FillLevelUi") + Abcd);
            
            double xyzz = (double)scoreminusvalue / 230.0;
            
            Scoreminus = Scoreminus - (float)xyzz;
            if(Scoreminus<=0)
            {
                Scoreminus = 0;
            }
            FindObjectOfType<UiManager>().MeterText.text = Mathf.Round(Scoreminus).ToString();


            if (FindObjectOfType<UiManager>().UiADBtn.transform.GetChild(1).gameObject.activeSelf == false)
            {
                PlayerPrefs.SetFloat("Coin", PlayerPrefs.GetFloat("Coin") + CoinValue);
            }
            else
            {
                PlayerPrefs.SetFloat("Coin", PlayerPrefs.GetFloat("Coin") + CoinValue*2);
            }


            if (TextProgress == false)
            {
                if (Mathf.Round(transform.eulerAngles.z) <= 360 && Mathf.Round(transform.eulerAngles.z) > 353.6 || Mathf.Round(transform.eulerAngles.z) < 5.3 && Mathf.Round(transform.eulerAngles.z) >= 0)
                {
                    if (FindObjectOfType<UiManager>().UiADBtn.transform.GetChild(1).gameObject.activeSelf == false)
                    {
                        GameObject UiScoreText = Instantiate(Resources.Load("ScoreImage") as GameObject);
                        UiScoreText.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                        UiScoreText.transform.localPosition = new Vector2(Random.Range(-371, 371), Random.Range(-200, 460));
                        UiScoreText.transform.localScale = new Vector3(1, 1, 1);
                        Destroy(UiScoreText, 1f);
                        TextProgress = true;
                        StartCoroutine(TextWaiter());
                    }
                    else
                    {
                        GameObject UiScoreText = Instantiate(Resources.Load("ScoreImage") as GameObject);
                        UiScoreText.transform.GetChild(0).transform.GetComponent<Text>().text = "+$0.100";
                        UiScoreText.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                        UiScoreText.transform.localPosition = new Vector2(Random.Range(-371, 371), Random.Range(-200, 460));
                        UiScoreText.transform.localScale = new Vector3(1, 1, 1);
                        Destroy(UiScoreText, 1f);
                        TextProgress = true;
                        StartCoroutine(TextWaiter());
                    }
                }
                else if (Mathf.Round(transform.eulerAngles.z) < 353.7 && Mathf.Round(transform.eulerAngles.z) > 345.8 || Mathf.Round(transform.eulerAngles.z) < 13.86 && Mathf.Round(transform.eulerAngles.z) > 5.2)
                {
                    if (FindObjectOfType<UiManager>().UiADBtn.transform.GetChild(1).gameObject.activeSelf == false)
                    {
                        GameObject UiScoreText = Instantiate(Resources.Load("ScoreImage") as GameObject);
                        UiScoreText.transform.GetChild(0).transform.GetComponent<Text>().text = "+$0.40";
                        UiScoreText.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                        UiScoreText.transform.localPosition = new Vector2(Random.Range(-371, 371), Random.Range(-200, 460));
                        UiScoreText.transform.localScale = new Vector3(1, 1, 1);
                        Destroy(UiScoreText, 1f);
                        TextProgress = true;
                        StartCoroutine(TextWaiter());
                    }
                    else
                    {
                        GameObject UiScoreText = Instantiate(Resources.Load("ScoreImage") as GameObject);
                        UiScoreText.transform.GetChild(0).transform.GetComponent<Text>().text = "+$0.80";
                        UiScoreText.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                        UiScoreText.transform.localPosition = new Vector2(Random.Range(-371, 371), Random.Range(-200, 460));
                        UiScoreText.transform.localScale = new Vector3(1, 1, 1);
                        Destroy(UiScoreText, 1f);
                        TextProgress = true;
                        StartCoroutine(TextWaiter());
                    }
                }
                else if (Mathf.Round(transform.eulerAngles.z) < 26.6 && Mathf.Round(transform.eulerAngles.z) > 13.85 || Mathf.Round(transform.eulerAngles.z) < 345.9 && Mathf.Round(transform.eulerAngles.z) > 333.3)
                {
                    if (FindObjectOfType<UiManager>().UiADBtn.transform.GetChild(1).gameObject.activeSelf == false)
                    {
                        GameObject UiScoreText = Instantiate(Resources.Load("ScoreImage") as GameObject);
                        UiScoreText.transform.GetChild(0).transform.GetComponent<Text>().text = "+$0.30";
                        UiScoreText.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                        UiScoreText.transform.localPosition = new Vector2(Random.Range(-371, 371), Random.Range(-200, 460));
                        UiScoreText.transform.localScale = new Vector3(1, 1, 1);
                        Destroy(UiScoreText, 1f);
                        TextProgress = true;
                        StartCoroutine(TextWaiter());
                    }
                    else
                    {
                        GameObject UiScoreText = Instantiate(Resources.Load("ScoreImage") as GameObject);
                        UiScoreText.transform.GetChild(0).transform.GetComponent<Text>().text = "+$0.60";
                        UiScoreText.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                        UiScoreText.transform.localPosition = new Vector2(Random.Range(-371, 371), Random.Range(-200, 460));
                        UiScoreText.transform.localScale = new Vector3(1, 1, 1);
                        Destroy(UiScoreText, 1f);
                        TextProgress = true;
                        StartCoroutine(TextWaiter());
                    }
                }
                else if (Mathf.Round(transform.eulerAngles.z) < 42 && Mathf.Round(transform.eulerAngles.z) > 26.5 || Mathf.Round(transform.eulerAngles.z) < 333.4 && Mathf.Round(transform.eulerAngles.z) > 318.5)
                {
                    if (FindObjectOfType<UiManager>().UiADBtn.transform.GetChild(1).gameObject.activeSelf == false)
                    {
                        GameObject UiScoreText = Instantiate(Resources.Load("ScoreImage") as GameObject);
                        UiScoreText.transform.GetChild(0).transform.GetComponent<Text>().text = "+$0.20";
                        UiScoreText.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                        UiScoreText.transform.localPosition = new Vector2(Random.Range(-371, 371), Random.Range(-200, 460));
                        UiScoreText.transform.localScale = new Vector3(1, 1, 1);
                        Destroy(UiScoreText, 1f);
                        TextProgress = true;
                        StartCoroutine(TextWaiter());
                    }
                    else
                    {
                        GameObject UiScoreText = Instantiate(Resources.Load("ScoreImage") as GameObject);
                        UiScoreText.transform.GetChild(0).transform.GetComponent<Text>().text = "+$0.40";
                        UiScoreText.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                        UiScoreText.transform.localPosition = new Vector2(Random.Range(-371, 371), Random.Range(-200, 460));
                        UiScoreText.transform.localScale = new Vector3(1, 1, 1);
                        Destroy(UiScoreText, 1f);
                        TextProgress = true;
                        StartCoroutine(TextWaiter());
                    }
                }
            }
            if (EmojiProgress == false)
            {
                if (Mathf.Round(transform.eulerAngles.z) <= 360 && Mathf.Round(transform.eulerAngles.z) > 353.6 || Mathf.Round(transform.eulerAngles.z) < 5.3 && Mathf.Round(transform.eulerAngles.z) >= 0)
                {
                    GameObject emoji4 = Instantiate(Resources.Load("Emoji-4") as GameObject);
                    emoji4.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                    emoji4.transform.localScale = new Vector3(1, 1f, 1);
                    Destroy(emoji4, 1.2f);
                    EmojiProgress = true;
                    StartCoroutine(EmojiWaiter());
                }
                else if (Mathf.Round(transform.eulerAngles.z) < 353.7 && Mathf.Round(transform.eulerAngles.z) > 345.8 || Mathf.Round(transform.eulerAngles.z) < 13.86 && Mathf.Round(transform.eulerAngles.z) > 5.2)
                {
                    GameObject emoji4 = Instantiate(Resources.Load("Emoji-3") as GameObject);
                    emoji4.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                    emoji4.transform.localScale = new Vector3(1, 1f, 1);
                    Destroy(emoji4, 1.2f);
                    EmojiProgress = true;
                    StartCoroutine(EmojiWaiter());
                }
                else if (Mathf.Round(transform.eulerAngles.z) < 26.6 && Mathf.Round(transform.eulerAngles.z) > 13.85 || Mathf.Round(transform.eulerAngles.z) < 345.9 && Mathf.Round(transform.eulerAngles.z) > 333.3)
                {
                    GameObject emoji4 = Instantiate(Resources.Load("Emoji-2") as GameObject);
                    emoji4.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                    emoji4.transform.localScale = new Vector3(1, 1f, 1);
                    Destroy(emoji4, 1.2f);
                    EmojiProgress = true;
                    StartCoroutine(EmojiWaiter());
                }
                else if (Mathf.Round(transform.eulerAngles.z) < 42 && Mathf.Round(transform.eulerAngles.z) > 26.5 || Mathf.Round(transform.eulerAngles.z) < 333.4 && Mathf.Round(transform.eulerAngles.z) > 318.5)
                {
                    GameObject emoji4 = Instantiate(Resources.Load("Emoji-1") as GameObject);
                    emoji4.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                    emoji4.transform.localScale = new Vector3(1, 1f, 1);
                    
                    Destroy(emoji4, 1.2f);
                    EmojiProgress = true;
                    StartCoroutine(EmojiWaiter());
                }
            }
                    yield return new WaitForSeconds(0.01f);
            StartCoroutine(OnProgress());
        }
        
    }

    IEnumerator TextWaiter()
    {
        yield return new WaitForSeconds(0.3f);
        TextProgress = false;
    }

    IEnumerator EmojiWaiter()
    {
        yield return new WaitForSeconds(2f);
        EmojiProgress = false;
    }

   

}
