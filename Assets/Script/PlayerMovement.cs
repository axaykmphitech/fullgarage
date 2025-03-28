using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform playerTransform;

    private Vector3 cameraOffset;

    [Range(0.01f,1.0f)]
    public float smothfactor = 0.5f;

    public bool lookAtPlayer = false;
    public bool rotatrAroundPlayer = true;

    public float RotationsSpeed = 5.0f;

    private void Start()
    {
        cameraOffset = transform.position - playerTransform.position;
    }


    private void LateUpdate()
    {
        if (rotatrAroundPlayer)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed,Vector3.up);
            cameraOffset = camTurnAngle * cameraOffset;
        }

        Vector3 newPos = playerTransform.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smothfactor);

        if (lookAtPlayer || rotatrAroundPlayer)
        {
            transform.LookAt(playerTransform);
        }

    }
}
