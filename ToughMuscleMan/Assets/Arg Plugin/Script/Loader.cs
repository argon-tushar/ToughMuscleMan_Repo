using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public static Loader manager;
    [SerializeField] Image loaderFill;
    [SerializeField] TextMeshProUGUI txtpercentage;
    [SerializeField] float currentprogress;
    [SerializeField] Image transit;

    void Awake() => manager = this;
    private void Start()
    {
        txtpercentage.SetText(("Loading...0%"));

        doTransit(null);
        currentprogress = 0f;
    }

    public void setLoader(float progress)
    {
        DOTween.Kill(loaderFill);
        currentprogress += (progress / 100f);
        ArgUtil.log("Progress " + currentprogress, Keys.ColorRose);
        loaderFill.DOFillAmount(currentprogress, 1f).OnComplete(() =>
        {

        }).OnUpdate(() => txtpercentage.SetText("Loading..." + (loaderFill.fillAmount * 100f).ToString("###") + "%"));
    }

    public void doTransit(Action callback)
    {
        transit.gameObject.SetActive(true);
        transit.DOFade(1f, 0.3f).OnComplete(() =>
        {
            callback?.Invoke();
            transit.DOFade(0f, 0.5f).SetDelay(0.3f).OnComplete(() =>
            {
                transit.gameObject.SetActive(false);
            });
        });
    }
}
