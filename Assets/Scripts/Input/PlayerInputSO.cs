using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ScriptableObjects/PlayerInputSO", order = 1)]
public class PlayerInputSO : ScriptableObject, IInputProvider
{
    public PlayerInput input { get; private set;}
    public List<ButtonAction> Abilities => abilityActions;
    private List<ButtonAction> abilityActions;

    public ButtonAction OnJump { get { return jumpAction; } }
    public ButtonAction OnSprint { get { return sprintAction; } }

    public ButtonAction OnInteract { get { return interactAction; } }

    public ButtonAction OnExit { get { return exitAction; } }

    private ButtonAction jumpAction, sprintAction, interactAction, exitAction;

    [SerializeField] private float lookSensitivity;

    public void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();
        jumpAction = new ButtonAction(input.Game.Jump);
        sprintAction = new ButtonAction(input.Game.Sprint);
        interactAction = new ButtonAction(input.Game.Interact);
        exitAction = new ButtonAction(input.Game.Exit);
    }

    public InputState GetState()
    {
        InputState state = new InputState(input.Game.Move.ReadValue<Vector2>(), input.Game.Look.ReadValue<Vector2>() * lookSensitivity);
        return state;
    }
}
