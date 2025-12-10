using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ToastManager : MonoBehaviour
{
    public static ToastManager manager;
    [SerializeField] List<Toast> toastList;

    private void Awake() => manager = this;

    public void showToastMsg(string msg)
    {
        StartCoroutine(asyncToastMsg(msg));
    }

    IEnumerator asyncToastMsg(string msg)
    {
        yield return null;

        Toast t = toastList[0];
        toastList.RemoveAt(0);
        toastList.Add(t);

        t.gameObject.SetActive(true);
        t.showTost(msg);
    }
}