using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class CameraController : SerializedMonoBehaviour, IInputModifier
{
    [Header("Input")]
    [OdinSerialize] public IInputProvider inputProvider;
    [SerializeField] private InputState inputState;

    //[Header("Camera Properties")]
    public Transform PositionTarget { 
        get { 
            return positionTarget;
        } 
        set { 
            positionTarget = value;
            inheritTargetRotation = value.Equals(CharTarget) ? false : true;
        } 
    }
    private Transform positionTarget;

    

    public Transform CharTarget, CarTarget;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private bool inheritTargetRotation;
    private Camera activeCamera;
    [SerializeField] private Vector2 cameraAngleLimit;
    public bool IsFree => !(IsTransitioning || IsFocused);
    public bool IsTransitioning, IsFocused;
    private Vector3 targetPosition, localEulers;

    private void Awake()
    {
        activeCamera = GetComponentInChildren<Camera>();
        PositionTarget = CharTarget;
        localEulers = transform.localEulerAngles;
        IsTransitioning = false;
        
    }
   


    void LateUpdate()
    {
        inputState = inputProvider.GetState();
        if(!IsTransitioning) UpdatePosition();
        if(!IsFocused) UpdateRotation();
    }

    private void UpdatePosition() {
        targetPosition = PositionTarget.position;
        transform.position = targetPosition;
    }

    private void UpdateRotation() {
        Vector3 clampedLookEulers = inputState.lookEulers + localEulers;
        clampedLookEulers.x = Mathf.Clamp(Mathf.DeltaAngle(0, clampedLookEulers.x), cameraAngleLimit.x, cameraAngleLimit.y);
        
        localEulers = clampedLookEulers;
        transform.localEulerAngles = localEulers + (inheritTargetRotation ? PositionTarget.eulerAngles : Vector3.zero);
        
    }

    public InputState ModifyInput(InputState input)
    {
        //input.aimPoint = aimPoint;
        input.lookEulers = localEulers;
        return input;
    }
}
