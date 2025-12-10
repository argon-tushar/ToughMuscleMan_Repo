using UnityEngine;

public class IgnoreBannerShow : MonoBehaviour
{
    private void OnEnable()
    {
        AdManager.manager?.hideBanner();
    }

    private void OnDisable()
    {
        AdManager.manager?.showBanner();
    }
}
