using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject Ball; // �{�[���I�u�W�F�N�g
    public Transform TargetArea; // ����`�[���̃G���A
    public Vector3[] ReceivePositions; // �����̃`�[���̃��V�[�u�G���A�̍��W
    public Vector3[] TossPositions; // �g�X�G���A�̍��W
    public Vector3 homePosition; // NPC�̑ҋ@�ʒu
    public float moveSpeed = 5f; // NPC�̈ړ����x
    public float hitForce = 20f; // �{�[�����΂���
    public float verticalLift = 10f; // ��������`�����߂̏�����̗�
    public float randomRange = 0.5f; // �����_���ȕ����͈̔�
    public float jumpForce = 5f; // �W�����v��

    private bool isBallInRange = false; // �{�[�����͈͓��ɂ��邩
    private bool isGrounded = true; // NPC���n�ʂɂ��邩
    private bool canJump = false; // �W�����v���ł��邩
    private bool canReceive = true; // ���V�[�u���ł��邩
    private bool canToss = false; // �g�X���ł��邩
    private bool canSpike = false; // �X�p�C�N���ł��邩

    void Start()
    {
        // �K�v�ɉ����ď����ݒ�
    }

    void Update()
    {
        HandleMovement(); // NPC�̈ړ�����
        HandleBallActions(); // �{�[���̃A�N�V��������
    }

    // NPC�̈ړ�����
    void HandleMovement()
    {
        if (canReceive || canToss || canSpike)
        {
            // �{�[���Ɍ������Ĉړ�
            Vector3 directionToBall = (Ball.transform.position - transform.position).normalized;
            transform.Translate(directionToBall * moveSpeed * Time.deltaTime, Space.World);

            // �{�[���̋߂��ɓ��B�������~
            if (Vector3.Distance(transform.position, Ball.transform.position) < 1f)
            {
                transform.position = Ball.transform.position;
            }
        }
        else if (!isBallInRange)
        {
            // �{�[�����͈͊O�ɂ���Ƃ��͑ҋ@�ʒu�ɖ߂�
            Vector3 directionToHome = (homePosition - transform.position).normalized;
            transform.Translate(directionToHome * moveSpeed * Time.deltaTime, Space.World);

            // �ҋ@�ʒu�ɓ��B�������~
            if (Vector3.Distance(transform.position, homePosition) < 1f)
            {
                transform.position = homePosition;
            }
        }
    }

    // �{�[���̃A�N�V��������
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
                    // ���V�[�u����
                    Vector3 randomPosition = ReceivePositions[Random.Range(0, ReceivePositions.Length)];
                    Vector3 force = CalculateLaunchVelocity(Ball.transform.position, randomPosition, verticalLift);
                    ballRb.AddForce(force, ForceMode.VelocityChange);

                    canReceive = false;
                    canToss = true;
                }
                else if (canToss)
                {
                    // �g�X����
                    Vector3 randomPosition = TossPositions[Random.Range(0, TossPositions.Length)];
                    Vector3 force = CalculateLaunchVelocity(Ball.transform.position, randomPosition, verticalLift);
                    ballRb.AddForce(force, ForceMode.VelocityChange);

                    canToss = false;
                    canJump = true;
                    canSpike = true;
                }
                else if (canSpike && !isGrounded)
                {
                    // �X�p�C�N����
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

    // �{�[�����g���K�[���ɓ������Ƃ��ɔ͈͓��t���O�𗧂Ă�
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Ball)
        {
            isBallInRange = true;
        }
    }

    // �{�[�����g���K�[����o���Ƃ��ɔ͈͓��t���O�����Z�b�g
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Ball)
        {
            isBallInRange = false;
        }
    }

    // NPC���n�ʂɒ��n�����Ƃ��ɌĂ΂��
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Team_A_Ground"))
        {
            isGrounded = true;
        }
    }

    // �M�Y�����g���ă��V�[�u�G���A�ƃg�X�G���A�����o�I�ɕ\��
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

    // �w�肵���ʒu�ɕ�������`���Ĕ�΂����߂̏����x���v�Z
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
