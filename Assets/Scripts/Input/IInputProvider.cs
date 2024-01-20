using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputProvider {

    public ButtonAction OnJump { get; }
    public ButtonAction OnSprint { get; }  

    public List<ButtonAction> Abilities { get; }

    public InputState GetState();
}

public interface IInputChain
{
    public void Initialize(List<IInputModifier> chain);

    public void Register(IInputModifier chain);

    public void Unregister(IInputModifier chain);
}

public interface IInputModifier
{
    public InputState ModifyInput(InputState input);
}

public class ButtonAction {
    public event Action started;
    public event Action ended;

    public ButtonAction(InputAction inputAction) {
        inputAction.started += _ => started?.Invoke();
        inputAction.canceled += _ => ended?.Invoke();
    }
    /*
    public ButtonActions(Action started, Action canceled)
    {
        started += () => OnStarted?.Invoke();
        canceled += () => OnEnded?.Invoke();
    }
    */
    public ButtonAction() {}

    public void InvokeStart() => started?.Invoke();
    public void InvokeEnd() => ended?.Invoke();
}

[Serializable]
public struct InputState
{
    public Vector3 moveDirection;
    public bool shouldLookAtAim;
    public Vector3 aimPoint;
    public Vector3 lookEulers;

    public InputState(Vector2 moveInput, Vector2 lookInput)
    {
        moveDirection = moveInput;
        lookEulers = new Vector3(-lookInput.y, lookInput.x, 0);
        aimPoint = Vector3.zero;
        shouldLookAtAim = false;
    }

    public InputState(Vector3 moveDirection, Vector2 lookInput)
    {
        this.moveDirection = moveDirection;
        lookEulers = new Vector3(lookInput.y, lookInput.x, 0);
        aimPoint = Vector3.zero;
        shouldLookAtAim = false;
    }

    public InputState(Vector3 moveDirection, Vector3 lookEulers)
    {
        this.moveDirection = moveDirection;
        this.lookEulers = lookEulers;
        aimPoint = Vector3.zero;
        shouldLookAtAim = false;
    }
}
