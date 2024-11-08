using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Ball; // �{�[�����A�^�b�`����I�u�W�F�N�g
    public float moveSpeed = 5f; // �v���C���[�̈ړ����x
    public float ballLiftForce = 10f; // �{�[������ɂ������

    private bool isBallInRange = false; // �{�[�����͈͓��ɂ��邩

    void Start()
    {
        // �K�v�ɉ����ď����ݒ�
    }

    void Update()
    {
        HandleMovement(); // �v���C���[�̈ړ�����
        HandleBallLift(); // �{�[������ɂ����鏈��
    }

    // �v���C���[�̈ړ�����
    void HandleMovement()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // W, A, S, D�L�[�ňړ�
        if (Input.GetKey(KeyCode.W)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    // �{�[������ɂ����鏈��
    void HandleBallLift()
    {
        // �{�[�����͈͓�����Q�L�[�������ꂽ�Ƃ�
        if (isBallInRange && Input.GetKeyDown(KeyCode.Q))
        {
            Rigidbody ballRb = Ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.velocity = Vector3.zero; // �{�[���̑��x�����Z�b�g
                ballRb.AddForce(Vector3.up * ballLiftForce, ForceMode.Impulse); // �{�[������ɂ�����
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
