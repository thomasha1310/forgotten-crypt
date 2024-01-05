using UnityEngine;
using System.Collections;

public abstract class InputManager : MonoBehaviour
{
    public abstract float getHorizontalInput();
    public abstract bool getJumpInput();
}
