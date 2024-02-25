using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private bool isWaitingForInput = true;
    public static bool isUsingRadio = false;
    [SerializeField] EventReference refere, radioClick;
    [SerializeField] StaticController controller;
    [SerializeField] ParticleSystem system;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite enabled, disabled;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isWaitingForInput = false;
            EventInstance startSound = FMODUnity.RuntimeManager.CreateInstance(refere);
            startSound.start();
            controller.MainMenuFadeToWhite();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            isUsingRadio = !isUsingRadio;

            var shape = system.shape;
            if (isUsingRadio)
            {
                shape.texture = enabled.texture;
                spriteRenderer.sprite = enabled;
            }
            else {
                shape.texture = disabled.texture;
                spriteRenderer.sprite = disabled;
            }

            EventInstance startSound = FMODUnity.RuntimeManager.CreateInstance(radioClick);
            startSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
            startSound.start();
        }
    }
}