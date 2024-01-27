using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{

    [SerializeField]float scale;
    [SerializeField] private GameObject[] splashPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        if(splashPrefab == null)
        {
            Debug.LogError("ERROR no splat tiles");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //test code(will delete later)
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            int i = 0;
            CreateSplash(splashPrefab[i]);
            i++;
            if(i == splashPrefab.Length)
            {
                i = 0;
            }
        }
    }

    void CreateSplash(GameObject prefab)
    {
        Vector3 splashPos = transform.position;
        Vector3 vector3 = splashPos;
        
        vector3.y = 0.1f;
        prefab.transform.position = vector3;

        prefab.transform.localScale = new Vector3(scale, scale, scale);
        Instantiate(prefab);
    }
}

// on collison with floor
// then -- Create splash on floor