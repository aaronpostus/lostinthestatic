using UnityEngine;


[RequireComponent(typeof(CameraController))]
public class CameraInteractionController : MonoBehaviour
{
    private CameraController cc;

    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float interactionDistance;
    [SerializeField] private Vector2 screenPivot = new Vector2(0.5f,0.5f);
    [SerializeField] private StringReference interactionString;
    private Camera c;
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
        //if (interactionString != null) interactionString.Value = interactTarget != null ? "" : ;
        if (!cc.IsFree || !Physics.Raycast(c.ViewportPointToRay(screenPivot), out RaycastHit hit, interactionDistance, interactMask)) return;

        interactTarget = hit.collider.transform;
    }

    private void TryInteract()
    {
        if (interactTarget == null) return;
        IInteractable interactable = interactTarget.GetComponent<IInteractable>();
        if (interactable == null) return;
        interactable.Interact();
    }
}
