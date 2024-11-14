using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Ball; // ボールオブジェクト
    public Transform TargetArea; // 相手チームのエリア
    public Vector3[] ReceivePositions; // 自分のチームのレシーブエリアの座標
    public Vector3[] TossPositions; // トスエリアの座標
    public float moveSpeed = 5f; // プレイヤーの移動速度
    public float hitForce = 20f; // ボールを飛ばす力
    public float verticalLift = 10f; // 放物線を描くための上方向の力
    public float randomRange = 0.5f; // ランダムな方向の範囲
    public float jumpForce = 5f; // ジャンプ力

    private bool isBallInRange = false; // ボールが範囲内にあるか
    private bool isGrounded = true; // プレイヤーが地面にいるか
    private bool canJump = false; // ジャンプができるか
    private bool canReceive = true; // レシーブができるか
    private bool canToss = false; // トスができるか
    private bool canSpike = false; // スパイクができるか

    void Start()
    {
        // 必要に応じて初期設定
    }

    void Update()
    {
        HandleMovement(); // プレイヤーの移動処理
        HandleBallActions(); // ボールのアクション処理
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

        // ジャンプ処理
        if (canJump && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    // ボールのアクション処理
    void HandleBallActions()
    {
        if (isBallInRange && Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody ballRb = Ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.velocity = Vector3.zero;

                if (canReceive)
                {
                    // レシーブ処理
                    Vector3 randomPosition = ReceivePositions[Random.Range(0, ReceivePositions.Length)];
                    Vector3 force = CalculateLaunchVelocity(Ball.transform.position, randomPosition, verticalLift);
                    ballRb.AddForce(force, ForceMode.VelocityChange);

                    canReceive = false;
                    canToss = true;
                }
                else if (canToss)
                {
                    // トス処理
                    Vector3 randomPosition = TossPositions[Random.Range(0, TossPositions.Length)];
                    Vector3 force = CalculateLaunchVelocity(Ball.transform.position, randomPosition, verticalLift);
                    ballRb.AddForce(force, ForceMode.VelocityChange);

                    canToss = false;
                    canJump = true;
                    canSpike = true;
                }
                else if (canSpike && !isGrounded)
                {
                    // スパイク処理
                    Vector3 directionToTarget = (TargetArea.position - Ball.transform.position).normalized;
                    directionToTarget.x += Random.Range(-randomRange, randomRange);
                    directionToTarget.z += Random.Range(-randomRange, randomRange);
                    directionToTarget = directionToTarget.normalized;
                    Vector3 force = directionToTarget * hitForce;
                    force.y = verticalLift;
                    ballRb.AddForce(force, ForceMode.Impulse);

                    canSpike = false;
                    canReceive = true;
                    canJump = false;
                }
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

    // プレイヤーが地面に着地したときに呼ばれる
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Team_A_Ground"))
        {
            isGrounded = true;
        }
    }

    // ギズモを使ってレシーブエリアとトスエリアを視覚的に表示
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (ReceivePositions != null)
        {
            foreach (Vector3 pos in ReceivePositions)
            {
                Gizmos.DrawSphere(pos, 0.5f);
            }
        }

        Gizmos.color = Color.blue;
        if (TossPositions != null)
        {
            foreach (Vector3 pos in TossPositions)
            {
                Gizmos.DrawSphere(pos, 0.5f);
            }
        }
    }

    // 指定した位置に放物線を描いて飛ばすための初速度を計算
    Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float height)
    {
        float gravity = Physics.gravity.y;
        float displacementY = target.y - start.y;
        Vector3 displacementXZ = new Vector3(target.x - start.x, 0, target.z - start.z);
        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;
        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }
}
