using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject teamA; // Team A�̃G���A
    public GameObject teamB; // Team B�̃G���A

    private GameObject ball; // �{�[���I�u�W�F�N�g
    private bool isBallInTeamA = false; // �{�[����Team A�̃G���A�ɂ��邩
    private bool isBallInTeamB = false; // �{�[����Team B�̃G���A�ɂ��邩

    void Start()
    {
        // �{�[���I�u�W�F�N�g���擾
        ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball == null)
        {
            Debug.LogError("Ball object not found. Make sure the Ball is tagged correctly.");

            // Debugging: List all objects with the "Ball" tag
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
            Debug.Log("Number of objects with 'Ball' tag: " + balls.Length);
            foreach (GameObject obj in balls)
            {
                Debug.Log("Found object with 'Ball' tag: " + obj.name);
            }
        }
        else
        {
            Debug.Log("Ball object found: " + ball.name);
        }
    }


    void Update()
    {
        if (ball != null)
        {
            // �{�[���̈ʒu����Ƀ`�F�b�N
            CheckBallPosition();
        }
    }

    void CheckBallPosition()
    {
        // Team A�̃G���A�Ƀ{�[�������邩�`�F�b�N
        isBallInTeamA = teamA.GetComponent<Collider>().bounds.Contains(ball.transform.position);

        // Team B�̃G���A�Ƀ{�[�������邩�`�F�b�N
        isBallInTeamB = teamB.GetComponent<Collider>().bounds.Contains(ball.transform.position);

        // �f�o�b�O�p�ɃR���\�[���ɏo��
        Debug.Log("Ball in Team A Area: " + isBallInTeamA);
        Debug.Log("Ball in Team B Area: " + isBallInTeamB);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.gameObject.transform.IsChildOf(teamA.transform))
            {
                isBallInTeamA = true;
                isBallInTeamB = false;
            }
            else if (other.gameObject.transform.IsChildOf(teamB.transform))
            {
                isBallInTeamA = false;
                isBallInTeamB = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.gameObject.transform.IsChildOf(teamA.transform))
            {
                isBallInTeamA = false;
            }
            else if (other.gameObject.transform.IsChildOf(teamB.transform))
            {
                isBallInTeamB = false;
            }
        }
    }
}
