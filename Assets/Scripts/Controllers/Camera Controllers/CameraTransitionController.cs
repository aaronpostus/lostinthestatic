using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class CameraTransitionController : MonoBehaviour
{

    [SerializeField] private float transitionTime = 1;
    [SerializeField] private float transitionTimer;
    [SerializeField] private AnimationCurve transitionCurve;

    private CameraController cc;
    private Transform fromTarget, toTarget;
    private Transform playerTarget;
    private Transform carTarget;
    private Vector3 startEulers, endEulers;
    private void Start()
    {
        cc = GameManager.Instance.Camera;
        playerTarget = GameManager.Instance.Player.GetTarget();
        carTarget = GameManager.Instance.Car.GetTarget();
        cc.PositionTarget = GameManager.Instance.ActiveState == PlayerState.OnFoot ? playerTarget : carTarget;
    }

    private void OnEnable()
    {
        CarHandle.OnTryTransition += TransitionTarget;
    }

    private void OnDisable()
    {
        CarHandle.OnTryTransition -= TransitionTarget;
    }

    public void TransitionTarget(PlayerState targetState)
    {
        if (targetState == GameManager.Instance.ActiveState) return;
        GameManager.Instance.TargetState = targetState;
        fromTarget = targetState == PlayerState.OnFoot ? carTarget : playerTarget;
        toTarget = targetState == PlayerState.InCar ? carTarget : playerTarget;
        cc.PositionTarget = null;
        transitionTimer = 0;
        startEulers = fromTarget.eulerAngles;
        endEulers = toTarget.eulerAngles;
        UpdateTicker.Subscribe(IncrementTransition);
    }

    private void IncrementTransition()
    {
        transitionTimer += Time.deltaTime;
        transform.position = Vector3.Lerp(fromTarget.position, toTarget.position, transitionCurve.Evaluate(transitionTimer / transitionTime));
        transform.eulerAngles = Vector3.Slerp(fromTarget.position, toTarget.position, transitionCurve.Evaluate(transitionTimer / transitionTime));
        if (transitionTimer > transitionTime)
        {
            cc.PositionTarget = toTarget;
            GameManager.Instance.ActiveState = GameManager.Instance.TargetState;
            UpdateTicker.Unsubscribe(IncrementTransition);
        }
    }
}
