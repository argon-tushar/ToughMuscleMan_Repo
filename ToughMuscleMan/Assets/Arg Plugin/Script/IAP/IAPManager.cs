using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

[Serializable]
public class IAPProduct
{
    public string productID;
    public ProductType productType;
    public List<IAPAttacher> Btns;
    public Action<int> purchaseCallback;
    public Action entitleMentCallback;
}

public class IAPManager : MonoBehaviour
{
    public static IAPManager manager;
    StoreController m_StoreController;
    public bool? isInitialize;
    [SerializeField] List<IAPProduct> productsList_Android, productsList_IOS;
    public bool hasBannerPurchase;
    public bool hasVideoPurchase;
    public List<GameObject> obj_hideable;

    [Header("Pop up")]
    public GameObject PanelRemoveAds;
    public GameObject obj_Duration;
    public GameObject obj_WeeklyBtns, obj_MonthlyBtns, obj_ForeverBtns;
    [SerializeField] GameObject btnWeeklyBanner, btnMonthlyBanner, btnForeverBanner;
    [SerializeField] GameObject btnWeeklyVideo, btnMonthlyVideo, btnForeverVideo;
    [SerializeField] GameObject btnWeeklyAll, btnMonthlyAll, btnForeverAll;

    private void Awake() => manager = this;

    public async Task initializeIAP()
    {
        try
        {
            ArgUtil.log("Trying to INIT IAP...", Keys.ColorYellow);
            List<IAPAttacher> allIAPButtons = FindObjectsByType<IAPAttacher>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

            foreach (var button in allIAPButtons)
            {
                button.initData();
                string _id = Application.platform == RuntimePlatform.Android ? button.productID_Android : button.productID_iOS;
                var p = getProduct_ByID(_id);
                p?.Btns.Add(button);
            }

            if (ARGManager.manager.data.IAPDuration == Keys.ForWeek)
            {
                obj_Duration.transform.GetChild(0).gameObject.SetActive(true);
                obj_WeeklyBtns.SetActive(true);
            }
            else if (ARGManager.manager.data.IAPDuration == Keys.ForMonth)
            {
                obj_Duration.transform.GetChild(1).gameObject.SetActive(true);
                obj_MonthlyBtns.SetActive(true);
            }
            else if (ARGManager.manager.data.IAPDuration == Keys.ForLifeTime)
            {
                obj_Duration.transform.GetChild(2).gameObject.SetActive(true);
                obj_ForeverBtns.SetActive(true);
            }

            var option = new InitializationOptions().SetEnvironmentName("production");
            await UnityServices.InitializeAsync(option);

            m_StoreController = UnityIAPServices.StoreController();
            m_StoreController.ProcessPendingOrdersOnPurchasesFetched(false);
            m_StoreController.OnStoreDisconnected += OnStoreDisconnected;
            m_StoreController.OnProductsFetched += OnProductsFetched;
            m_StoreController.OnProductsFetchFailed += OnProductsFetchFailed;
            m_StoreController.OnPurchasesFetched += OnPurchasesFetched;
            m_StoreController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;
            m_StoreController.OnPurchasePending += OnPurchasePending;
            m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            m_StoreController.OnPurchaseFailed += OnPurchaseFailed;
            m_StoreController.OnPurchaseDeferred += OnPurchaseDeferred;
            registerEntitlementCallback();

            await m_StoreController.Connect();

            var initialProductsToFetch = allProductLists();
            m_StoreController.FetchProducts(initialProductsToFetch);
        }
        catch (Exception ex)
        {
            isInitialize = false;
            ArgUtil.log($"IAP Exception while init\n{ex} ", Keys.ColorRed);
        }
    }

    private void registerEntitlementCallback()
    {
        m_StoreController.OnCheckEntitlement += (result) =>
        {
            Product product = result.Product;
            var status = result.Status;
            ArgUtil.log($"Product is {product.definition.id}, Entitle Status is {status} || " +
                $"Pending : {result.Order is PendingOrder} || " +
                $"Confirmed : {result.Order is ConfirmedOrder}" +
                $"Failed : {result.Order is FailedOrder} || " +
                $"Null : {result.Order is null} || "
                );
            if (status == EntitlementStatus.EntitledButNotFinished && result.Order is PendingOrder pendingOrder)
            {
                m_StoreController.ConfirmPurchase(pendingOrder);
                return;
            }
            if (status == EntitlementStatus.FullyEntitled)
            {
                IAPProduct iapproduct = getProduct_ByID(product.definition.id);
                iapproduct?.entitleMentCallback?.Invoke();
                ARGManager.manager.hideProcessWaiter();
                if (!ARGManager.manager.SplashPanel.activeSelf) ToastManager.manager.showToastMsg($"Successfully purchased!");
            }
        };
    }

    private List<ProductDefinition> allProductLists()
    {
        var initialProductsToFetch = new List<ProductDefinition>();

        if (Application.platform == RuntimePlatform.Android)
        {
            foreach (var products in productsList_Android)
            {
                initialProductsToFetch.Add(new ProductDefinition(products.productID, products.productType));
            }
        }
        else
        {
            foreach (var products in productsList_IOS)
            {
                initialProductsToFetch.Add(new ProductDefinition(products.productID, products.productType));
            }
        }

        return initialProductsToFetch;
    }

    private void OnStoreDisconnected(StoreConnectionFailureDescription description)
    {
        ArgUtil.log($"IAP Store Disconnected,reason is {description}", Keys.ColorYellow);
    }

    private void OnProductsFetched(List<Product> list)
    {
        foreach (var product in list)
        {
            string price = product.metadata.localizedPriceString;
            var myproduct = getProduct_ByID(product.definition.id);

            foreach (var obj_btn in myproduct.Btns)
            {
                obj_btn.txtPrice.text = price;
                obj_btn.btn.onClick.AddListener(() => BuyProduct(myproduct.productID));
            }
        }

        m_StoreController.FetchPurchases();
    }

    private void OnProductsFetchFailed(ProductFetchFailed failed)
    {
        if (failed.FailedFetchProducts != null && failed.FailedFetchProducts.Count > 0)
        {
            ArgUtil.log($"Failed to fetch {failed.FailedFetchProducts.Count} products:", Keys.ColorRed);
        }
    }

    private void OnPurchasesFetched(Orders orders)
    {
        isInitialize = true;

        foreach (var order in orders.ConfirmedOrders)
        {
            Product p = order.CartOrdered.Items().FirstOrDefault()?.Product;
            if (p.definition.type != ProductType.Consumable) m_StoreController.CheckEntitlement(p);
        }

        foreach (var order in orders.PendingOrders)
        {
            m_StoreController.ConfirmPurchase(order);
        }
    }

    private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription description)
    {
        ArgUtil.log($"Purchase fetch failed: {description}", Keys.ColorRed);
    }

    private void OnPurchasePending(PendingOrder order)
    {
        ArgUtil.log($"Pending order : {order}", Keys.ColorSkyblue);
        m_StoreController.ConfirmPurchase(order);
    }

    private void OnPurchaseConfirmed(Order order)
    {
        if (order is FailedOrder fOrder)
        {
            OnPurchaseFailed(fOrder);
            return;
        }

        if (order is ConfirmedOrder cOrder)
        {
            if (cOrder?.Info?.PurchasedProductInfo != null && cOrder.Info.PurchasedProductInfo.Count > 0)
            {
                int qty = getPurchasedQuantity(cOrder);
                string pID = cOrder.Info.PurchasedProductInfo[0].productId;
                ArgUtil.log($"Purchase Confirmed : {pID}", Keys.ColorSkyblue);

                Product p = m_StoreController.GetProductById(pID);
                if (p.definition.type == ProductType.Consumable)
                {
                    ARGManager.manager.hideProcessWaiter();
                    ArgUtil.log($"Successfully Purchased", Keys.ColorGreen);
                    ToastManager.manager.showToastMsg($"Successfully purchased!");
                    getPurchaseCallback(pID)?.Invoke(qty);
                }
                else
                {
                    ArgUtil.log($"Purchase Confirmed Checking Entitlement", Keys.ColorYellow);
                    m_StoreController.CheckEntitlement(p);
                }
            }
        }
    }

    int getPurchasedQuantity(Order order)
    {
        int qty = 1;

        string receipt = order.Info.Receipt;
        ArgUtil.log($"Receipt Data : {receipt}", Keys.ColorYellow);
        if (!string.IsNullOrEmpty(receipt))
        {
            try
            {
                IAPPayData payData = JsonUtility.FromJson<IAPPayData>(receipt);
                if (payData.Store != "fake")
                {
                    IAPPayload payload = JsonUtility.FromJson<IAPPayload>(payData.Payload);
                    IAPPayloadData payloadData = JsonUtility.FromJson<IAPPayloadData>(payload.json);
                    qty = payloadData.quantity;
                    ArgUtil.log($"Qty is : {qty}", Keys.ColorYellow);
                }
            }
            catch (Exception ex)
            {
                ArgUtil.log($"IAP QTY parse error : {ex.Message}", Keys.ColorRed);
            }
        }
        return qty;
    }

    private void OnPurchaseFailed(FailedOrder order)
    {
        ARGManager.manager.hideProcessWaiter();
        if (!ARGManager.manager.SplashPanel.activeSelf) ToastManager.manager.showToastMsg("Purchase failed!");

        if (order.Info.PurchasedProductInfo == null && order.Info.PurchasedProductInfo.Count == 0)
        {
            ArgUtil.log($"Purchase Failed but not order info available", Keys.ColorRed);
            return;
        }

        var item = order.CartOrdered.Items().FirstOrDefault();
        var product = item?.Product;
        string productId = product?.definition?.id;
        string storeSpecificId = product?.definition?.storeSpecificId;
        var reason = order.FailureReason;
        var message = order.Details;
        ArgUtil.log($"Purchase Failed is {productId} -- {storeSpecificId} --- Reason is {reason} --- Here is the message : {message}", Keys.ColorRed);
    }

    private void OnPurchaseDeferred(DeferredOrder order)
    {
        ArgUtil.log($"Purchase deferred : {order?.Info}", Keys.ColorSkyblue);
        //Show UI Like "Purchase Pending Approval..."
    }

    public void BuyProduct(string productID)
    {
        ARGManager.manager.OnButtonClickSound();
        if (isInitialize != true)
        {
            ArgUtil.log($"IAP is not init yet", Keys.ColorYellow);
            return;
        }

        ArgUtil.log($"Purchase btn clicked... {productID}", Keys.ColorSkyblue);
        ARGManager.manager.showProcessWaiter();
        m_StoreController.PurchaseProduct(productID);
    }

    //----------------------------------------------------------

    public Action<int> getPurchaseCallback(string _id) => getProduct_ByID(_id).purchaseCallback;

    public IAPProduct getProduct_ByID(string _id)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return productsList_Android.FirstOrDefault(p => p.productID.Equals(_id));
        }
        else
        {
            return productsList_IOS.FirstOrDefault(p => p.productID.Equals(_id));
        }
    }

    public void RestoreIOSPurchases()
    {
#if UNITY_IOS
        if (isInitialize == false) { ArgUtil.log("[IAP] Not initialized."); return; }
        ARGManager.manager.showProcessWaiter();

        m_StoreController.RestoreTransactions((success, message) =>
        {
            ArgUtil.log($"[IAP] RestoreTransactions finished. Success: {success}, Message: {message}");
            if (!success) ARGManager.manager.hideProcessWaiter();
            // Restored entitlements will arrive via OnPurchasesFetched/OnPurchaseConfirmed/OnPurchasePending
        });
#endif
    }

    #region ----------------- Removed Ads Popup -----------------

    public void ShowPopup_RemoveAds()
    {
        if (!ARGManager.manager.data.hasIAP) return;
        Loader.manager.doTransit(() =>
        {

            PanelRemoveAds.SetActive(true);
        });
    }

    public void HidePopup_RemoveAds()
    {
        ARGManager.manager.OnButtonClickSound();
        Loader.manager.doTransit(() =>
        {
            PanelRemoveAds.SetActive(false);
        });
    }

    public void OnclickBannerBGClose()
    {
        ARGManager.manager.OnButtonClickSound();
        if (isInitialize != true) return;
        if (!ARGManager.manager.data.hasIAP) return;

        IAPAttacher tempAttacher = null;
        if (ARGManager.manager.data.IAPDuration == Keys.ForWeek) tempAttacher = btnWeeklyBanner.GetComponent<IAPAttacher>();
        else if (ARGManager.manager.data.IAPDuration == Keys.ForMonth) tempAttacher = btnMonthlyBanner.GetComponent<IAPAttacher>();
        else if (ARGManager.manager.data.IAPDuration == Keys.ForLifeTime) tempAttacher = btnForeverBanner.GetComponent<IAPAttacher>();

        BuyProduct(Application.platform == RuntimePlatform.Android ? tempAttacher.productID_Android : tempAttacher.productID_iOS);
    }

    // -------------- CALLBACKS --------------
    public void IAPOnSuccessRemove_Banner()
    {
        ArgUtil.log("IAP Success Banner ads Removed", Keys.ColorGreen);
        ARGManager.manager.data.provider_Banner = Keys._NONE;
        ARGManager.manager.data.canShowNativeAd = false;
        hasBannerPurchase = true;

        btnWeeklyBanner.SetActive(false);
        btnMonthlyBanner.SetActive(false);
        btnForeverBanner.SetActive(false);
        btnWeeklyAll.SetActive(false);
        btnMonthlyAll.SetActive(false);
        btnForeverAll.SetActive(false);

        AdmobManager.manager.banner_DestroyAd();
        AdmobManager.manager.forceDestroy_native();
        UnityAdsManager.manager.banner_HideAd();

        checkForHidesOnPurchase();
    }

    public void IAPOnSuccessRemove_VideoAds()
    {
        ArgUtil.log("IAP Success Video ads Removed", Keys.ColorGreen);
        ARGManager.manager.data.provider_InterAd = Keys._NONE;
        ARGManager.manager.data.provider_AppOpen = Keys._NONE;
        hasVideoPurchase = true;

        btnWeeklyVideo.SetActive(false);
        btnMonthlyVideo.SetActive(false);
        btnForeverVideo.SetActive(false);
        btnWeeklyAll.SetActive(false);
        btnMonthlyAll.SetActive(false);
        btnForeverAll.SetActive(false);

        checkForHidesOnPurchase();
    }

    public void IAPOnSuccessRemove_Both()
    {
        ArgUtil.log("IAP Success both ads Removed", Keys.ColorGreen);
        IAPOnSuccessRemove_Banner();
        IAPOnSuccessRemove_VideoAds();
    }

    void checkForHidesOnPurchase()
    {
        CancelInvoke(nameof(HideRelatedThingsOnPurchase));
        Invoke(nameof(HideRelatedThingsOnPurchase), 0f);
    }

    void HideRelatedThingsOnPurchase()
    {
        if (hasBannerPurchase && hasVideoPurchase)
        {
            foreach (var obj in obj_hideable)
            {
                obj?.SetActive(false);
            }
        }
    }

    #endregion

    public void RegisterRemoveAdsButton(Button RemoveAdsButton)
    {
        RemoveAdsButton.onClick.RemoveAllListeners();
        RemoveAdsButton.onClick.AddListener(() =>
        {
            ARGManager.manager.OnButtonClickSound();
            ShowPopup_RemoveAds();
        });

        obj_hideable.Add(RemoveAdsButton.gameObject);
        RemoveAdsButton.gameObject.SetActive(!(hasBannerPurchase && hasVideoPurchase));
    }

    public void RegisterRestoreButton(Button RestorePurchaseButton)
    {
        RestorePurchaseButton.gameObject.SetActive(false);

#if UNITY_IOS
        RestorePurchaseButton.gameObject.SetActive(true);
        RestorePurchaseButton.onClick.RemoveAllListeners();
        RestorePurchaseButton.gameObject.SetActive(true);
        RestorePurchaseButton.onClick.AddListener(() =>
        {
            ARGManager.manager.OnButtonClickSound();
            RestoreIOSPurchases();
        });
#endif

    }

}

//------------------------------ ANOTHER IAP Json Classes--------------------
[Serializable]
public class IAPPayData
{
    public string Payload;
    public string Store;
    public string TransactionID;
}

[Serializable]
public class IAPPayload
{
    public string json;
    public string signature;
    public IAPPayloadData payloadData;
}

[Serializable]
public class IAPPayloadData
{
    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
    public int quantity;
    public bool acknowledged;
}


/*
   
//Removed ADD button Init

        IAPManager.manager.RegisterRemoveAdsButton(RemoveAdsButton);
        IAPManager.manager.RegisterRestoreButton(RestorePurchaseButton);

//Removed ADD button Init

 */