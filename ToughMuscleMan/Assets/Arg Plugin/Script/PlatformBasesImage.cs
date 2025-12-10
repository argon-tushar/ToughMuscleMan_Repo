using UnityEngine;
using UnityEngine.UI;

public class PlatformBasesImage : MonoBehaviour
{
    [SerializeField] Sprite spAndroid;
    [SerializeField] Sprite spIos;

    private void Awake()
    {
#if UNITY_ANDROID
        this.GetComponent<Image>().sprite = spAndroid;
#else
        this.GetComponent<Image>().sprite = spIos;
#endif

    }
}
