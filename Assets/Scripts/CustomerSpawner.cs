using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customer;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        GameObject newCustomer = Instantiate(customer, spawnPoint);
        newCustomer.transform.localPosition = Vector3.zero;

        Transform skins = newCustomer.transform.Find("Geometry");

        int skinIndex = Random.Range(0, skins.childCount);
        GameObject targetSkin = skins.GetChild(skinIndex).gameObject;
        targetSkin.SetActive(true);

    }
}
