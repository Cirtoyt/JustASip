using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private WaiterInteractions waiterInteractions;
    [SerializeField] private Text deliveredTextValue;
    [SerializeField] private Text smashedTextValue;
    [SerializeField] private GameObject introTextValue;
    [SerializeField] private Text countdownTextValue;
    [SerializeField] private Text roundTimerTextValue;
    [SerializeField] private GameObject finalScoreUI;
    [SerializeField] private GameObject finalScoreDeliveredMinusSmashedTextLabel;
    [SerializeField] private Text finalScoreDeliveredMinusSmashedTextValue;
    [SerializeField] private GameObject finalScoreDrunknessMultiplierTextLabel;
    [SerializeField] private Text finalScoreDrunknessMultiplierTextValue;
    [SerializeField] private GameObject finalScoreTextLabel;
    [SerializeField] private Text finalScoreTextValue;
    [SerializeField] private GameObject finalScoreReplayTextLabel;
    [Header("Data")]
    [SerializeField] private bool useDrunknessData = true;
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
    private int deliveredDishes = 0;
    private int smashedDishes = 0;
    private float roundTimer = 300;
    private int drinksDrank = 0;
    private bool roundInProgress = false;
    private bool roundCompleted = false;

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
        roundTimerTextValue.text = roundTimer.ToString("F2");
        countdownTextValue.gameObject.SetActive(false);
        finalScoreUI.SetActive(false);
        finalScoreDeliveredMinusSmashedTextValue.text = "";
        finalScoreDrunknessMultiplierTextValue.text = "";
        finalScoreTextValue.text = "";

        // Start cut-scene
        StartCoroutine(StartCutScene());
    }

    private IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(8);

        introTextValue.SetActive(false);

        SoundManager.Instance.countdownAudioSource.Play();
        countdownTextValue.gameObject.SetActive(true);
        countdownTextValue.text = "Deliver";

        yield return new WaitForSeconds(1);

        countdownTextValue.text = "Those";

        yield return new WaitForSeconds(1);

        countdownTextValue.text = "Dishes!";

        yield return new WaitForSeconds(1);

        countdownTextValue.gameObject.SetActive(false);

        BeginFirstRound();
    }

    public void BeginFirstRound()
    {
        SoundManager.Instance.musicAudioSource.Play();

        currentPresetDrunknessDataIndex = 0;
        currentScalingChangeWobbleTargetDelayScale = 1;
        currentScalingWobbleIntensityScale = 1;

        drinksGiver.SendToBackOfBar();

        dishGiver.DishUpNewDishStack();
        
        // Set initial drunkness
        IntensifyDrunkness(true);

        roundInProgress = true;
    }

    private void Update()
    {
        if (roundCompleted)
        {
            // Check to replay by reloading scene
            if (Input.GetKeyDown(KeyCode.E))
                SceneManager.LoadScene(0);
        }

        if (!roundInProgress)
            return;

        roundTimer -= Time.deltaTime;
        roundTimerTextValue.text = roundTimer.ToString("F2");

        if (roundTimer <= 0)
        {
            roundInProgress = false;
            roundTimerTextValue.text = "0.00";
            StartCoroutine(DisplayFinalScoreDramatically());
        }
    }

    private IEnumerator DisplayFinalScoreDramatically()
    {
        finalScoreUI.SetActive(true);
        finalScoreDeliveredMinusSmashedTextLabel.SetActive(false);
        finalScoreDrunknessMultiplierTextLabel.SetActive(false);
        finalScoreTextLabel.SetActive(false);
        finalScoreReplayTextLabel.SetActive(false);

        SoundManager.Instance.dishesAudioSource.Stop();
        balancingPlatesManager.enabled = false;
        waiterWalkMovement.enabled = false;
        waiterInteractions.enabled = false;

        yield return new WaitUntil(() =>
        {
            SoundManager.Instance.musicAudioSource.volume -= 0.1f * Time.deltaTime;
            return SoundManager.Instance.musicAudioSource.volume <= 0;
        });
        SoundManager.Instance.musicAudioSource.Stop();

        yield return new WaitForSeconds(1);

        finalScoreDeliveredMinusSmashedTextLabel.SetActive(true);
        finalScoreDeliveredMinusSmashedTextValue.text = (deliveredDishes - smashedDishes).ToString();

        yield return new WaitForSeconds(1);

        finalScoreDrunknessMultiplierTextLabel.SetActive(true);
        finalScoreDrunknessMultiplierTextValue.text = "x" + drinksDrank.ToString();

        yield return new WaitForSeconds(2);

        finalScoreTextLabel.SetActive(true);
        finalScoreTextValue.text = ((deliveredDishes - smashedDishes) * drinksDrank).ToString() + " Points!";
        SoundManager.Instance.clappingAudioSource.Play();

        yield return new WaitForSeconds(1.5f);

        finalScoreReplayTextLabel.SetActive(true);
        roundCompleted = true;
    }

    public void IntensifyDrunkness(bool initialDrink)
    {
        Debug.Log("IntensifyDrunkness!");

        if (!initialDrink) SoundManager.Instance.gulpDrinkAudioSource.Play();
        drinksDrank++;

        if (currentPresetDrunknessDataIndex + 1 < initialPresetDrunknessEffectData.Count && useDrunknessData)
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
            if (currentScalingChangeWobbleTargetDelayScale <= 0) currentScalingChangeWobbleTargetDelayScale = 0.01f;
            currentScalingWobbleIntensityScale += wobbleIntensityScaler;
            if (currentScalingWobbleIntensityScale <= 0) currentScalingWobbleIntensityScale = 0.01f;
        }

        if (!initialDrink) waiterWalkMovement.IncreaseDrunkness(0.2f);
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

    public void DeliverDish(bool successfully)
    {
        SoundManager.Instance.dishesAudioSource.Stop();

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
