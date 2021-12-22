using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform camTr;     // 카메라의 transform
    Transform tr;               // 오브젝트의 transform
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        // 오브젝트를 카메라 방향으로 회전
        tr.LookAt(camTr.position);
    }
}
