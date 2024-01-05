using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CombatController : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private int maxHealth = 2000;
    private int health;

    [SerializeField] private int defense = 500;
    [SerializeField] private int attack = 1000;
    [SerializeField] private int critRatePercent = 5;
    [SerializeField] private int critDamagePercent = 50;

    [SerializeField] private int combatHealPerSecond = 0;
    [SerializeField] private int idleHealPerSecond = 100;
    private float nextHealTime = 0;




    [SerializeField] private float attackCooldown = 1.0f;
    private bool shouldAttack = false;
    private float nextAttackTime = 0;

    public bool inCombat = false;


    private void Awake()
    {
        health = maxHealth;
    }

    private void Update()
    {
        shouldAttack = inputManager.GetAttackInput();
        HandleHeal();
        HandleAttack();
    }

    private void HandleHeal()
    {
        if (Time.time >= nextHealTime)
        {
            nextHealTime = Time.time + 1;
            if (inCombat)
            {
                health = Mathf.RoundToInt(Mathf.Min(health + combatHealPerSecond, maxHealth));
            }
            else
            {
                health = Mathf.RoundToInt(Mathf.Min(health + idleHealPerSecond, maxHealth));
            }
        }
    }

    private void HandleAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            // attack

            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
