using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeLootHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeGem(GameObject gem)
    {
        gem.GetComponent<BoxCollider2D>().enabled = false;
        gem.transform.localScale = new Vector3(0, 0, 0);
    }
}
