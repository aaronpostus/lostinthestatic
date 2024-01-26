using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEditorInternal;
using UnityEngine;
using static System.TimeZoneInfo;


[RequireComponent(typeof(CameraController))]
public class CameraInteractionController : MonoBehaviour
{
    private CameraController cc;

    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float interactionDistance;
    [SerializeField] private Vector2 screenPivot = new Vector2(0.5f,0.5f);
    private Camera c;
    [SerializeField] private float transitionTime;
    [SerializeField] private AnimationCurve transitionCurve;
    [SerializeField] private float transitionTimer;
    private Vector3 oldTargetPosition;
    private Transform interactTarget;

    private void OnEnable()
    {
        cc = GetComponent<CameraController>();
        c = Camera.main;
        cc.inputProvider.OnInteract.started += TryInteract;
    }


    private void OnDisable()
    {
        cc.inputProvider.OnInteract.started -= TryInteract;
    }

    void Update()
    {
        interactTarget = null;
        if (!(cc.IsFree && Physics.Raycast(c.ViewportPointToRay(screenPivot), out RaycastHit hit, interactionDistance, interactMask))) return;
        interactTarget = hit.collider.transform;
        Debug.Log(interactTarget.name);
    }

    private void TryInteract()
    {
        
        if (interactTarget == null) return;

        if (interactTarget.TryGetComponent(out IInteractable interactable)) interactable.Interact();
        
        if (interactTarget.TryGetComponent(out InspectableItem item)) InteractInspectable(item);
        else if (interactTarget.TryGetComponent(out Monument mon)) InteractMonument(mon);
        else if (interactTarget.TryGetComponent(out CarHandle carHandle)) InteractCarHandle(carHandle);
    }

    private void InteractMonument(Monument mon) {}
    private void InteractInspectable(InspectableItem item) {}

    private void InteractCarHandle(CarHandle carHandle) {
        oldTargetPosition = cc.PositionTarget.position;
        cc.PositionTarget = carHandle.IsEnter ? cc.CarTarget : cc.CharTarget;
        cc.IsTransitioning = true;
        transitionTimer = 0;
        UpdateTicker.Subscribe(TransitionCameraTarget);
    }

    private void TransitionCameraTarget() {
        transitionTimer += Time.deltaTime;
        transform.position = Vector3.Lerp(oldTargetPosition, cc.PositionTarget.position, transitionCurve.Evaluate(transitionTimer / transitionTime));

        if (transitionTimer > transitionTime) {
            UpdateTicker.Unsubscribe(TransitionCameraTarget);
            cc.IsTransitioning = false;
        }
    }
}
