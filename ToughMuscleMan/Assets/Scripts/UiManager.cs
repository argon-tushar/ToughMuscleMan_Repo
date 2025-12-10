using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UiManager : MonoBehaviour
{
    public GameObject Character, LevelProcess;
    public GameObject LevelFailed, LevelComplate, UiCanvas, PlayerWin, ContinueBtn, AdWinBtn, playerLose, UiADBtn;
    public Text leveltextUi;
    public bool WinBool = false;
    public Text CompleteText, FailedText;
    public Text CoinScoreText, MeterText;
    public GameObject Hand;
    public List<Sprite> Backgrounds = new List<Sprite>();
    public GameObject LeftParticle, RightParticle;


    void Start()
    {
        AdManager.manager.setInterCountDown();

        BackGroundUi.GetComponent<Image>().sprite = Backgrounds[(int.Parse(PlayerPrefs.GetString("BackgroundNo").Substring(6)))];

        lvc = 0;
        PlayerPrefs.SetFloat("FillLevelUi", 0);

        ChangeCharacter();

        if (PlayerPrefs.GetString("handfinger") == "")
        {
            Hand.SetActive(true);
        }
        else
        {
            Hand.SetActive(false);
        }
    }

    bool particlewinbool = false;
    int Tm = 0;
    public Text uiText;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (UiCanvas.activeSelf == true)
            {
                HomeBtn();
            }
        }


        if (ADDoubeScoreTimer.timer > 0)
        {


            // PlayerPrefs.SetFloat("Timer", timer);
            FindObjectOfType<UiManager>().uiText.text = Mathf.Round(ADDoubeScoreTimer.timer).ToString() + " Sec";
            UiADBtn.transform.GetChild(0).gameObject.SetActive(false);
            UiADBtn.transform.GetChild(1).gameObject.SetActive(true);

        }
        else if (ADDoubeScoreTimer.timer <= 0)
        {
            FindObjectOfType<UiManager>().uiText.text = "0" + " Sec";
            Tm++;
            if (Tm == 1)
            {
                UiADBtn.transform.GetChild(0).gameObject.SetActive(true);
                UiADBtn.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        LevelProcess.GetComponent<Image>().fillAmount = PlayerPrefs.GetFloat("FillLevelUi");
        CoinScoreText.text = Mathf.Round(PlayerPrefs.GetFloat("Coin")).ToString();

        if (LevelProcess.GetComponent<Image>().fillAmount == 1)
        {
            Character.transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = null;
            FindObjectOfType<Rotation>().StopArrov = true;
            FindObjectOfType<Rotation>().IsProgress = false;

            if (particlewinbool == false)
            {

                LeftParticle.SetActive(true);
                RightParticle.SetActive(true);

                particlewinbool = true;
                FindObjectOfType<SoundManager>().OnLevelWin();
                Destroy(FindObjectOfType<Rotation>().Particles1);


            }

            PlayerPrefs.SetString("abc", "xy");
            StartCoroutine(WinWaiter());
        }

        if (PlayerPrefs.GetString("abc") == "")
        {

            leveltextUi.text = "LEVEL " + PlayerPrefs.GetInt("Level").ToString();
        }
        else
        {
            leveltextUi.text = "LEVEL " + PlayerPrefs.GetInt("Level").ToString();
        }
    }

    private GameObject Character1;

    public void ChangeCharacter()
    {
        //if (PlayerPrefs.GetInt("Level") < 31)
        //{

        Character1 = Instantiate(Resources.Load("Character" + PlayerPrefs.GetInt("CharacterLevel").ToString()) as GameObject);
        Character1.transform.SetParent(GameObject.Find("CharacterCanvas/SafeArea").transform);
        Character1.transform.localScale = new Vector3(1f, 1f, 0);
        Character1.transform.localPosition = new Vector3(0, 0, 0);
        //}
        //else
        //{
        //    Character1 = Instantiate(Resources.Load("Character30") as GameObject);
        //    Character1.transform.SetParent(GameObject.Find("CharacterCanvas/SafeArea").transform);
        //    Character1.transform.localScale = new Vector3(1f, 1f, 0);
        //    Character1.transform.localPosition = new Vector3(0, 0, 0);
        //}
    }

    public GameObject BackGroundUi;

    IEnumerator CounterWinner()
    {
        float FirstCount = PlayerPrefs.GetFloat("WinFirstCount");
        float anow = PlayerPrefs.GetFloat("WinFirstNow");
        double now = (double)anow;
        //Debug.Log("sdsdsdsdsdsdsd");
        while (true)
        {
            yield return new WaitForSeconds(0.009f);


            if (now < FirstCount)
            {

                now = now + 0.1;

                if (FirstCount <= 100)
                {
                    //Debug.Log(now);

                    int abcd = (int)now;
                    CompleteText.text = abcd.ToString() + " %";

                    PlayerWin.GetComponent<Image>().fillAmount = (float)now / 100;


                }
                else
                {
                    Debug.Log("else");
                    now = 0;
                    FirstCount = 100 / 10;

                }

            }
            else
            {
                FirstCount = FirstCount + 10f;

                PlayerPrefs.SetFloat("WinFirstCount", FirstCount);
                PlayerPrefs.SetFloat("WinFirstNow", (float)now);

                ContinueBtn.GetComponent<Button>().interactable = true;
                AdWinBtn.GetComponent<Button>().interactable = true;
                MeterText.text = "0";
                break;

            }
        }

    }
    int lvc = 0;
    IEnumerator WinWaiter()
    {
        yield return new WaitForSeconds(2.3f);
        if (WinBool == false)
        {
            AdManager.manager.requestInter();

            WinBool = true;

            PlayerPrefs.SetInt(PlayerPrefs.GetInt("Level").ToString(), 2);
            PlayerPrefs.SetInt((PlayerPrefs.GetInt("Level") + 1).ToString(), 1);

            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);

            if (PlayerPrefs.GetInt("CharacterLevel") == 30)
            {
                PlayerPrefs.SetInt("CharacterLevel", 0);
            }

            PlayerPrefs.SetInt("CharacterLevel", PlayerPrefs.GetInt("CharacterLevel") + 1);


            LevelComplate.SetActive(true);
            //Level Completed
            lvc++;

            /* if(lvc==1)
             {
                 FindObjectOfType<BannerAdScript>().Fullads();
             }*/
            UiCanvas.SetActive(false);

            float now = PlayerPrefs.GetFloat("WinFirstNow");

            int abcde = (int)now;

            CompleteText.text = abcde.ToString() + " %";
            PlayerWin.GetComponent<Image>().fillAmount = now / 100;

            PlayerPrefs.SetFloat("FillLevelUi", 0);

            claim = false;


            StartCoroutine(WinnerWaiter());
        }

    }

    IEnumerator WinnerWaiter()
    {
        yield return new WaitForSeconds(1f);
        //PlayerWin.transform.GetComponent<Animator>().enabled = true;
        StartCoroutine(CounterWinner());

    }


    public void OnContinueBtn()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();
        AdManager.manager.showInter(after_OnContinueBtn);
    }

    void after_OnContinueBtn()
    {
        lvc = 0;
        LevelComplate.SetActive(false);
        UiCanvas.SetActive(true);
        LeftParticle.SetActive(false);
        RightParticle.SetActive(false);

        Destroy(Character1);
        ChangeCharacter();
        particlewinbool = false;
        WinBool = false;
        ContinueBtn.GetComponent<Button>().interactable = false;
        AdWinBtn.GetComponent<Button>().interactable = false;
        StartCoroutine(ArrowWaiter());
    }

    IEnumerator ArrowWaiter()
    {
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<Rotation>().StopArrov = false;
    }

    public static int rInt = 0;
    public void RetryBtn()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();
        AdManager.manager.showInter(afterRetry, true);
    }

    void afterRetry()
    {
        SceneManager.LoadScene("UiScene");
        PlayerPrefs.SetFloat("FillLevelUi", 0);
    }

    public void OnHomeFailed()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();
        AdManager.manager.showInter(afterHomeFailed, true);
    }

    void afterHomeFailed()
    {
        SceneManager.LoadScene("HomeScene");
        PlayerPrefs.SetFloat("FillLevelUi", 0);
    }

    public void HomeBtn()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();
        AdManager.manager.showInter(afterHome, true);
    }

    void afterHome()
    {
        SceneManager.LoadScene("HomeScene");
    }


    bool claim = false;

    public void On5XBtn()
    {
        FindObjectOfType<SoundManager>().Onbuttonclick();
        AdManager.manager.showRewardedAD(Aftr5Video);
    }

    public void Aftr5Video()
    {
        LeftParticle.SetActive(false);
        RightParticle.SetActive(false);
        StartCoroutine(giftwaiter());
    }

    public void OnContinueBtnX()
    {
        lvc = 0;
        LevelComplate.SetActive(false);
        UiCanvas.SetActive(true);

        //SceneManager.LoadScene(1);
        Destroy(Character1);
        ChangeCharacter();
        particlewinbool = false;
        WinBool = false;
        ContinueBtn.GetComponent<Button>().interactable = false;
        AdWinBtn.GetComponent<Button>().interactable = false;

        StartCoroutine(ArrowWaiter());
    }

    IEnumerator giftwaiter()
    {
        OnContinueBtnX();
        yield return new WaitForSeconds(0.5f);

        if (claim == false)
        {

            for (int i = 0; i < 50; i++)
            {
                GameObject Coin = Instantiate(Resources.Load("coin") as GameObject);
                Coin.transform.SetParent(GameObject.Find("CharacterCanvas").transform);
                Coin.transform.localScale = new Vector3(1f, 1f, 0);
                Coin.transform.position = new Vector3(0f, 0f, 0);
            }
            PlayerPrefs.SetFloat("Coin", PlayerPrefs.GetFloat("Coin") + 100);
            claim = true;
            //FindObjectOfType<SoundManager>().OnCollectCoinClip();
        }
        FindObjectOfType<SoundManager>().OnCoin5x();
    }

    public void OnTimeAdBtn()
    {
        if (UiADBtn.transform.GetChild(1).gameObject.activeSelf == false)
        {
            FindObjectOfType<SoundManager>().Onbuttonclick();
            AdManager.manager.showRewardedAD(XMinVideoAfter);
        }
    }

    public void XMinVideoAfter()
    {
        if (UiADBtn.transform.GetChild(1).gameObject.activeSelf == false)
        {
            ADDoubeScoreTimer.timer = 60;
            Tm = 0;
        }
    }
}
