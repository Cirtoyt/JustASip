using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinksGiver : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] private Animator anim;
    [SerializeField] private List<GameObject> drinkPrefabs = new List<GameObject>();
    [SerializeField] private Transform holdDrinkAnchor;
    [SerializeField] private Transform placeDrinkAnchor;
    [SerializeField] private Transform FrontOfBarAnchor;
    [SerializeField] private Transform BackOfBarAnchor;
    [SerializeField] private Transform waiter;
    [Header("Settings")]
    [SerializeField] private float placeDrinkAnimWeight = 1;
    [SerializeField] private float placeDrinkAnimLength = 1;
    [SerializeField] private float pauseAferDrinkPlaceLength = 1;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float waiterDrinksMinDistance = 1f;

    private enum States
    {
        StandingAtFrontOfBar,
        OfferingDrink,
        WalkingToBackOfBar,
        StandingAtBackOfBar,
        WalkingToFrontOfBar,
    }

    private States state;
    public Transform HeldDrink;
    public bool DrinkIsPlaced = false;

    private void Update()
    {
        switch (state)
        {
            case States.StandingAtFrontOfBar:
                if (Vector3.Distance(waiter.position, transform.position) <= waiterDrinksMinDistance)
                {
                    state = States.OfferingDrink;
                    OfferDrink();
                }
                break;
            case States.OfferingDrink:
                break;
            case States.WalkingToBackOfBar:
                if (!TryWalkToTargetAnchor(BackOfBarAnchor, FrontOfBarAnchor))
                    state = States.StandingAtBackOfBar;
                break;
            case States.StandingAtBackOfBar:
                break;
            case States.WalkingToFrontOfBar:
                if (!TryWalkToTargetAnchor(FrontOfBarAnchor, BackOfBarAnchor))
                    state = States.StandingAtFrontOfBar;
                break;
        }
    }

    private bool TryWalkToTargetAnchor(Transform targetAnchor, Transform originAnchor)
    {
        if (Vector3.Distance(transform.position, targetAnchor.position) > 0.1f)
        {
            anim.SetBool("Walking", true);

            transform.position += (targetAnchor.position - transform.position).normalized * walkSpeed * Time.deltaTime;

            Vector3 targetDirection = (targetAnchor.position - originAnchor.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection, Vector3.up), rotationSpeed);

            return true;
        }
        else
        {
            anim.SetBool("Walking", false);

            return false;
        }
    }

    public void OfferDrink()
    {
        StartCoroutine(OfferDrinkCoroutine());
    }

    private IEnumerator OfferDrinkCoroutine()
    {
        // Place drink on counter
        anim.SetLayerWeight(1, placeDrinkAnimWeight);
        anim.SetTrigger("PlaceDrink");
        
        yield return new WaitForSeconds(placeDrinkAnimLength);

        anim.SetLayerWeight(1, 0);

        yield return PlaceDrinkOnCounter();

        yield return new WaitForSeconds(pauseAferDrinkPlaceLength);

        // Return to back of bar
        state = States.WalkingToBackOfBar;
    }

    public void SpawnDrinkInHand()
    {
        int drinkToSpawnIndex = Random.Range(0, drinkPrefabs.Count);
        HeldDrink = Instantiate(drinkPrefabs[drinkToSpawnIndex], holdDrinkAnchor.position, holdDrinkAnchor.rotation, holdDrinkAnchor).transform;
        HeldDrink.transform.GetChild(0).gameObject.SetActive(false);
        anim.SetBool("HoldingDrink", true);
    }

    private IEnumerator PlaceDrinkOnCounter()
    {
        yield return new WaitUntil(() => HeldDrink != null);

        HeldDrink.parent = placeDrinkAnchor;
        HeldDrink.localPosition = Vector3.zero;
        HeldDrink.localRotation = Quaternion.identity;
        anim.SetBool("HoldingDrink", false);
        DrinkIsPlaced = true;
        HeldDrink.transform.GetChild(0).gameObject.SetActive(true);
        SoundManager.Instance.placeDrinkAudioSource.Play();
    }

    public void DestroyDrinkPlaced()
    {
        Destroy(HeldDrink.gameObject);
        HeldDrink = null;
        DrinkIsPlaced = false;
    }

    public void SendToBackOfBar()
    {
        state = States.WalkingToBackOfBar;
    }

    public void ReturnToBarFrontWithDrink()
    {
        SpawnDrinkInHand();

        // Walk to front with drink
        state = States.WalkingToFrontOfBar;
    }
}
