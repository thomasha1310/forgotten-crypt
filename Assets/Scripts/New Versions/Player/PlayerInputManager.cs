using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : InputManager
{
    private float horizontalInput;
    private bool shouldJump = false;
    private bool shouldAttack = false;

    // variables used for sticky jump key
    private float jumpButtonDownDuration = 0;
    private readonly float stickyJumpDuration = 0.2f;
    private bool stickyJumpOverride = false;
    private bool wasJumping = false;

    

    public override float GetHorizontalInput()
    {
        return horizontalInput;
    }

    public override bool GetJumpInput()
    {
        if (stickyJumpOverride)
        {
            return false;
        }
        return shouldJump;
    }

    public override bool GetAttackInput()
    {
        return shouldAttack;
    }

    private void Update()
    {
        wasJumping = shouldJump;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        shouldJump = Input.GetAxisRaw("Vertical") > 0.7f;
        if (!shouldAttack && Input.GetButtonDown("Fire1"))
        {
            shouldAttack = true;
        }
        else if (shouldAttack && Input.GetButtonUp("Fire1"))
        {
            shouldAttack = false;
        }
        Debug.Log(shouldAttack);
        HandleStickyKeys();
    }

    private void HandleStickyKeys()
    {
        HandleStickyJump();
    }

    private void HandleStickyJump()
    {
        if (shouldJump && !wasJumping)
        {
            jumpButtonDownDuration = 0f;
        }

        if (wasJumping && shouldJump)
        {
            jumpButtonDownDuration += Time.deltaTime;
        }

        if (jumpButtonDownDuration > stickyJumpDuration)
        {
            stickyJumpOverride = true;
        }

        if (!shouldJump)
        {
            stickyJumpOverride = false;
        }
    }

}
