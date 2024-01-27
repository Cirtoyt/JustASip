using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaiterWalkMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraLookAtTarget;
    [SerializeField] private Transform cameraPositionTarget;
    [SerializeField] private WaiterWalkingCamera walkingCamera;

    [Header("Properties")]
    [Range(0f, 1f)] [SerializeField] private float drunkness;
    [SerializeField] float runSpeed = 3.5f;
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] private float rotationSmoothing = 0.1f;

    public Transform CameraLookAtTarget => cameraLookAtTarget;
    public Transform CameraPositionTarget => cameraPositionTarget;
    public Vector2 WalkDirection { get; private set; }

    private float rotationSmoothVelocity;

    public void OnWalkingMovement(InputValue value)
    {
        WalkDirection = value.Get<Vector2>();
    }

    //private void Update()
    //{
    //    HandleRotation();

    //    HandleMovement();
    //}

    //private void HandleRotation()
    //{
    //    Vector3 rawMovement = new Vector3(WalkDirection.x, 0f, WalkDirection.y).normalized;

    //    if (rawMovement.magnitude > 0.1f)
    //    {
    //        // Calculate the rotation angle based on the input.
    //        float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
    //        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothVelocity, rotationSmoothing);

    //        // Apply rotation to the character
    //        transform.rotation = Quaternion.Euler(0f, angle, 0f);

    //        // Calculate the movement direction in world space.
    //        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

    //        // Move the character.
    //        float moveSpeed = ((runSpeed - walkSpeed) * drunkness) + walkSpeed;
    //        transform.Translate(test3 * moveSpeed * Time.deltaTime, Space.World);
    //    }
    //}

    //public void HandleMovement()
    //{
    //    animator.SetFloat("speed", drunkness);
    //    animator.SetBool("isMoving", rawMovement.magnitude > 0.1f);

    //    Vector3 qwe = walkingCamera.transform.forward;
    //    Vector3 rty = walkingCamera.transform.right;

    //    qwe.y = 0;
    //    rty.y = 0;

    //    qwe.Normalize();
    //    rty.Normalize();

    //    Vector3 test1 = qwe * WalkDirection.y;
    //    Vector3 test2 = rty * WalkDirection.x;

    //    Vector3 test3 = test1 + test2;

    //    // Check if there is any input, then move the character.
    //    if (rawMovement.magnitude > 0.1f)
    //    {
    //        // Calculate the rotation angle based on the input.
    //        float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
    //        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothVelocity, rotationSmoothing);

    //        // Apply rotation to the character
    //        transform.rotation = Quaternion.Euler(0f, angle, 0f);

    //        // Calculate the movement direction in world space.
    //        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

    //        // Move the character.
    //        float moveSpeed = ((runSpeed - walkSpeed) * drunkness) + walkSpeed;
    //        transform.Translate(test3 * moveSpeed * Time.deltaTime, Space.World);
    //    }
    //}
}
