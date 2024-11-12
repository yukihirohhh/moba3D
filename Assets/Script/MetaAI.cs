using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaController : MonoBehaviour
{
    public GameObject Ball; // ボールオブジェクト
    private bool isReceive = false; // レシーブ状態
    private bool isToss = false;    // トス状態
    private bool isSpike = false;   // スパイク状態
    private int touchCount = 0;     // ボールに触れた回数

    void Start()
    {
        ResetStates(); // 初期化時に状態をリセット
    }

    void Update()
    {
        // 現在の状態をデバッグ表示（利用されるように）
        if (isReceive)
        {
            Debug.Log("現在はレシーブ状態です。");
        }
        else if (isToss)
        {
            Debug.Log("現在はトス状態です。");
        }
        else if (isSpike)
        {
            Debug.Log("現在はスパイク状態です。");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Team_A_Tag") || other.CompareTag("Team_B_Tag"))
        {
            ResetStates();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UpdateTouchState();
        }
    }

    private void UpdateTouchState()
    {
        touchCount++;

        if (touchCount == 1)
        {
            isReceive = true;
            isToss = false;
            isSpike = false;
            Debug.Log("レシーブ状態");
        }
        else if (touchCount == 2)
        {
            isReceive = false;
            isToss = true;
            isSpike = false;
            Debug.Log("トス状態");
        }
        else if (touchCount == 3)
        {
            isReceive = false;
            isToss = false;
            isSpike = true;
            Debug.Log("スパイク状態");
        }
    }

    private void ResetStates()
    {
        isReceive = false;
        isToss = false;
        isSpike = false;
        touchCount = 0;
        Debug.Log("状態リセット");
    }
}
