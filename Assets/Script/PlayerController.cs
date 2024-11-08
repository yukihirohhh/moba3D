using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Ball; // ボールをアタッチするオブジェクト
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public float ballLiftForce = 10f; // ボールを上にあげる力

    private bool isBallInRange = false; // ボールが範囲内にあるか

    void Start()
    {
        // 必要に応じて初期設定
    }

    void Update()
    {
        HandleMovement(); // プレイヤーの移動処理
        HandleBallLift(); // ボールを上にあげる処理
    }

    // プレイヤーの移動処理
    void HandleMovement()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // W, A, S, Dキーで移動
        if (Input.GetKey(KeyCode.W)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    // ボールを上にあげる処理
    void HandleBallLift()
    {
        // ボールが範囲内かつQキーが押されたとき
        if (isBallInRange && Input.GetKeyDown(KeyCode.Q))
        {
            Rigidbody ballRb = Ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.velocity = Vector3.zero; // ボールの速度をリセット
                ballRb.AddForce(Vector3.up * ballLiftForce, ForceMode.Impulse); // ボールを上にあげる
            }
        }
    }

    // ボールがトリガー内に入ったときに範囲内フラグを立てる
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Ball)
        {
            isBallInRange = true;
        }
    }

    // ボールがトリガーから出たときに範囲内フラグをリセット
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Ball)
        {
            isBallInRange = false;
        }
    }
}
