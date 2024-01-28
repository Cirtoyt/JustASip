using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] private DrinksGiver drinksGiver;
    [SerializeField] private DishStack dishStack;
    [SerializeField] private BalancingPlatesManager balancingPlatesManager;
    [Header("Data")]
    [SerializeField] private List<DrunknessEffectDataSO> initialPresetDrunknessEffectData = new List<DrunknessEffectDataSO>();
    [SerializeField] private float initialScalingMinChangeWobbleTargetDelay;
    [SerializeField] private float initialScalingMaxChangeWobbleTargetDelay;
    [SerializeField] private float changeWobbleTargetDelayScaler = 0.02f;
    [SerializeField] private float initialScalingMinWobbleIntensity;
    [SerializeField] private float initialScalingMaxWobbleIntensity;
    [SerializeField] private float wobbleIntensityScaler = 0.02f;
    [SerializeField] private AnimationCurve scalingWobbleIntensitySmoothingCurve;

    private int currentPresetDrunknessDataIndex;
    private float currentScalingChangeWobbleTargetDelayScale;
    private float currentScalingWobbleIntensityScale;

    public void BeginFirstRound()
    {
        currentPresetDrunknessDataIndex = 0;
        currentScalingChangeWobbleTargetDelayScale = 1;
        currentScalingWobbleIntensityScale = 1;

        drinksGiver.OfferDrink();
    }

    public void IntensityDrunkness()
    {
        if (currentPresetDrunknessDataIndex + 1 < initialPresetDrunknessEffectData.Count)
        {
            currentPresetDrunknessDataIndex++;
            DrunknessEffectDataSO drunknessEffectData = initialPresetDrunknessEffectData[currentPresetDrunknessDataIndex];
            balancingPlatesManager.SetChangeWobbleDirectionDelay(drunknessEffectData.MinChangeWobbleTargetDelay, drunknessEffectData.MaxChangeWobbleTargetDelay);
            balancingPlatesManager.SetWobbleIntensity(drunknessEffectData.MinWobbleIntensity, drunknessEffectData.MaxWobbleIntensity);
            balancingPlatesManager.SetWobbleIntensitySmoothingCurve(drunknessEffectData.WobbleIntensitySmoothingCurve);
        }
        else
        {
            balancingPlatesManager.SetChangeWobbleDirectionDelay(initialScalingMinChangeWobbleTargetDelay * currentScalingChangeWobbleTargetDelayScale,
                                                                 initialScalingMaxChangeWobbleTargetDelay * currentScalingChangeWobbleTargetDelayScale);
            balancingPlatesManager.SetWobbleIntensity(initialScalingMinWobbleIntensity * currentScalingWobbleIntensityScale,
                                                      initialScalingMaxWobbleIntensity * currentScalingWobbleIntensityScale);
            balancingPlatesManager.SetWobbleIntensitySmoothingCurve(scalingWobbleIntensitySmoothingCurve);

            currentScalingChangeWobbleTargetDelayScale += changeWobbleTargetDelayScaler;
            currentScalingWobbleIntensityScale += wobbleIntensityScaler;
        }
    }

    public void PickUpNextDish()
    {
        //dishStack.AddDishes();

        balancingPlatesManager.BeginTheWobbling();
    }

    public void DeliverDish()
    {
        // Bell ding sound effect from bar
        // SHow something visually to say go back to bar

        drinksGiver.ReturnToBarFrontWithDrink();
    }

    private void Update()
    {
        // Start cut-scene

        // Grab a drink

        // Pick-up dishes

        // Deliver

        // Go back to bar
    }
}
