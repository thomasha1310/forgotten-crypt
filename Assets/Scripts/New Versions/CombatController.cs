using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InputManager))]
public class CombatController : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private Transform attackSensor;
    [SerializeField] private float attackRadius;

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

    public UnityEvent<int, Collider2D[]> sendAttack;


    private bool inCombat = false;


    private void Awake()
    {
        health = maxHealth;
        sendAttack = new UnityEvent<int, Collider2D[]>();
        inputManager = GetComponent<InputManager>();
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
        if (!shouldAttack)
        {
            return;
        }
        if (Time.time >= nextAttackTime)
        {
            Debug.Log("CombatController: attack");

            Collider2D[] targetsHit = Physics2D.OverlapCircleAll(attackSensor.position, attackRadius);

            int damageDone = attack;
            if (RollForCrit())
            {
                attack += (int)(attack * critDamagePercent / 100f);
            }
            sendAttack.Invoke(damageDone, targetsHit);

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void HandleDeath()
    {
        Debug.Log(name + "died");
    }

    private bool RollForCrit()
    {
        return Random.Range(1, 100) <= critRatePercent;
    }

    public void ReceiveAttack(int damageDone, Collider2D[] targetsHit)
    {
        Debug.Log(name + " received attack");
        Collider2D collider = GetComponent<Collider2D>();
        if (targetsHit.Contains(collider))
        {
            float defenseMultiplier = 1f - defense / (defense + 1000f);
            damageDone = Mathf.RoundToInt(damageDone * defenseMultiplier);
            health -= damageDone;

            if (health < 0)
            {
                HandleDeath();
            }
        }
    }
}
