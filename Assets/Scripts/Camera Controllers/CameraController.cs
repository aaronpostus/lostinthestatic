using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
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
            if (value.Equals(CarTarget) && !charMotor.BoundToCar)
            {
                charMotor.BindCar(CarTarget);
                carController.Occupied = true;
            }

            else if (value.Equals(CharTarget) && charMotor.BoundToCar)
            {
                charMotor.UnBindCar();
                carController.Occupied = false;
            }
        } 
    }
    private Transform positionTarget;

    

    public Transform CharTarget, CarTarget;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float lookTargetSlerp;
    [SerializeField] private bool inheritTargetRotation;
    private Camera activeCamera;
    [SerializeField] private Vector2 cameraAngleLimit;
    public bool IsFree => !(IsTransitioning || IsFocused);
    public bool IsFocused => lookTarget != null;
    public bool IsTransitioning;
    private Vector3 targetPosition, localEulers;
    private CharacterMotor charMotor;
    private CarController carController;

    private void Awake()
    {
        activeCamera = GetComponentInChildren<Camera>();
        charMotor = CharTarget.GetComponentInParent<CharacterMotor>();
        carController = CarTarget.GetComponentInParent<CarController>();
        localEulers = transform.localEulerAngles;
        IsTransitioning = false;
        PositionTarget = CharTarget;
    }
   


    void LateUpdate()
    {
        inputState = inputProvider.GetState();
        if(!IsTransitioning) UpdatePosition();
        if (!IsFocused)
        {
            UpdateRotationInput();
        }
        else {
            UpdateRotationTarget();
        }
        
    }

    private void UpdatePosition() {
        targetPosition = PositionTarget.position;
        transform.position = targetPosition;
    }

    private void UpdateRotationInput() {
        Vector3 clampedLookEulers = inputState.lookEulers + localEulers;
        clampedLookEulers.x = Mathf.Clamp(Mathf.DeltaAngle(0, clampedLookEulers.x), cameraAngleLimit.x, cameraAngleLimit.y);
        
        localEulers = clampedLookEulers;
        transform.localEulerAngles = localEulers + (inheritTargetRotation ? PositionTarget.eulerAngles : Vector3.zero);
    }
    private void UpdateRotationTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position, Vector3.up), lookTargetSlerp*Time.deltaTime);
        localEulers = transform.localEulerAngles;
    }

    public InputState ModifyInput(InputState input)
    {
        input.moveDirection = Quaternion.AngleAxis(localEulers.y, Vector3.up) * input.moveDirection;
        input.lookEulers = localEulers;
        return input;
    }
}
