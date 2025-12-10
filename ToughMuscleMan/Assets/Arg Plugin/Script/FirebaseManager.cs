using Firebase.RemoteConfig;
using Firebase;
using System;
using UnityEngine;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Collections;
using Firebase.Crashlytics;

public class FirebaseManager : MonoBehaviour
{
    FirebaseApp firebase_app;
    [SerializeField] bool isFirebaseInit;

    void Start()
    {
        StartCoroutine(initFirebase());
    }

    IEnumerator initFirebase()
    {
        yield return new WaitForSeconds(1);
        while (!ConnectionManager.manager.hasConnection)
        {
            yield return new WaitForSeconds(1);
            ArgUtil.log("Waiting for internet connection...");
        }

        ArgUtil.log("Trying to INIT firebase...", Keys.ColorYellow);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                isFirebaseInit = true;
                firebase_app = FirebaseApp.DefaultInstance;
                Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
                Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                ArgUtil.log("Firebase init success", Keys.ColorGreen);
                fetchData();
            }
            else
            {
                ArgUtil.log("Firebase init fails, check log to get more details.", Keys.ColorRed);
            }
        });
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        ArgUtil.log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        ArgUtil.log("Received a new message from: " + e.Message.From);
    }

    async void fetchData()
    {
        Loader.manager.setLoader(5f);
        ArgUtil.log("Fetching json data...", Keys.ColorYellow);
        await fetchFromFirebase();
    }

    private async Task fetchFromFirebase()
    {
        try
        {
            var defaults = new System.Collections.Generic.Dictionary<string, object>
            {
                { Keys.param_firebase_data, "{}"}
            };

            await FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();

            string json = FirebaseRemoteConfig.DefaultInstance.GetValue(Keys.param_firebase_data).StringValue;
            try
            {
                Loader.manager.setLoader(15f);
                ARGManager.manager.data = JsonUtility.FromJson<DataModel>(json);
                ArgUtil.log("Json Loaded Successfully", Keys.ColorGreen);

                if (ARGManager.manager.data.hasIAP)
                {
                    await IAPManager.manager.initializeIAP();
                    Loader.manager.setLoader(10f);
                    await Awaitable.WaitForSecondsAsync(2.5f);
                }
                UpdateManager.manager.checkUpdate();

            }
            catch (Exception ex)
            {
                ArgUtil.log("Json load failed\n\n" + ex, Keys.ColorRed);
            }

        }
        catch (Exception ex)
        {
            ArgUtil.log("Remote Config fetch / parse failed " + ex.Message, Keys.ColorRed);
        }
    }
}
