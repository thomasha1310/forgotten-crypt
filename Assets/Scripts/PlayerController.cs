using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;
    private Rigidbody2D rb = null;
    private Animator animator = null;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float acceleration = 0.2f;
    [SerializeField] private float groundDeceleration = 0.5f;
    [SerializeField] private float airDeceleration = 0.5f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float wallJumpXPower = 8f;
    [SerializeField] private float wallJumpYPower = 16f;
    [SerializeField] private bool shouldHop = true;
    [SerializeField] private float hopRatio = 0.5f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;

    [SerializeField] private Transform groundSensor;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallSensor;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private AnimationClip animIdle;
    [SerializeField] private AnimationClip animWalk;
    [SerializeField] private AnimationClip animJumpUp;
    [SerializeField] private AnimationClip animJumpDown;
    [SerializeField] private AnimationClip animAttack;


    private enum AnimationState
    {
        kDefault,
        kIdle,
        kWalk,
        kJumpUp, kJumpDown,
        kAttack
    }

    private AnimationState animState;

    private void PlayAnimation(AnimationClip clip)
    {
        if (animator == null || clip == null) return;
        Debug.Log("playing " + clip.name);
        animator.Play(clip.name);
    }
    private void SetAnimationState(AnimationState state)
    {
        if (animState == state) return;

        
        animState = state;
        Debug.Log(animState);
        switch (state)
        {
            case AnimationState.kIdle:

                PlayAnimation(animIdle);
                break;
            case AnimationState.kWalk:
                PlayAnimation(animWalk);
                break;
            case AnimationState.kJumpUp:
                PlayAnimation(animJumpUp);
                break;
            case AnimationState.kJumpDown:
                PlayAnimation(animJumpDown);
                break;
            case AnimationState.kAttack:
                PlayAnimation(animAttack);
                Invoke(nameof(ResetToIdle), 0.75f);
                break;
            default:
                break;

        }

    }

    private void ResetToIdle()
    {
        SetAnimationState(AnimationState.kIdle);
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpBufferCounter = 0;
        }

        if (shouldHop && Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * hopRatio);
            coyoteTimeCounter = 0;
        }

        if (wallSensor)
        {
            WallSlide();
            WallJump();
        }

        if (!isWallJumping)
        {
            Flip();
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (animState == AnimationState.kAttack)
        {
            return;
        }
        if (IsGrounded())
        {
            
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Fire");
                SetAnimationState(AnimationState.kAttack);
                return;
            }
            else if (IsZeroish(rb.velocity.x))
            {
                SetAnimationState(AnimationState.kIdle);
                return;
            }
            else
            {
                SetAnimationState(AnimationState.kWalk);
                return;
            }
        }
        else
        {
            if (rb.velocity.y > 0.1f)
            {
                SetAnimationState(AnimationState.kJumpUp);
                return;
            }
            else
            {
                SetAnimationState(AnimationState.kJumpDown);
                return;
            }
        }
    }

    private bool IsZeroish(float v)
    {
        return Mathf.Abs(v) < 0.001f;
    }

    private void FixedUpdate()
    {
        if (animState == AnimationState.kAttack)
        {
            rb.velocity = new Vector2(rb.velocity.x / 4, rb.velocity.y);
        }
        if (isWallJumping)
        {
            return;
        }

        float vx = rb.velocity.x;

        // turning around?
        if (!IsZeroish(horizontal) && (Mathf.Sign(vx) != Mathf.Sign(horizontal)))
        {
            vx = 0;
        }

        float deceleration = IsGrounded() ? groundDeceleration : airDeceleration;

        float r = Mathf.Abs(horizontal) > 0 ? acceleration : deceleration;

        vx = Mathf.Lerp(vx, horizontal * speed, r);
        if (IsZeroish(horizontal) && Mathf.Abs(vx) < 0.5f)
        {
            vx = 0.0f;
        }
        rb.velocity = new Vector2(vx, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundSensor.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallSensor.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpXPower, wallJumpYPower);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}