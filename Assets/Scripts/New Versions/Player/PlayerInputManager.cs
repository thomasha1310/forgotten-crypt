using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : InputManager
{
    private float horizontalInput;
    private bool shouldJump = false;

    private float jumpButtonDownDuration = 0;
    private float stickyKeyDuration = 0.2f;
    private bool stickyKeyOverride = false;

    private bool wasJumping = false;

    public override float getHorizontalInput()
    {
        return horizontalInput;
    }

    public override bool getJumpInput()
    {
        if (stickyKeyOverride)
        {
            return false;
        }
        return shouldJump;
    }

    private void Update()
    {
        wasJumping = shouldJump;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        shouldJump = Input.GetAxisRaw("Vertical") > 0.7f;

        if (shouldJump && !wasJumping)
        {
            jumpButtonDownDuration = 0f;
        }

        if (wasJumping && shouldJump)
        {
            jumpButtonDownDuration += Time.deltaTime;
        }

        if (jumpButtonDownDuration > stickyKeyDuration)
        {
            stickyKeyOverride = true;
        }

        if (!shouldJump)
        {
            stickyKeyOverride = false;
        }
    }


}
