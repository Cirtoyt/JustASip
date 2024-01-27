using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DishStack : MonoBehaviour
{
    [SerializeField] private BalancingPlatesManager balancingPlatesManager;
    [SerializeField] private List<Rigidbody> dishes = new();
    [SerializeField] private float wobbleScaler = 0.1f;
    [SerializeField] private float forceScaler = 4f;
    private void Update()
    {
        for (int i = 0; i < dishes.Count; i++)
        {
            float directionX = balancingPlatesManager.CurrentArmPosition.x * i * wobbleScaler;
            float directionZ = balancingPlatesManager.CurrentArmPosition.y * i * wobbleScaler;
            dishes[i].transform.localPosition = new( directionX, dishes[i].transform.localPosition.y, directionZ);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RemoveDish();
        }
    }

    public void RemoveDish()
    {
        Rigidbody topDish = dishes?.LastOrDefault();

        if (topDish == null)
        {
            return;
        }

        topDish.isKinematic = false;
        topDish.AddForce(new Vector3(balancingPlatesManager.CurrentArmPosition.x * forceScaler, forceScaler / 2f, balancingPlatesManager.CurrentArmPosition.y * forceScaler), ForceMode.Impulse);
        dishes.Remove(topDish);
    }
}
