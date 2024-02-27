using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class GlassShard : MonoBehaviour, IInteractable
{
    public InteractionType Type => InteractionType.Inspect;
    [SerializeField] GlassShardHUD hud;
    [SerializeField] Sprite sprite;
    [SerializeField] bool isHUD;
    public void Interact()
    {
        if (isHUD) {
            hud.DisplayMap();
            return;
        }
        hud.DisplayShard(sprite);
    }
}
