using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class CameraController : SerializedMonoBehaviour, IInputModifier
{
    [Header("Input")]
    [OdinSerialize] private IInputProvider inputProvider;
    [SerializeField] private InputState inputState;

    [Header("Camera Properties")]
    [SerializeField] private Transform target;
    [SerializeField] private bool inheritTargetRotation;
    private Camera activeCamera;
    [SerializeField] private Vector2 cameraAngleLimit;
    private Vector3 targetPosition, localEulers;

    private void Awake()
    {
        activeCamera = GetComponentInChildren<Camera>();
        localEulers = transform.localEulerAngles;
    }
    private void Start()
    {
    }

    void LateUpdate()
    {
        inputState = inputProvider.GetState();
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition() {
        targetPosition = target.position;
        transform.position = targetPosition;
    }

    private void UpdateRotation() {
        Vector3 clampedLookEulers = inputState.lookEulers + localEulers;
        clampedLookEulers.x = Mathf.Clamp(Mathf.DeltaAngle(0, clampedLookEulers.x), cameraAngleLimit.x, cameraAngleLimit.y);
        localEulers = clampedLookEulers;
        transform.localEulerAngles = localEulers + (inheritTargetRotation? target.eulerAngles : Vector3.zero);
    }

    public InputState ModifyInput(InputState input)
    {
        //input.aimPoint = aimPoint;
        input.lookEulers = localEulers;
        return input;
    }

    public void ChangeTarget(Transform target, bool inheritTargetRotation)
    {
        this.target = target;
        
    }
}
