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

    [Header("Properties")]
    [Range(0f, 1f)] [SerializeField] private float drunkness;
    [SerializeField] private float rotationSpeed = 80f;
    [SerializeField] private float runSpeed = 3.5f;
    [SerializeField] private float walkSpeed = 1f;

    public Vector2 MoveDirection { get; private set; }
    public float Drunkness => drunkness;
    public float MoveSpeed => ((runSpeed - walkSpeed) * drunkness) + walkSpeed;

    private void Update()
    {
        // Rotate the player based on input
        transform.Rotate(Vector3.up, MoveDirection.x * rotationSpeed * Time.deltaTime);

        // Get the forward direction of the player
        Vector3 forwardDirection = transform.forward;

        animator.SetFloat("speed", drunkness);
        animator.SetBool("isMoving", MoveDirection.magnitude > 0.1f);

        // Get vertical input for movement
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement in the forward direction relative to the player's rotation
        Vector3 moveAmount = forwardDirection * verticalInput * MoveSpeed * Time.deltaTime;

        // Translate the player in the calculated direction
        transform.Translate(moveAmount, Space.World);
    }

    public void OnWalkingMovement(InputValue value)
    {
        MoveDirection = value.Get<Vector2>();
    }
}
