using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrdering : MonoBehaviour
{
    public List<GameObject> generatedDishs = new List<GameObject>();
    public List<Dish> OrderQueue = new List<Dish>();


    // Start is called before the first frame update
    void Start()
    {
        GenerateNewOrder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public void GenerateNewOrder()
    {
        //randomly select dish

        generatedDishs[0].GetComponent<Dish>().RandomlyGenerateADish();
        
        //queue up on order list
        OrderQueue.Add(generatedDishs[0].GetComponent<Dish>());

    }
}
