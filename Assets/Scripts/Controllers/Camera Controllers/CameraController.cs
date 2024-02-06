using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

public class CameraController : SerializedMonoBehaviour, IInputModifier
{
    [Header("Input Provider")]
    [OdinSerialize] public IInputProvider inputProvider;
    [SerializeField] private InputState inputState;

    public Transform PositionTarget { get { return positionTarget; } 
        set {
            InheritTargetRotation = value == GameManager.Instance.Player.GetTarget() ? true : false;
            positionTarget = value;
        }
    }

    [SerializeField] private Transform positionTarget;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float lookTargetSlerp;
    public bool InheritTargetRotation;
    [SerializeField] private Vector2 cameraAngleLimit;
    public bool IsFree => PositionTarget != null && !IsFocused;
    public bool IsFocused => lookTarget != null;
    public Vector3 LocalEulers;

    private void Awake()
    {
        LocalEulers = transform.localEulerAngles;
    }

    void LateUpdate()
    {
        inputState = inputProvider.GetState();
        if (PositionTarget != null) {
            UpdatePosition();
            if (!IsFocused) UpdateRotationInput();
            else UpdateRotationTarget();
        }
    }

    private void UpdatePosition() {
        transform.position = PositionTarget.position;
    }

    private void UpdateRotationInput() {
        Vector3 clampedLookEulers = inputState.lookEulers + LocalEulers;
        clampedLookEulers.x = Mathf.Clamp(Mathf.DeltaAngle(0, clampedLookEulers.x), cameraAngleLimit.x, cameraAngleLimit.y);

        LocalEulers = clampedLookEulers;
        transform.localEulerAngles = LocalEulers + PositionTarget.eulerAngles;
    }

    private void UpdateRotationTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position, Vector3.up), lookTargetSlerp*Time.deltaTime);
        LocalEulers = transform.localEulerAngles;
    }

    public InputState ModifyInput(InputState input)
    {
        input.moveDirection = Quaternion.AngleAxis(LocalEulers.y, Vector3.up) * input.moveDirection;
        input.lookEulers = LocalEulers;
        return input;
    }
}
