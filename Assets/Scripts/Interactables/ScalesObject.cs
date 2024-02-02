using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalesObject : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Interact;
    [SerializeField] private ObjectID id;
    private bool pickUp;

    private void Start()
    {
        pickUp = false;
    }
    public void Interact()
    {
        //Player clicks on bottle, key, wallet or handcuffs
        //object goes into "inventory"
        gameObject.SetActive(false);
        pickUp = true;
        //player clicks on left scale-bowl
        //object appears on scale & scale weight adjusts 
    }
}

public enum ObjectID //idk if this is necessary
{
    Keys,
    Wallet,
    Handcuffs,
    Bottle
}
