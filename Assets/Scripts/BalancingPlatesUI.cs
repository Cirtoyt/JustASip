using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalancingPlatesUI : MonoBehaviour
{
    [SerializeField] private RectTransform UIContainer;
    [SerializeField] private Image centreDot;
    [SerializeField] private Image armPositionDot;

    public void UpdateArmPosition(Vector2 currentArmPosition)
    {
        Vector3 centreDotPos = centreDot.rectTransform.localPosition;
        Vector3 scaledCurrentArmPosition = new Vector3(currentArmPosition.x * (UIContainer.rect.width / 2), currentArmPosition.y * (UIContainer.rect.height / 2), 0);
        armPositionDot.rectTransform.localPosition = centreDotPos + scaledCurrentArmPosition;
    }
}
