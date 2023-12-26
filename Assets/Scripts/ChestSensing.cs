using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSensing : MonoBehaviour
{
    [SerializeField] private Transform chestSensor;
    [SerializeField] private LayerMask chestLayer;

    [SerializeField] private GameObject chests;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (IsTouchingChest() && Input.GetButtonDown("Submit"))
        {
            GameObject chest = GetTouchingChest();
            Debug.Log("opened chest " + chest.name);
            chests.SendMessage("OpenChest", chest);
        }
    }

    private bool IsTouchingChest()
    {
        return Physics2D.OverlapCircle(chestSensor.position, 0.2f, chestLayer);
    }

    private GameObject GetTouchingChest()
    {
        Collider2D collider = Physics2D.OverlapCircle(chestSensor.position, 0.2f, chestLayer);
        return collider ? collider.gameObject : null;
    }
}
