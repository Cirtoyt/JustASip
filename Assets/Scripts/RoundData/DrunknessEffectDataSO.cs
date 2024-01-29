using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDrunknessEffectDataSO", menuName = "DrunknessEffectDataSO")]
public class DrunknessEffectDataSO : ScriptableObject
{
    public float MinChangeWobbleTargetDelay;
    public float MaxChangeWobbleTargetDelay;
    public float MinWobbleIntensity;
    public float MaxWobbleIntensity;
    public AnimationCurve WobbleIntensitySmoothingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // Any drunk visual effect data
}
