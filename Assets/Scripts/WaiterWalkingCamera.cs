using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterWalkingCamera : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private WaiterWalkMovement waiter;

    [Header("Properties")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float smoothSpeed = 0.125f;


    void LateUpdate()
    {
        // Use SmoothDamp to smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, waiter.CameraPositionTarget.position, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(waiter.CameraLookAtTarget);
    }
}

