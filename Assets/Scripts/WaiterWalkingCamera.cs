using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterWalkingCamera : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private WaiterWalkMovement waiter;
    [SerializeField] private LayerMask obstacleMask; 

    [Header("Properties")]
    [SerializeField] private float distance = 2f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float rotationSpeed = 20f;

    private float currentRotationAngle = 0f;

    void LateUpdate()
    {
        Vector3 playerForward = waiter.transform.forward;

        float targetRotationAngle = Mathf.Atan2(playerForward.x, playerForward.z) * Mathf.Rad2Deg;
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationSpeed * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
        Vector3 offset = new(0f, height, -distance);
        Vector3 desiredPosition = waiter.transform.position + rotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);

        if (Physics.Linecast(waiter.transform.position, desiredPosition, out RaycastHit hit, obstacleMask))
        {
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);
        }

        transform.LookAt(waiter.transform.position + Vector3.up * height);
    }
}


