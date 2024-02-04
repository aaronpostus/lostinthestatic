using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandle : MonoBehaviour, IInteractable
{
    public static event Action<PlayerState> OnTryTransition;
    [SerializeField] private PlayerState toState;

    public InteractionType Type => InteractionType.Use;

    public void Interact() => OnTryTransition?.Invoke(toState);
}
