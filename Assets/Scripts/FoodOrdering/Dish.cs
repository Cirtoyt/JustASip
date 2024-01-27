using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public List<Mesh> meshes = new List<Mesh>();    
    private MeshFilter meshFilter;
    private bool inUse = false;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomlyGenerateADish()
    {
        meshFilter.mesh = meshes[Random.Range(0, meshes.Count)];
    }

    public void StackFood(int amountToStack)
    {
        //stack food 
        //loop through amount create new objects stack them on top of eachother
        for(int i = 0; i < amountToStack; i++)
        {
            
        }

    }

}
