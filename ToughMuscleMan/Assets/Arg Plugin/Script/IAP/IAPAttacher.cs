using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IAPAttacher : MonoBehaviour
{
    public string productID_Android;
    public string productID_iOS;
    public Button btn;
    public Text txtPrice;

    [Space(5)]
    [SerializeField] UnityEvent<int?> callback;
    [SerializeField] UnityEvent callback_entitlement;

    public void initData()
    {
        string _id = Application.platform == RuntimePlatform.Android ? productID_Android : productID_iOS;
        IAPProduct product = IAPManager.manager.getProduct_ByID(_id);
        if (product != null)
        {
            product.purchaseCallback += onConfirmCallback;
            product.entitleMentCallback += onEntitlement;
        }
    }

    public void onConfirmCallback(int qty) => callback?.Invoke(qty);

    public void onEntitlement() => callback_entitlement?.Invoke();
}