using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class CameraController : SerializedMonoBehaviour, IInputModifier
{
    [Header("Input")]
    [OdinSerialize] private IInputProvider inputProvider;
    [SerializeField] private InputState playerInputState;

    [Header("Camera Properties")]
    [SerializeField] private Transform target;
    private Camera activeCamera;
    [SerializeField] private Vector2 cameraAngleLimit;
    private Vector3 targetPosition;

    private void Awake()
    {
        activeCamera = GetComponentInChildren<Camera>();
    }

    void LateUpdate()
    {
        playerInputState = inputProvider.GetState();
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition() {
        targetPosition = target.position;
        transform.position = targetPosition;
    }


    private void UpdateRotation() {
        Vector3 clampedLookEulers = playerInputState.lookEulers;
        clampedLookEulers.x = Mathf.Clamp(Mathf.DeltaAngle(0, clampedLookEulers.x), cameraAngleLimit.x,  cameraAngleLimit.y);
        transform.localEulerAngles = clampedLookEulers;
    }

    public InputState ModifyInput(InputState input)
    {
        //input.aimPoint = aimPoint;
        input.lookEulers = input.lookEulers + transform.localEulerAngles;
        input.moveDirection = Quaternion.Euler(0,transform.localEulerAngles.y,0) * new Vector3 (input.moveDirection.x, 0, input.moveDirection.y);
        return input;
    }
}
