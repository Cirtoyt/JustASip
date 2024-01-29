using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterInteractions : MonoBehaviour
{
    [SerializeField] private DishGiver dishGiver;
    [SerializeField] private DrinksGiver drinksGiver;
    [SerializeField] private RoundManager roundManager;
    [Space]
    [SerializeField] private float distanceToTableRequired = 1;
    [SerializeField] private float distanceToDishesRequired = 1;
    [SerializeField] private float distanceToDrinkRequired = 1;

    public void OnInteract()
    {
        if (!enabled)
            return;

        if (roundManager.CurrentTargetTable != null)
        {
            if (Vector3.Distance(roundManager.CurrentTargetTable.transform.position, transform.position) <= distanceToTableRequired)
            {
                // Delivery to table
                roundManager.DeliverDish(true);
                return;
            }
        }

        if (Vector3.Distance(dishGiver.transform.position, transform.position) <= distanceToDishesRequired)
        {
            // Pick-up dishes
            if (dishGiver.hasDishesReady)
                roundManager.PickUpNextDishStack();
            return;
        }

        if (drinksGiver.DrinkIsPlaced)
        {
            if (Vector3.Distance(drinksGiver.HeldDrink.position, transform.position) <= distanceToDrinkRequired)
            {
                // Pick-up drink
                drinksGiver.DestroyDrinkPlaced();
                roundManager.IntensifyDrunkness(false);
                return;
            }
        }
    }
}
