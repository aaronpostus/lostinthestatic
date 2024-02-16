using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class CharacterToCarBinder : MonoBehaviour
{
    private Transform bindTarget;
    private Vector3 offset;
    private void Awake()
    {
        GameManager.TransitionStarted += CheckBind;
        GameManager.TransitionEnded += CheckUnbind;
    }
    private void OnDestroy()
    {
        GameManager.TransitionStarted -= CheckBind;
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
        UpdateTicker.Unsubscribe(KeepRelativeOffset);
    }

    private void KeepRelativeOffset()
    {
        transform.position = bindTarget.TransformPoint(offset);
    }
}
