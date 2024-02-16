using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class CameraTransitionController : MonoBehaviour
{
    [SerializeField] private AnimationCurve transitionCurve;

    private CameraController cc;
    private Transform fromTarget, toTarget;
    private Transform playerTarget;
    private Transform carTarget;
    private Vector3 cameraEulers;

    private void Start()
    {
        cc = GameManager.Instance.Camera;
        playerTarget = GameManager.Instance.Player.GetTarget();
        carTarget = GameManager.Instance.Car.GetTarget();
        cc.PositionTarget = GameManager.Instance.ActiveState == PlayerState.OnFoot ? playerTarget : carTarget;
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

    private void StartTransition(PlayerState targetState) {
        fromTarget = targetState == PlayerState.OnFoot ? carTarget : playerTarget;
        toTarget = targetState == PlayerState.InCar ? carTarget : playerTarget;
        cc.PositionTarget = null;
        cameraEulers = cc.LocalEulers;
        UpdateTicker.Subscribe(Transition);
    }

    private void EndTransition(PlayerState activeState) {
        cc.PositionTarget = toTarget;
        UpdateTicker.Unsubscribe(Transition);
    }

    private void Transition()
    {
        float transitionValue = transitionCurve.Evaluate(GameManager.Instance.TransitionProgress);
        transform.position = Vector3.Lerp(fromTarget.position, toTarget.position, transitionValue);
        transform.rotation = Quaternion.Slerp(Quaternion.Euler(cameraEulers + fromTarget.eulerAngles), Quaternion.Euler(cameraEulers + toTarget.eulerAngles), transitionValue);
    }
}
