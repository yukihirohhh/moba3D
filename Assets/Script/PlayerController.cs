using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Ball; // ボールオブジェクト
    public Transform TargetArea; // 相手チームのエリア
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public float hitForce = 20f; // ボールを飛ばす力
    public float verticalLift = 10f; // 放物線を描くための上方向の力
    public float randomRange = 0.5f; // ランダムな方向の範囲

    private bool isBallInRange = false; // ボールが範囲内にあるか

    void Start()
    {
        // 必要に応じて初期設定
    }

    void Update()
    {
        HandleMovement(); // プレイヤーの移動処理
        HandleBallHit(); // ボールを打ち返す処理
    }

    // プレイヤーの移動処理
    void HandleMovement()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    // ボールを打ち返す処理
    void HandleBallHit()
    {
        if (isBallInRange && Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody ballRb = Ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.velocity = Vector3.zero;

                // ターゲットエリア方向のベースベクトルを取得
                Vector3 directionToTarget = (TargetArea.position - Ball.transform.position).normalized;

                // ランダムな範囲で少し方向を変える
                directionToTarget.x += Random.Range(-randomRange, randomRange);
                directionToTarget.z += Random.Range(-randomRange, randomRange);

                // 正規化して水平力を加える
                directionToTarget = directionToTarget.normalized;
                Vector3 force = directionToTarget * hitForce;

                // 上方向の力を加えて放物線を描く
                force.y = verticalLift;

                // ボールに最終的な力を加える
                ballRb.AddForce(force, ForceMode.Impulse);
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
