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

        if (GUILayout.Button("Begin The Wobbling"))
        {
            TestBalancingPlatesLevelController script = target as TestBalancingPlatesLevelController;
            script.BeginTheWobbling();
        }
    }
}
