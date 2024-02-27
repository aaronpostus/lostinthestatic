using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Collider))]
public class ScalesItem : MonoBehaviour, IInteractable
{
    public event Action<ScalesItem, bool> OnPlaced;
    
    public ScalesFlag ItemType;
    public Transform placementTarget;
    private Vector3 basePos, fromPos, toPos;
    private Quaternion baseRot, fromRot, toRot;
    private Collider col;
    [SerializeField] private float placementLength = 0.2f, heightScalar;
    [SerializeField] private AnimationCurve heightCurve;
    private float placementProgress;
    private bool onScale;



    public InteractionType Type => onScale? InteractionType.Remove : InteractionType.Place;

    private void Awake()
    {
        basePos = transform.position;
        baseRot = transform.rotation;
        col = GetComponent<Collider>();
    }

    public void Interact()
    {
        col.enabled = false;
        fromPos = onScale ? placementTarget.position : basePos;
        toPos = !onScale ? placementTarget.position : basePos;
        fromRot = onScale ? placementTarget.rotation : baseRot;
        toRot = !onScale ? placementTarget.rotation : baseRot;
        placementProgress = 0;
        UpdateTicker.Subscribe(IncrementPlacement);
    }

    public void IncrementPlacement() {
        placementProgress += Time.deltaTime / placementLength;
        transform.position = Vector3.Lerp(fromPos, toPos, placementProgress) + Vector3.up*heightScalar * heightCurve.Evaluate(placementProgress);
        transform.rotation = Quaternion.Slerp(fromRot, toRot, placementProgress);
        if (placementProgress >= 1) {
            
            col.enabled = true;
            onScale = !onScale;
            transform.parent = onScale ? placementTarget : null;
            OnPlaced?.Invoke(this, onScale);
            UpdateTicker.Unsubscribe(IncrementPlacement);
        }
    }
}
