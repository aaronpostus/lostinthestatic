using FMODUnity;
using System;
using UnityEngine;

public class Glass : MonoBehaviour, IInteractable
{
    [SerializeField] EventReference eventRef;
    public InteractionType Type => InteractionType.Play;
    public event Action<Glass> OnPlay;

    public void Interact()
    {
        //player has clicked on Glass
        OnPlay?.Invoke(this);
        FMODUnity.RuntimeManager.PlayOneShotAttached(eventRef, gameObject);
    }
}