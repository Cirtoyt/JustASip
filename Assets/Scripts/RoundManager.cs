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
    [SerializeField] private float initialScalingMinChangeWobbleTargetDelay = 1;
    [SerializeField] private float initialScalingMaxChangeWobbleTargetDelay = 2;
    [SerializeField] private float changeWobbleTargetDelayScaler = 0.02f;
    [SerializeField] private float initialScalingMinWobbleIntensity = 0.5f;
    [SerializeField] private float initialScalingMaxWobbleIntensity = 1.1f;
    [SerializeField] private float wobbleIntensityScaler = 0.01f;
    [SerializeField] private AnimationCurve scalingWobbleIntensitySmoothingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private int currentPresetDrunknessDataIndex;
    private float currentScalingChangeWobbleTargetDelayScale;
    private float currentScalingWobbleIntensityScale;
    private List<TableTarget> tableTargets = new List<TableTarget>();

    private void Start()
    {
        foreach (var tableTarget in FindObjectsOfType<TableTarget>())
        {
            if (tableTarget.CanBeDeliveredTo)
                tableTargets.Add(tableTarget);
        }

        // Start cut-scene
    }

    public void BeginFirstRound()
    {
        currentPresetDrunknessDataIndex = 0;
        currentScalingChangeWobbleTargetDelayScale = 1;
        currentScalingWobbleIntensityScale = 1;

        drinksGiver.SpawnDrinkInHand();
        drinksGiver.OfferDrink();
    }

    public void IntensifyDrunkness()
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
        // Show something visually to say go back to bar

        drinksGiver.ReturnToBarFrontWithDrink();
    }
}
