using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputChainRegisterer : SerializedMonoBehaviour
{
    [OdinSerialize] private List<IInputModifier> inputChain;
    [OdinSerialize] private IInputChain target;

    private void Start()
    {
        target.Initialize(inputChain);
    }
}
