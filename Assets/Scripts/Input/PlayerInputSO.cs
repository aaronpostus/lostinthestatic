using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ScriptableObjects/PlayerInputSO", order = 1)]
public class PlayerInputSO : ScriptableObject, IInputProvider, IInputChain
{
    public PlayerInput input { get; private set;}
    public List<ButtonAction> Abilities => abilityActions;
    private List<ButtonAction> abilityActions;

    public ButtonAction OnJump { get { return jumpAction; } }
    public ButtonAction OnSprint { get { return sprintAction; } }
    private ButtonAction jumpAction, sprintAction;

    [SerializeField] private float lookSensitivity;
    private List<IInputModifier> modifiers;

    public void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();

        jumpAction = new ButtonAction(input.Game.Jump);
        sprintAction = new ButtonAction(input.Game.Sprint);
        ButtonAction abilityPrimary = new ButtonAction(input.Game.Ability1);
        ButtonAction abilitySecondary = new ButtonAction(input.Game.Ability2);
        ButtonAction abilityUtility = new ButtonAction(input.Game.Ability3);
        ButtonAction abilitySpecial = new ButtonAction(input.Game.Ability4);
        abilityActions = new List<ButtonAction> { abilityPrimary, abilitySecondary, abilityUtility, abilitySpecial };
    }

    public InputState GetState()
    {
        InputState state = new InputState(input.Game.Move.ReadValue<Vector2>(), input.Game.Look.ReadValue<Vector2>() * lookSensitivity);
        foreach (IInputModifier modifier in modifiers) {
            state = modifier.ModifyInput(state);
        }

        return state;
    }

    public void Register(IInputModifier modifier) { modifiers.Add(modifier); }

    public void Unregister(IInputModifier middleware) { modifiers.Remove(middleware); }

    public void Initialize(List<IInputModifier> chain)
    {
        modifiers = chain;
    }
}
