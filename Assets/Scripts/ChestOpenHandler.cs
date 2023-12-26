using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpenHandler : MonoBehaviour
{
    private bool canOpenChest = false;
    private GameObject chest;
    private int i = 1;
    private bool gemGoingUp = true;
    private GameObject chestParent;
    private GameObject chestBottom;
    private GameObject chestTop;
    private GameObject gem;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.transform.GetChild(3).GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(1).gameObject.transform.GetChild(3).GetComponent<BoxCollider2D>().enabled = false;
        //transform.GetChild(2).gameObject.transform.GetChild(3).GetComponent<BoxCollider2D>().enabled = false;
        //transform.GetChild(3).gameObject.transform.GetChild(3).GetComponent<BoxCollider2D>().enabled = false;
        //transform.GetChild(4).gameObject.transform.GetChild(3).GetComponent<BoxCollider2D>().enabled = false;

    }

    void FixedUpdate()
    {
        if (canOpenChest)
        {
            RunOpenChest(chest);
            if (gemGoingUp && i <= 40)
            {;
                gem.transform.localPosition += new Vector3(0, 0.025f, 0);
                i++;
            }
            else if (gemGoingUp && i <= 120)
            {
                gem.transform.localPosition += new Vector3(0, 0.025f, 0) * (120 - i) / 80;
                i++;
            }
            else if (gemGoingUp && i > 120)
            {
                gemGoingUp = false;
                i = 120;
                gem.layer = 9;
                SpriteRenderer renderer = gem.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 2;
            }
            else if (!gemGoingUp && i >= 50)
            {
                gem.transform.localPosition += new Vector3(0, -0.025f, 0) * (120 - i) / 80;
                i--;
            }
            else
            {
                canOpenChest = false;
                i = 1;
                gemGoingUp = true;
                gem.transform.localPosition = new Vector3(0, 0.25f, 0);
                BoxCollider2D collider = gem.GetComponent<BoxCollider2D>();
                collider.enabled = true;
            }
        }
    }

    void OpenChest(GameObject chest)
    {
        this.chest = chest;
        chestParent = chest.transform.parent.gameObject;
        chestBottom = chestParent.transform.GetChild(1).gameObject;
        chestTop = chestParent.transform.GetChild(2).gameObject;
        gem = chestParent.transform.GetChild(3).gameObject;
        canOpenChest = true;
    }

    void RunOpenChest(GameObject chest)
    {
        chestBottom.transform.localScale = new Vector3(1, 1, 1);
        chestTop.transform.localScale = new Vector3(1, 1, 1);
        chest.transform.localScale = new Vector3(0, 0, 0);
    }
}
