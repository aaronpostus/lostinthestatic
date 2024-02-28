using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkController : MonoBehaviour
{
    [SerializeField] float openDuration = 1f;
    private float timer;
    private static event Action Open;
    private bool opened;
    public EventReference trunkSound;

    public static void InvokeOpen() {
        Open?.Invoke();
    }

    private void OnEnable()
    {
        Open += OpenTrunk;
    }
    private void OnDisable()
    {
        Open -= OpenTrunk;
    }
    private void OpenTrunk() {
        if (opened) return;
        opened = true;
        RuntimeManager.PlayOneShot(trunkSound, transform.position);
        UpdateTicker.Subscribe(IncrementTimer);
    }

    private void IncrementTimer() {
        timer += Time.deltaTime;
        float prog = Mathf.Clamp01(timer / openDuration);
        transform.localRotation = Quaternion.Euler(Mathf.Lerp(0,60, prog), 0, -90);
        if (timer > openDuration) {
            UpdateTicker.Unsubscribe(IncrementTimer);
        }
    }

}
