using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class CharacterToCarBinder : MonoBehaviour
{
    private Transform bindTarget;
    private Vector3 offset;
    [SerializeField] private float minExitDistance=1f;
    [SerializeField] private LayerMask exitMask;
    private void Awake()
    {
        GameManager.TransitionStarted += CheckBind;
        GameManager.TransitionStarted += CheckPlayerPosition;
        GameManager.TransitionEnded += CheckUnbind;
    }
    private void OnDestroy()
    {
        GameManager.TransitionStarted -= CheckBind;
        GameManager.TransitionStarted -= CheckPlayerPosition;
        GameManager.TransitionEnded -= CheckUnbind;
    }

    public void CheckBind(PlayerState targetState)
    {
        if (targetState != PlayerState.InCar) return;
        bindTarget = GameManager.Instance.Car.transform;
        offset = bindTarget.InverseTransformPoint(transform.position);
        UpdateTicker.Subscribe(KeepRelativeOffset);
        gameObject.SetActive(false);
    }

    public void CheckUnbind(PlayerState activeState)
    {
        if (activeState != PlayerState.OnFoot) return;
        gameObject.SetActive(true);
        
    }

    public void CheckPlayerPosition(PlayerState targetState) {
        if (targetState != PlayerState.OnFoot) return;
        UpdateTicker.Unsubscribe(KeepRelativeOffset);
        Transform origin = GameManager.Instance.Car.ExitPosition;
        if (!Physics.CapsuleCast(origin.position+Vector3.up, origin.position-Vector3.up, 0.5f, -origin.right*minExitDistance, exitMask)) {
            transform.position = origin.position - origin.right*minExitDistance;
        } else if (!Physics.CapsuleCast(origin.position + Vector3.up, origin.position - Vector3.up, 0.5f, origin.right*minExitDistance, exitMask)) {
            transform.position = origin.position + origin.right * minExitDistance;
        } else {
            transform.position = GameManager.Instance.Car.transform.position + Vector3.up * 3f;
        }

    }


    private void KeepRelativeOffset()
    {
        transform.position = bindTarget.TransformPoint(offset);
    }
}
