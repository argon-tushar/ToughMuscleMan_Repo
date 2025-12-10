using Coffee.UIExtensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    [SerializeField] RectTransform obj;
    [SerializeField] TextMeshProUGUI txtmsg;
    [SerializeField] UIShadow objshadow;


    private void Awake()
    {
        txtmsg.color = ArgUtil.getColor(ARGManager.manager.data.toastmsg_textcolor);
        obj.GetComponent<Image>().color = ArgUtil.getColor(ARGManager.manager.data.toastmsg_bgcolor, ARGManager.manager.data.toastmsg_bgalpha);
        objshadow.effectColor = ArgUtil.getColor(ARGManager.manager.data.toastmsg_bordercolor, ARGManager.manager.data.toastmsg_borderalpha);
        objshadow.effectDistance=  new Vector2(ARGManager.manager.data.toastmsg_bordersize_x, ARGManager.manager.data.toastmsg_bordersize_y);
    }

    public void showTost(string msg)
    {

        obj.anchoredPosition = Vector2.zero;
        obj.localScale = Vector3.zero;

        txtmsg.SetText(msg);
        obj.DOKill();
        obj.DOAnchorPosY(ARGManager.manager.data.toastmsgPos, 0.4f).SetEase(Ease.OutBack);
        obj.DOScale(1f, 0.2f).SetEase(Ease.OutBack);

        CancelInvoke(nameof(hideMsg));
        Invoke(nameof(hideMsg), ARGManager.manager.data.toastmsg_hidetime);
    }

    void hideMsg()
    {
        obj.DOScale(0f, 0.1f).OnComplete(() =>
        {
            obj.anchoredPosition = Vector2.zero;
            obj.gameObject.SetActive(false);
        });
    }
}
