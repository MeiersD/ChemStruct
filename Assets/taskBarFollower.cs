using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBarFollower : MonoBehaviour
{
    public Transform xrCamera;
    public float distance = 0.5f;
    public float verticalOffset = -0.2f;

    void Update()
    {
        Vector3 targetPosition = xrCamera.position + xrCamera.forward * distance;
        targetPosition.y += verticalOffset;
        transform.position = targetPosition;
        transform.rotation = Quaternion.LookRotation(transform.position - xrCamera.position);
    }
}
