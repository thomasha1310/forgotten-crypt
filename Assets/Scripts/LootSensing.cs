using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSensing : MonoBehaviour
{
    [SerializeField] private Transform lootSensor;
    [SerializeField] private LayerMask lootLayer;

    [SerializeField] private GameObject chests;


    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (IsTouchingGem() && Input.GetButtonDown("Submit"))
        {
            GameObject gem = GetTouchingGem();
            Debug.Log("took loot " + gem.name);
            chests.SendMessage("TakeGem", gem);
        }
    }

    private bool IsTouchingGem()
    {
        return Physics2D.OverlapCircle(lootSensor.position, 0.2f, lootLayer);
    }

    private GameObject GetTouchingGem()
    {
        Collider2D collider = Physics2D.OverlapCircle(lootSensor.position, 0.2f, lootLayer);
        return collider ? collider.gameObject : null;
    }
}
