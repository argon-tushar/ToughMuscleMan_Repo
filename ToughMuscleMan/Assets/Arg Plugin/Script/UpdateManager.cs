using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager manager;
    [SerializeField] CanvasGroup UpdatePanel;
    [SerializeField] RectTransform SubUpdatePanel;
    [SerializeField] TextMeshProUGUI txtversion;
    [SerializeField] GameObject BtnLater;

    private void Awake() => manager = this;

    public void checkUpdate()
    {
        Loader.manager.setLoader(Random.Range(4, 10));
        StartCoroutine(waitIfCancled());
    }

    bool updateCanceled;
    IEnumerator waitIfCancled()
    {
        if (ARGManager.manager.data.canshowUpdate)
        {
            ArgUtil.log("Checking if update available", Keys.ColorYellow);
            yield return new WaitForSeconds(1f);

            if (Application.version == ARGManager.manager.data.currentversion)
            {
                ArgUtil.log("Application is upto-date", Keys.ColorGreen);
                continueToGame();
            }
            else
            {
                ArgUtil.log("New update available - " + ARGManager.manager.data.currentversion, Keys.ColorYellow);
                BtnLater.SetActive(ARGManager.manager.data.mustUpdate);
                txtversion.SetText("New Version " + ARGManager.manager.data.currentversion);
                updateCanceled = false;
                UpdatePanel.gameObject.SetActive(true);
                UpdatePanel.DOFade(1f, 0.4f).SetUpdate(true);
                SubUpdatePanel.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.OutBack, 2).SetUpdate(true);
                SubUpdatePanel.DORotate(new Vector3(0, 0, 10), 0.3f).SetUpdate(true).OnComplete(() =>
                {
                    SubUpdatePanel.DORotate(new Vector3(0, 0, 0), 0.2f).SetUpdate(true);
                    UpdatePanel.interactable = true;
                });

                while (!updateCanceled)
                {
                    yield return new WaitForSeconds(1f);
                }
                continueToGame();
            }
        }
        else continueToGame();
    }

    public void OnclickUpdate()
    {
        ARGManager.manager.OnButtonClickSound();
        Application.OpenURL(ARGManager.manager.data.myStoreLink);
    }

    public void OnclickLater()
    {
        ARGManager.manager.OnButtonClickSound();
        UpdatePanel.interactable = false;
        UpdatePanel.DOFade(0f, 0.1f).SetDelay(0.4f).SetUpdate(true).OnComplete(() =>
        {
            UpdatePanel.gameObject.SetActive(false);
            updateCanceled = true;
        });

        SubUpdatePanel.DOAnchorPos(new Vector2(500f, 0f), 0.5f).SetEase(Ease.InBack, 2).SetUpdate(true);
        SubUpdatePanel.DORotate(new Vector3(0, 0, 10f), 0.4f).SetUpdate(true).OnComplete(() =>
        {
            SubUpdatePanel.DORotate(Vector3.zero, 0.1f).SetUpdate(true);
        });
    }

    public void continueToGame()
    {
        Loader.manager.setLoader(15f);
        ArgUtil.log("Entering in game", Keys.ColorWhite);
        ConsentManager.manager.checkForConsentAndContinue();
    }
}
