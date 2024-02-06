using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Play;
    public event Action<Glass> OnPlay;

    public void Interact()
    {
        //player has clicked on Glass
        OnPlay?.Invoke(this);
    }
}