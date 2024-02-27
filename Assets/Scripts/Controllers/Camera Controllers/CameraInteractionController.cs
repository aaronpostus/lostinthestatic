using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class CameraInteractionController : MonoBehaviour
{
    private CameraController cc;

    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float interactionDistance;
    [SerializeField] private Vector2 screenPivot = new Vector2(0.5f, 0.5f);
    [SerializeField] private StringReference interactionString;

    private Camera c;
    private Transform interactTarget;
    private Outline lastInteractedOutline;

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

    void FixedUpdate()
    {
        interactTarget = null;
        interactionString.Value = "";

        if (!cc.IsFree || !Physics.Raycast(c.ViewportPointToRay(screenPivot), out RaycastHit hit, interactionDistance, interactMask))
        {
            if (lastInteractedOutline != null)
            {
                Destroy(lastInteractedOutline);
                lastInteractedOutline = null;
            }
            return;
        }

        interactTarget = hit.collider.transform;
        if (interactTarget.TryGetComponent(out IInteractable interactableTarget))
        {
            interactionString.Value = string.Format("[E] to {0}", interactableTarget.Type.ToString());

            if (lastInteractedOutline == null || lastInteractedOutline.transform != interactTarget)
            {
                if (lastInteractedOutline != null)
                {
                    Destroy(lastInteractedOutline);
                }
                lastInteractedOutline = interactTarget.gameObject.AddComponent<Outline>();
            }
        }
        else
        {
            if (lastInteractedOutline != null)
            {
                Destroy(lastInteractedOutline);
                lastInteractedOutline = null;
            }
        }
    }

    private void TryInteract()
    {
        if (interactTarget == null) return;
        IInteractable interactable = interactTarget.GetComponent<IInteractable>();
        if (interactable == null) return;
        interactable.Interact();
    }
}