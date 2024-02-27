using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class GlassShard : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Inspect;
    [SerializeField] GlassShardHUD hud;
    [SerializeField] Sprite sprite;

    public void Interact()
    {
        hud.DisplayShard(sprite);
    }
}
