using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BalancingPlatesManager : MonoBehaviour
{
    [SerializeField] private BalancingPlatesUI balancingPlatesUI;
    [SerializeField] private DishStack dishStack;
    [SerializeField] private float rightArmMovementSpeed = 1;
    [SerializeField] private float maxArmFromCentreDeviation = 1;

    private Vector2 moveRightArmDelta;
    private Vector2 currentArmPosition;
    private Vector2 targetWobbleDirection;
    private float minChangeWobbleTargetDelay;
    private float maxChangeWobbleTargetDelay;
    private float changeWobbleTargetDelay;
    private float changeWobbleTargetDelayTimer = 0;
    private float minWobbleIntensity;
    private float maxWobbleIntensity;
    private float wobbleIntensity;
    private AnimationCurve wobbleIntensitySmoothingCurve;
    private Vector2 currentWobbleTargetPosition;
    private Vector2 lastWobbleTargetPosition;
    private Vector2 currentWobblePosition;
    private Vector2 lastWobblePosition;
    private Vector2 currentWobbleDelta;

    public Vector2 CurrentArmPosition => currentArmPosition;

    public void OnBalancingArm(InputValue inputValue)
    {
        moveRightArmDelta = inputValue.Get<Vector2>();
    }

    public void BeginTheWobbling()
    {
        targetWobbleDirection = Vector2.zero;
        changeWobbleTargetDelayTimer = 0;
        currentWobbleTargetPosition = Vector2.zero;
        lastWobbleTargetPosition = Vector2.zero;
        currentWobblePosition = Vector2.zero;
        lastWobblePosition = Vector2.zero;
        currentWobbleDelta = Vector2.zero;

        // Set initial target position
        GenerateNewTargetWobblePosition();
    }

    private void GenerateNewTargetWobblePosition()
    {
        lastWobbleTargetPosition = currentWobbleTargetPosition;
        targetWobbleDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        currentWobbleTargetPosition = targetWobbleDirection * wobbleIntensity;
    }

    private void Update()
    {
        // Apply random wobble input
        GenerateWobbleDelta();
        currentArmPosition += currentWobbleDelta;

        // Apply correcting player input
        currentArmPosition += moveRightArmDelta * rightArmMovementSpeed;

        //Update UI
        balancingPlatesUI.UpdateArmPosition(currentArmPosition);

        // Check if out of range
        if (currentArmPosition.magnitude > maxArmFromCentreDeviation)
        {

            dishStack.RemoveDish();

            // Reset to centre?
            currentArmPosition = Vector2.zero;
        }
    }

    private void GenerateWobbleDelta()
    {
        // Check for new target direction
        changeWobbleTargetDelayTimer += Time.deltaTime;
        bool reachedDelayThisFrame = false;
        if (changeWobbleTargetDelayTimer >= changeWobbleTargetDelay)
        {
            GenerateNewTargetWobblePosition();

            reachedDelayThisFrame = true;
            changeWobbleTargetDelayTimer = 0;
            GenerateNewChangeWobbleTargetDelay();
        }

        float wobbleDelayPerc = reachedDelayThisFrame ? 1 : changeWobbleTargetDelayTimer / changeWobbleTargetDelay;
        lastWobblePosition = currentWobblePosition;
        if (reachedDelayThisFrame)
        {
            currentWobblePosition = lastWobbleTargetPosition;
        }
        else
        {
            // Lerp towards wobble target position
            currentWobblePosition = Vector2.Lerp(lastWobbleTargetPosition, currentWobbleTargetPosition, wobbleIntensitySmoothingCurve.Evaluate(wobbleDelayPerc));
        }

        currentWobbleDelta = currentWobblePosition - lastWobblePosition;
    }

    public void SetChangeWobbleDirectionDelay(float newMin, float newMax)
    {
        minChangeWobbleTargetDelay = newMin;
        maxChangeWobbleTargetDelay = newMax;

        GenerateNewChangeWobbleTargetDelay();
    }

    private void GenerateNewChangeWobbleTargetDelay()
    {
        changeWobbleTargetDelay = Random.Range(minChangeWobbleTargetDelay, maxChangeWobbleTargetDelay);
    }

    public void SetWobbleIntensity(float newMin, float newMax)
    {
        minWobbleIntensity = newMin;
        maxWobbleIntensity = newMax;
        
        wobbleIntensity = Random.Range(minWobbleIntensity, maxWobbleIntensity);
    }

    public void SetWobbleIntensitySmoothingCurve(AnimationCurve newCurve)
    {
        wobbleIntensitySmoothingCurve = newCurve;
    }
}
