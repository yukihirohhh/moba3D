using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // �Ǐ]����I�u�W�F�N�g
    public Vector3 offset; // �J�����ƃI�u�W�F�N�g�̋���

    void LateUpdate()
    {
        // �J�����̈ʒu���X�V
        transform.position = target.position + offset;
    }
}
