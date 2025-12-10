using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager manager;
    [SerializeField] CanvasGroup PanelConnection;
    [SerializeField] RectTransform subpanel;
    [SerializeField] GameObject obj_Noconnection, obj_Connecting;
    public bool hasConnection;

    private void Awake()
    {
        manager = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 300;
        callConnection();
    }

    public void Onclick_Retry()
    {
        ARGManager.manager.OnButtonClickSound();
        obj_Connecting.SetActive(true);
        obj_Noconnection.SetActive(false);
        Invoke(nameof(callConnection), Random.Range(2f, 5f));
    }

    void callConnection() => StartCoroutine(CheckInternetConnection());

    IEnumerator CheckInternetConnection()
    {
        ArgUtil.log("Checking internet connection...", Keys.ColorYellow);
        yield return new WaitForSeconds(1);
        using (var request = UnityWebRequest.Head("https://www.google.com/generate_204"))
        {
            request.timeout = 10;
            yield return request.SendWebRequest();

            yield return new WaitForEndOfFrame();
            if (request.result == UnityWebRequest.Result.Success && (request.responseCode == 204 || request.responseCode == 200))
            {
                ArgUtil.log("Internet connection success...", Keys.ColorGreen);

                PanelConnection.interactable = false;
                PanelConnection.DOFade(0f, 0.1f).SetDelay(0.4f).SetUpdate(true).OnComplete(() =>
                {
                    PanelConnection.gameObject.SetActive(false);
                    hasConnection = true;
                });
                subpanel.DOAnchorPos(new Vector2(500f, 0f), 0.5f).SetEase(Ease.InBack, 2).SetUpdate(true);
                subpanel.DORotate(new Vector3(0, 0, 10f), 0.4f).SetUpdate(true).OnComplete(() =>
                {
                    subpanel.DORotate(Vector3.zero, 0.1f).SetUpdate(true);
                });
                Loader.manager.setLoader(10f);
            }
            else
            {
                ArgUtil.log("Internet connection failed...", Keys.ColorRed);
                hasConnection = false;

                obj_Connecting.SetActive(false);
                obj_Noconnection.SetActive(true);
                PanelConnection.gameObject.SetActive(true);
                PanelConnection.DOFade(1f, 0.4f).SetUpdate(true);
                subpanel.DOAnchorPos(Vector2.zero, 0.4f).SetEase(Ease.OutBack, 2).SetUpdate(true);
                subpanel.DORotate(new Vector3(0, 0, 10), 0.3f).SetUpdate(true).OnComplete(() =>
                {
                    subpanel.DORotate(new Vector3(0, 0, 0), 0.2f).SetUpdate(true);
                    PanelConnection.interactable = true;
                });
            }
        }
    }

    public static bool notConnected() => Application.internetReachability == NetworkReachability.NotReachable ? true : false;
}
