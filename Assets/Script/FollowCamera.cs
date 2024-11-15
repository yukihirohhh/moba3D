using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 追従するオブジェクト
    public Vector3 offset; // カメラとオブジェクトの距離

    void LateUpdate()
    {
        // カメラの位置を更新
        transform.position = target.position + offset;
    }
}
