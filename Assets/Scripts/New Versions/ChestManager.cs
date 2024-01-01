using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private GameObject chest;

    private GameObject[] chests;
    [SerializeField] private int numChests;
    [SerializeField] private string[] names;
    [SerializeField] private Vector2[] positions;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numChests; i++)
        {
            chests[i] = Instantiate(chest, positions[i], Quaternion.Euler(Vector3.zero));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}