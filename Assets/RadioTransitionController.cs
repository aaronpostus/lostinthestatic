using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RadioTransitionController : MonoBehaviour
{
    [SerializeField] private AnimationCurve transitionCurve;

    private Transform fromTarget, toTarget;
    private Transform playerTarget;
    private Transform carTarget;
    private FixedJoint joint;
    private Rigidbody rb;
    private void Start()
    {
        playerTarget = GameManager.Instance.Player.GetRadioTarget();
        carTarget = GameManager.Instance.Car.GetRadioTarget();
        UpdateTargets(GameManager.Instance.ActiveState);
        BindToTarget(GameManager.Instance.ActiveState);
    }

    private void OnEnable()
    {
        GameManager.TransitionStarted += StartTransition;
        GameManager.TransitionEnded += EndTransition;
    }

    private void OnDisable()
    {
        GameManager.TransitionStarted -= StartTransition;
        GameManager.TransitionEnded -= EndTransition;
    }

    private void StartTransition(PlayerState targetState)
    {
        if(targetState == PlayerState.OnFoot) Destroy(joint);
        transform.SetParent(null);
        UpdateTargets(GameManager.Instance.TargetState);
        UpdateTicker.Subscribe(Transition);
    }

    private void EndTransition(PlayerState activeState)
    {
        UpdateTicker.Unsubscribe(Transition);
        BindToTarget(activeState);
    }

    private void Transition()
    {
        float transitionValue = transitionCurve.Evaluate(GameManager.Instance.TransitionProgress);
        transform.position = Vector3.Lerp(fromTarget.position, toTarget.position, transitionValue);
        transform.rotation = Quaternion.Slerp(fromTarget.rotation, toTarget.rotation, transitionValue);
    }

    private void UpdateTargets(PlayerState targetState) {
        fromTarget = targetState == PlayerState.OnFoot ? carTarget : playerTarget;
        toTarget = targetState == PlayerState.OnFoot ? playerTarget : carTarget;
    }

    private void BindToTarget(PlayerState activeState) {
        transform.position = toTarget.position;
        transform.rotation = toTarget.rotation;
        if (activeState == PlayerState.OnFoot) {
            if(rb!= null) Destroy(rb);
            transform.SetParent(playerTarget, true);
        }else {
            rb=this.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            joint = this.AddComponent<FixedJoint>();
            joint.connectedBody = GameManager.Instance.Car.transform.GetComponent<Rigidbody>();
        }
    }
}
