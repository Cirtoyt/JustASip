using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinksGiver : MonoBehaviour
{
    private bool hasDrink = true;

    public void OfferDrink()
    {
        // Place drink on counter

        hasDrink = false;

        ReturnToBackOfBar();
    }

    private void ReturnToBackOfBar()
    {
        // Walk to back
    }

    public void ReturnToBarFrontWithDrink()
    {
        // Spawn drink in hand

        hasDrink = true;

        // Walk to front with drink, then:
        //OfferDrink();
    }
}
