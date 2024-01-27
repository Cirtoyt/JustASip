using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaiterWalkMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Properties")]
    [Range(0f, 1f)] [SerializeField] private float drunkness;
    [SerializeField] float runSpeed = 3.5f;
    [SerializeField] float walkSpeed = 1f;

    private float smoothVelocity;
    private readonly float smoothTime = 0.1f;

    public Vector2 WalkDirection { get; private set; }
    public float Drunkness => drunkness;
    public float MoveSpeed => ((runSpeed - walkSpeed) * drunkness) + walkSpeed;


    private void Update()
    {
        MovementHandler();
    }

    public void MovementHandler()
    {
        // Calculate the movement direction.
        Vector3 movement = new Vector3(WalkDirection.x, 0f, WalkDirection.y).normalized;

        animator.SetFloat("speed", drunkness);
        animator.SetBool("isMoving", movement.magnitude > 0.1f);

        // Check if there is any input, then move the character.
        if (movement.magnitude > 0.1f)
        {
            // Calculate the rotation angle based on the input.
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothTime);

            // Apply rotation to the character
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Calculate the movement direction in world space.
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Move the character.
            transform.Translate(moveDirection * MoveSpeed * Time.deltaTime, Space.World);
        }
    }

    public void OnWalkingMovement(InputValue value)
    {
        WalkDirection = value.Get<Vector2>();
    }
}
