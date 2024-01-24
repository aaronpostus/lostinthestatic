using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public InteractionType Type { get; }
    void Interact();
}

public enum InteractionType {
    Interact,
    Inspect,
    Open,
    Close,
}