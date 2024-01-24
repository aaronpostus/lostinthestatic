using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectableItem : MonoBehaviour, IInteractable
{
    public InteractionType Type => throw new System.NotImplementedException();

    public void Interact()
    {
        
    }
}
