using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishGiver : MonoBehaviour
{
    [SerializeField] private Transform waiter;
    [SerializeField] private DishStack dishStack;
    [SerializeField] private List<Rigidbody> possibleDishes = new List<Rigidbody>();
    [SerializeField] private int minDishesToServeInStack = 5;
    [SerializeField] private int maxDishesToServeInStack = 7;
    [SerializeField] private float servingHeight = 1.5f;
    [SerializeField] private float minDishHeight = 0.2f;
    [SerializeField] private float maxDishHeight = 0.3f;

    public bool hasDishesReady = false;

    private List<Rigidbody> spawnedDishes = new List<Rigidbody>();

    public void DishUpNewDishStack()
    {
        SoundManager.Instance.bellDingAudioSource.PlayDelayed(0.5f);

        spawnedDishes.Clear();
        int numDishesToServe = Random.Range(minDishesToServeInStack, maxDishesToServeInStack + 1);
        for (int i = 0; i < numDishesToServe; i++)
        {
            Rigidbody spawnedDish = Instantiate(possibleDishes[Random.Range(0, possibleDishes.Count)]).GetComponent<Rigidbody>();
            spawnedDishes.Add(spawnedDish);
        }

        // Display dishes stacked
        float height = servingHeight;
        for (int i = 0; i < numDishesToServe; i++)
        {
            height += Random.Range(minDishHeight, maxDishHeight);
            spawnedDishes[i].transform.position = new Vector3(0, height, 0);
            spawnedDishes[i].transform.parent = transform;
            spawnedDishes[i].transform.localPosition = new Vector3(0, spawnedDishes[i].transform.localPosition.y, 0);
        }

        hasDishesReady = true;
    }

    public void GiveDishStackToWaiter()
    {
        spawnedDishes.ForEach((d) => d.transform.position += Vector3.up * (dishStack.transform.position.y - servingHeight - 0.15f));
        dishStack.AddDishes(spawnedDishes);

        SoundManager.Instance.dishesAudioSource.Play();

        hasDishesReady = false;
    }
}
