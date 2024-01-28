using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestBalancingPlatesLevelController))]
public class TestBalancingPlatesLevelControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestBalancingPlatesLevelController script = target as TestBalancingPlatesLevelController;

        if (GUILayout.Button("Begin The Wobbling"))
        {
            script.BeginTheWobbling();
        }

        if (GUILayout.Button("Start Drink Giver"))
        {
            script.roundManager.BeginFirstRound();
        }

        if (GUILayout.Button("Return Drink Giver To Bar Front"))
        {
            script.drinksGiver.ReturnToBarFrontWithDrink();
        }
    }
}
