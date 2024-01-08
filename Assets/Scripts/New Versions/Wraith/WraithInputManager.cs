using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithInputManager : InputManager
{
    public override float GetHorizontalInput()
    {
        return 0;
    }

    public override bool GetJumpInput()
    {
        return false;
    }

    public override bool GetAttackInput()
    {
        return false;
    }

    void Update()
    {
        
    }
}
