using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkKey : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Use;


    public void Interact()
    {
        TrunkController.InvokeOpen();
        Destroy(gameObject);
    }
}
