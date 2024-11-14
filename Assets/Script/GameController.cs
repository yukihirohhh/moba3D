using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject teamA; // Team Aのエリア
    public GameObject teamB; // Team Bのエリア

    private GameObject ball; // ボールオブジェクト
    private bool isBallInTeamA = false; // ボールがTeam Aのエリアにいるか
    private bool isBallInTeamB = false; // ボールがTeam Bのエリアにいるか

    void Start()
    {
        // ボールオブジェクトを取得
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
            // ボールの位置を常にチェック
            CheckBallPosition();
        }
    }

    void CheckBallPosition()
    {
        // Team Aのエリアにボールがいるかチェック
        isBallInTeamA = teamA.GetComponent<Collider>().bounds.Contains(ball.transform.position);

        // Team Bのエリアにボールがいるかチェック
        isBallInTeamB = teamB.GetComponent<Collider>().bounds.Contains(ball.transform.position);

        // デバッグ用にコンソールに出力
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
