using UnityEngine;
using System.Collections;

public abstract class InputManager : MonoBehaviour
{
    public abstract float GetHorizontalInput();
    public abstract bool GetJumpInput();
    public abstract bool GetAttackInput();
}
