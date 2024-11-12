using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaController : MonoBehaviour
{
    public GameObject Ball; // �{�[���I�u�W�F�N�g
    private bool isReceive = false; // ���V�[�u���
    private bool isToss = false;    // �g�X���
    private bool isSpike = false;   // �X�p�C�N���
    private int touchCount = 0;     // �{�[���ɐG�ꂽ��

    void Start()
    {
        ResetStates(); // ���������ɏ�Ԃ����Z�b�g
    }

    void Update()
    {
        // ���݂̏�Ԃ��f�o�b�O�\���i���p�����悤�Ɂj
        if (isReceive)
        {
            Debug.Log("���݂̓��V�[�u��Ԃł��B");
        }
        else if (isToss)
        {
            Debug.Log("���݂̓g�X��Ԃł��B");
        }
        else if (isSpike)
        {
            Debug.Log("���݂̓X�p�C�N��Ԃł��B");
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
            Debug.Log("���V�[�u���");
        }
        else if (touchCount == 2)
        {
            isReceive = false;
            isToss = true;
            isSpike = false;
            Debug.Log("�g�X���");
        }
        else if (touchCount == 3)
        {
            isReceive = false;
            isToss = false;
            isSpike = true;
            Debug.Log("�X�p�C�N���");
        }
    }

    private void ResetStates()
    {
        isReceive = false;
        isToss = false;
        isSpike = false;
        touchCount = 0;
        Debug.Log("��ԃ��Z�b�g");
    }
}
