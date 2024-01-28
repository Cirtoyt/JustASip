using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] private DrinksGiver drinksGiver;
    [SerializeField] private DishGiver dishGiver;
    [SerializeField] private DishStack dishStack;
    [SerializeField] private BalancingPlatesManager balancingPlatesManager;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private WaiterWalkMovement waiterWalkMovement;
    [SerializeField] private Text deliveredTextValue;
    [SerializeField] private Text smashedTextValue;
    [Header("Data")]
    [SerializeField] private List<DrunknessEffectDataSO> initialPresetDrunknessEffectData = new List<DrunknessEffectDataSO>();
    [SerializeField] private float initialScalingMinChangeWobbleTargetDelay = 1;
    [SerializeField] private float initialScalingMaxChangeWobbleTargetDelay = 2;
    [SerializeField] private float changeWobbleTargetDelayScaler = 0.02f;
    [SerializeField] private float initialScalingMinWobbleIntensity = 0.5f;
    [SerializeField] private float initialScalingMaxWobbleIntensity = 1.1f;
    [SerializeField] private float wobbleIntensityScaler = 0.01f;
    [SerializeField] private AnimationCurve scalingWobbleIntensitySmoothingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float arrowOnTableHeightOffset = 1;

    public TableTarget CurrentTargetTable;

    private int currentPresetDrunknessDataIndex;
    private float currentScalingChangeWobbleTargetDelayScale;
    private float currentScalingWobbleIntensityScale;
    private List<TableTarget> tableTargets = new List<TableTarget>();
    private GameObject spawnedArrow;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        foreach (var tableTarget in FindObjectsOfType<TableTarget>())
        {
            if (tableTarget.CanBeDeliveredTo)
                tableTargets.Add(tableTarget);
        }

        balancingPlatesManager.enabled = false;

        // Start cut-scene
        BeginFirstRound();
    }

    public void BeginFirstRound()
    {
        currentPresetDrunknessDataIndex = 0;
        currentScalingChangeWobbleTargetDelayScale = 1;
        currentScalingWobbleIntensityScale = 1;

        drinksGiver.SpawnDrinkInHand();
        drinksGiver.OfferDrink();

        dishGiver.DishUpNewDishStack();
        
        // Set initial drunkness
        IntensifyDrunkness(false);
    }

    public void IntensifyDrunkness(bool increaseAnim)
    {
        Debug.Log("IntensifyDrunkness!");
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

        if (increaseAnim) waiterWalkMovement.IncreaseDrunkness(0.2f);
    }

    public void PickUpNextDishStack()
    {
        dishGiver.GiveDishStackToWaiter();

        // Pick target table
        CurrentTargetTable = tableTargets[Random.Range(0, tableTargets.Count)];
        tableTargets.ForEach((tt) => tt.CanBeDeliveredTo = true);

        Vector3 targetArrowPos = new Vector3(CurrentTargetTable.transform.position.x, CurrentTargetTable.transform.position.y + arrowOnTableHeightOffset, CurrentTargetTable.transform.position.z);
        spawnedArrow = Instantiate(arrowPrefab, targetArrowPos, Quaternion.identity);

        balancingPlatesManager.enabled = true;
        balancingPlatesManager.BeginTheWobbling();
    }

    private int deliveredDishes = 0;
    private int smashedDishes = 0;
    public void DeliverDish(bool successfully)
    {
        deliveredDishes += dishStack.DishesHeld();
        smashedDishes += dishStack.DishesSmashed();
        dishStack.droppedDishes.Clear();
        deliveredTextValue.text = deliveredDishes.ToString();
        smashedTextValue.text = smashedDishes.ToString();

        balancingPlatesManager.enabled = false;
        balancingPlatesManager.ResetToCentre();

        CurrentTargetTable.CanBeDeliveredTo = false;
        CurrentTargetTable = null;

        Destroy(spawnedArrow);

        dishStack.RemoveDishes();

        // Bell ding sound effect from bar

        dishGiver.DishUpNewDishStack();

        if (!drinksGiver.DrinkIsPlaced)
            drinksGiver.ReturnToBarFrontWithDrink();
    }
}
