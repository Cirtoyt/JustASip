using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBalancingPlatesLevelController : MonoBehaviour
{
    [SerializeField] BalancingPlatesManager balancingPlatesController;
    [Space]
    [SerializeField] private float minChangeWobbleTargetDelay;
    [SerializeField] private float maxChangeWobbleTargetDelay;
    [SerializeField] private float minWobbleIntensity;
    [SerializeField] private float maxWobbleIntensity;
    [SerializeField] private AnimationCurve wobbleIntensitySmoothingCurve;

    public void BeginTheWobbling()
    {
        if (!Application.isPlaying)
            return;

        balancingPlatesController.enabled = true;
        balancingPlatesController.SetChangeWobbleDirectionDelay(minChangeWobbleTargetDelay, maxChangeWobbleTargetDelay);
        balancingPlatesController.SetWobbleIntensity(minWobbleIntensity, maxWobbleIntensity);
        balancingPlatesController.SetWobbleIntensitySmoothingCurve(wobbleIntensitySmoothingCurve);
        balancingPlatesController.BeginTheWobbling();
    }
}
