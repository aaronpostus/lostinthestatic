using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    //public event Action OnInteract;
    public InteractionType Type { get; }
    void Interact();
}

public enum InteractionType {
    Interact,
    Inspect,
    Open,
    Close,
    Play
}