using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField] private int health = 2000;
    [SerializeField] private int defense = 500;
    [SerializeField] private int attack = 1000;
    [SerializeField] private int critRatePercent = 5;
    [SerializeField] private int critDamagePercent = 50;
    [SerializeField] private int combatHealPerSecond = 0;
    [SerializeField] private int idleHealPerSecond = 100;


}
