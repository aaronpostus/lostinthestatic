using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputChain : SerializedMonoBehaviour, IInputProvider
{
    [OdinSerialize] private List<IInputModifier> inputChain;
    [OdinSerialize] private IInputProvider inputProvider;

    public ButtonAction OnJump => inputProvider.OnJump;

    public ButtonAction OnSprint => inputProvider.OnSprint;

    public ButtonAction OnInteract => inputProvider.OnInteract;

    public ButtonAction OnExit => inputProvider.OnExit;

    public InputState GetState()
    {
        InputState input = inputProvider.GetState();
        input.moveDirection = new Vector3(input.moveDirection.x, 0, input.moveDirection.y).normalized;
        for(int i=0; i<inputChain.Count; i++)
        {
            input = inputChain[i].ModifyInput(input);
        }

        return input;
    }
}
