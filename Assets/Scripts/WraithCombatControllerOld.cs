using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithCombatControllerOld : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float maxSpeed = 6;
    [SerializeField] private float acceleration = 0.2f;
    [SerializeField] private float jumpingPower = 4;

    [SerializeField] private float attackCooldown = 3.0f;
    private float attackCooldownCounter = 0;

    [SerializeField] private GameObject patrolLeft;
    [SerializeField] private GameObject patrolRight;

    [SerializeField] private AnimationClip animIdle;
    [SerializeField] private AnimationClip animAttack;

    [SerializeField] private Transform wallSensor;
    [SerializeField] private LayerMask collisionLayer;

    private int direction = 1;
    private Rigidbody2D rb = null;
    private Animator animator;

    private enum State
    {
        kDefault,
        kPursue,
        kAttack,
        kJump,
        kPatrol
    }

    private State state = State.kDefault;
    private State previousState = State.kDefault;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldownCounter = Mathf.Max(attackCooldownCounter - Time.deltaTime, -1.0f);
        UpdateState();
        UpdateAnimation();
    }

    private void UpdateState()
    {
        previousState = state;
        if (state != State.kDefault)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.localPosition);
        float distanceToPatrol = (Vector3.Distance(transform.localPosition, patrolLeft.transform.localPosition) + Vector3.Distance(transform.localPosition, patrolRight.transform.localPosition)) / 2;
        if (distanceToPatrol >= 8.0)
        {
            state = State.kPatrol;
            return;
        }
        else if (distanceToPlayer <= 4.0)
        {
            if (distanceToPlayer <= 1.5 && attackCooldownCounter < 0)
            {
                attackCooldownCounter = attackCooldown;
                state = State.kAttack;
                return;
            }
            else
            {
                state = State.kPursue;
                return;
            }
        }
        else
        {
            state = State.kPatrol;
            return;
        }
    }

    private void UpdateAnimation()
    {
        if (state == previousState)
        {
            return;
        }
        switch (state)
        {
            case State.kDefault:
                PlayAnimation(animIdle);
                break;
            case State.kAttack:
                PlayAnimation(animAttack);
                Invoke(nameof(ResetToIdle), 0.75f);
                break;
            default:
                PlayAnimation(animIdle);
                Invoke(nameof(ResetToIdle), 1.00f);
                break;
        }
    }

    private void PlayAnimation(AnimationClip animationClip)
    {
        animator.Play(animationClip.name);
    }

    private void ResetToIdle()
    {
        state = State.kDefault;
    }

    private void FixedUpdate()
    {
        if (state == State.kAttack)
        {
            rb.velocity = new Vector2(rb.velocity.x / 4, rb.velocity.y);
        }


        if (state == State.kPursue)
        {
            direction = Mathf.RoundToInt(Mathf.Sign(player.transform.position.x - transform.localPosition.x));
        }

        if (state == State.kPatrol)
        {
            if (transform.localPosition.x < patrolLeft.transform.localPosition.x)
            {
                direction = 1;
            }
            else if (transform.localPosition.x > patrolRight.transform.localPosition.x)
            {
                direction = -1;
            }
        }

        float velocityx;
        if (direction > 0)
        {
            velocityx = Mathf.Min(maxSpeed, rb.velocity.x + acceleration * direction);
        }
        else
        {
            velocityx = Mathf.Max(-maxSpeed, rb.velocity.x + acceleration * direction);
        }

        transform.localScale = new Vector3(2 * direction, 2, 2);

        float velocityy = rb.velocity.y;
        if (Physics2D.OverlapCircle(wallSensor.position, 0.6f, collisionLayer))
        {
            velocityy = jumpingPower;
        }
        

        rb.velocity = new Vector2(velocityx, velocityy);
    }
}
