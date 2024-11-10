using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField]
    protected Transform trackingTarget;

    void LateUpdate()
    {
        transform.position = trackingTarget.transform.position + new Vector3(0, 1, -5);
    }
}

