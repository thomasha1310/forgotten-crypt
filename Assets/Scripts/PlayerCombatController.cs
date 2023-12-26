using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private GameObject healingRelic;
    [SerializeField] private GameObject protectionRelic;
    [SerializeField] private GameObject strengthRelic;
    [SerializeField] private GameObject wisdomRelic;
    [SerializeField] private GameObject foresightRelic;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject vignette;

    private bool collectedHealingRelic;
    private bool collectedProtectionRelic;
    private bool collectedStrengthRelic;
    private bool collectedWisdomRelic;
    private bool collectedForesightRelic;

    private int health = 2000;
    private int defense = 500;
    private int attack = 1000;
    private int critRatePercent = 5;
    private int critDamagePercent = 50;
    private int healPerFrame = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healingRelic.transform.localScale.Equals(new Vector3(0, 0, 0)) && !collectedHealingRelic) {
            collectedHealingRelic = true;
            healPerFrame = 2;
        }
        if (protectionRelic.transform.localScale.Equals(new Vector3(0, 0, 0)) && !collectedProtectionRelic)
        {
            collectedProtectionRelic = true;
            defense += 250;
        }
        if (strengthRelic.transform.localScale.Equals(new Vector3(0, 0, 0)) && !collectedStrengthRelic)
        {
            collectedStrengthRelic = true;
            attack += 500;
            critDamagePercent += 25;
            critRatePercent += 5;
        }
        if (wisdomRelic.transform.localScale.Equals(new Vector3(0, 0, 0)) && !collectedWisdomRelic)
        {
            collectedWisdomRelic = true;
        }
        if (foresightRelic.transform.localScale.Equals(new Vector3(0, 0, 0)) && !collectedForesightRelic)
        {
            collectedForesightRelic = true;
            critDamagePercent += 25;
            critRatePercent += 15;
        }


    }

    private void FixedUpdate()
    {
        if (collectedWisdomRelic && mainCamera.GetComponent<Camera>().orthographicSize < 5)
        {
            mainCamera.GetComponent<Camera>().orthographicSize += 0.1f;
            vignette.transform.localScale = new Vector3(vignette.transform.localScale.x + 0.025f, vignette.transform.localScale.y + 0.025f, vignette.transform.localScale.z);
            if (mainCamera.GetComponent<Camera>().orthographicSize - 5 > -0.05f)
            {
                mainCamera.GetComponent<Camera>().orthographicSize = 5;
            }
        }
    }
}
