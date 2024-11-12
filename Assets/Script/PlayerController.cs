using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Ball; // �{�[���I�u�W�F�N�g
    public Transform TargetArea; // ����`�[���̃G���A
    public float moveSpeed = 5f; // �v���C���[�̈ړ����x
    public float hitForce = 20f; // �{�[�����΂���
    public float verticalLift = 10f; // ��������`�����߂̏�����̗�
    public float randomRange = 0.5f; // �����_���ȕ����͈̔�

    private bool isBallInRange = false; // �{�[�����͈͓��ɂ��邩

    void Start()
    {
        // �K�v�ɉ����ď����ݒ�
    }

    void Update()
    {
        HandleMovement(); // �v���C���[�̈ړ�����
        HandleBallHit(); // �{�[����ł��Ԃ�����
    }

    // �v���C���[�̈ړ�����
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

    // �{�[����ł��Ԃ�����
    void HandleBallHit()
    {
        if (isBallInRange && Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody ballRb = Ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.velocity = Vector3.zero;

                // �^�[�Q�b�g�G���A�����̃x�[�X�x�N�g�����擾
                Vector3 directionToTarget = (TargetArea.position - Ball.transform.position).normalized;

                // �����_���Ȕ͈͂ŏ���������ς���
                directionToTarget.x += Random.Range(-randomRange, randomRange);
                directionToTarget.z += Random.Range(-randomRange, randomRange);

                // ���K�����Đ����͂�������
                directionToTarget = directionToTarget.normalized;
                Vector3 force = directionToTarget * hitForce;

                // ������̗͂������ĕ�������`��
                force.y = verticalLift;

                // �{�[���ɍŏI�I�ȗ͂�������
                ballRb.AddForce(force, ForceMode.Impulse);
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
}
