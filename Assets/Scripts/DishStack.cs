using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DishStack : MonoBehaviour
{
    [SerializeField] private BalancingPlatesManager balancingPlatesManager;
    [SerializeField] private List<Rigidbody> dishes = new List<Rigidbody>();
    [SerializeField] private float wobbleScaler = 0.1f;
    [SerializeField] private float forceScaler = 4f;

    public List<Rigidbody> droppedDishes = new List<Rigidbody>();

    private void Update()
    {
        // Display dishes stacked
        for (int i = 0; i < dishes.Count; i++)
        {
            float directionX = balancingPlatesManager.CurrentArmPosition.x * i * wobbleScaler;
            float directionZ = balancingPlatesManager.CurrentArmPosition.y * i * wobbleScaler;
            dishes[i].transform.localPosition = new( directionX, dishes[i].transform.localPosition.y, directionZ);
        }
    }

    public void AddDishes(List<Rigidbody> newDishes)
    {
        dishes = newDishes;
        dishes.ForEach((d) => d.transform.parent = transform);
    }

    public bool RemoveDish()
    {
        if (dishes.Count == 0)
            return false;

        Rigidbody topDish = dishes?.LastOrDefault();

        if (topDish == null)
        {
            return false;
        }

        topDish.isKinematic = false;
        topDish.AddForce(new Vector3(balancingPlatesManager.CurrentArmPosition.x * forceScaler, forceScaler / 2f, balancingPlatesManager.CurrentArmPosition.y * forceScaler), ForceMode.Impulse);
        topDish.transform.SetParent(null);

        dishes.Remove(topDish);
        droppedDishes.Add(topDish);

        SoundManager.Instance.smashPlateAudioSource.pitch = Random.Range(0.9f, 1.1f);
        SoundManager.Instance.smashPlateAudioSource.Play();

        if (dishes.Count > 0)
            return true;
        else
            return false;
    }

    public int DishesHeld() => dishes.Count;
    public int DishesSmashed() => droppedDishes.Count;

    public void RemoveDishes()
    {
        // Leave dropped dishes on the floor :>
        for (int i = dishes.Count - 1; i >= 0; i--)
        {
            Destroy(dishes[i].gameObject);
        }
        dishes.Clear();
    }
}
