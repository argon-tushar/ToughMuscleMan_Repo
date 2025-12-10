using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMover : MonoBehaviour
{
    GameObject target;
    float moveSpeed;
    public int increaseValue;

    private void Awake()
    {
        moveSpeed = Random.Range(20, 15);
        target = GameObject.Find("UiCanvas/SafeArea/CoinBg/Coin");
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.5f, 0.5f),
                                                                            Random.Range(-0.6f, 0.6f)) * 180);
        
        Invoke("rbDisableWait", Random.Range(0.25f, 0.65f));
    }

    void rbDisableWait()
    {
        Destroy(GetComponent<Rigidbody2D>());
    }

    private void Update()
    {
        if (GetComponent<Rigidbody2D>() == null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.transform.position) == 90f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        //PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + (1 * increaseValue));
    }
}
