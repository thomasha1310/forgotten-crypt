using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private AnimationClip idle;
    [SerializeField] private AnimationClip walk;
    [SerializeField] private AnimationClip jump;
    [SerializeField] private AnimationClip fall;
    [SerializeField] private AnimationClip attack;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerInputManager pim;

    private AnimationState animationState;

    private bool shouldAttack = false;

    private enum AnimationState
    {
        kIdle,
        kJump,
        kFall,
        kWalk,
        kAttack
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        pim = GetComponent<PlayerInputManager>();
    }

    private void PlayAnimation(AnimationClip clip)
    {
        if (animator == null || clip == null) { return; }

        animator.Play(clip.name);
    }

    private void SetAnimationState(AnimationState state)
    {
        if (animationState == state) { return; }

        animationState = state;

        switch (state)
        {
            case AnimationState.kIdle:
                PlayAnimation(idle);
                break;
            case AnimationState.kWalk:
                PlayAnimation(walk);
                break;
            case AnimationState.kJump:
                PlayAnimation(jump);
                break;
            case AnimationState.kFall:
                PlayAnimation(fall);
                break;
            case AnimationState.kAttack:
                PlayAnimation(attack);
                Invoke(nameof(ResetToIdle), attack.length);
                break;
            default:
                break;

        }
    }

    private void Update()
    {
        if (animationState == AnimationState.kAttack) { return; }

        AnimationState newState = AnimationState.kIdle;
        
        if (Mathf.Abs(rb.velocity.x) > 0.01f)
        {
            newState = AnimationState.kWalk;
        }
        if (rb.velocity.y > 0.01f)
        {
            newState = AnimationState.kJump;
        }
        if (rb.velocity.y < -0.01f)
        {
            newState = AnimationState.kFall;
        }
        //if (pim.GetAttackInput())
        //{
        //    shouldAttack = true;
        //}
        if (shouldAttack)
        {
            newState = AnimationState.kAttack;
            shouldAttack = false;
        }
        
        SetAnimationState(newState);
    }

    public void PlayerAttacks()
    {
        Debug.Log("PlayerAnimationController: should attack");
        shouldAttack = true;
        
    }

    private void ResetToIdle()
    {
        SetAnimationState(AnimationState.kIdle);
    }
}
