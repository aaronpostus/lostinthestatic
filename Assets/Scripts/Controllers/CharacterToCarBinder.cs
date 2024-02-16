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
        GameManager.TransitionStarted += HandleStateChange;
    }
    private void OnDestroy()
    {
        GameManager.TransitionStarted -= HandleStateChange;
    }

    private void HandleStateChange(PlayerState targetState) {
        if (targetState == PlayerState.InCar) BindCar();
        else UnBindCar();
    }


    public void BindCar()
    {
        bindTarget = GameManager.Instance.Car.transform;
        offset = bindTarget.InverseTransformPoint(transform.position);
        gameObject.SetActive(false);
        UpdateTicker.Subscribe(KeepRelativeOffset);
    }

    public void UnBindCar()
    {
        gameObject.SetActive(true);
        UpdateTicker.Unsubscribe(KeepRelativeOffset);
    }

    private void KeepRelativeOffset()
    {
        transform.position = bindTarget.TransformPoint(offset);
    }
}
