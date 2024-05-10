using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Pixelplacement;

public class TimerInput : Singleton<TimerInput>
{
    public event System.Action onInteraction;
    public void OnFire(InputValue value)
    {
        if(value.isPressed)
        {
            if(onInteraction != null)
            {
                onInteraction();
            }
        }
    }
}
