using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;


[RequireComponent(typeof(CameraController))]
public class CameraInteractionController : MonoBehaviour
{
    private CameraController cc;

    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float interactionDistance;
    [SerializeField] private Vector2 screenPivot;
    private Camera c;

    void Start()
    {
        cc = GetComponent<CameraController>();
        c = Camera.main;
    }

    void Update()
    {
        if (!Physics.Raycast(c.ViewportPointToRay(screenPivot), out RaycastHit hit, interactionDistance, interactMask)) return;
        if (hit.collider.TryGetComponent(out IInteractable interactable)) interactable.Interact();
        if (hit.collider.TryGetComponent(out InspectableItem item)) InteractInspectable(item);
        else if (hit.collider.TryGetComponent(out Monument mon)) InteractMonument(mon);
    }

    private void InteractMonument(Monument mon) {}
    private void InteractInspectable(InspectableItem item) {}

}
