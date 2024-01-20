using UnityEngine;
using System;

public class UpdateTicker : MonoBehaviour
{
    private static UpdateTicker instance;
    private static event Action OnUpdate;

    public static void Subscribe(Action updateMethod)
    {
        if (!Application.isPlaying) return;
        if (instance == null) instance = new GameObject("[Update Ticker]").AddComponent<UpdateTicker>();
        OnUpdate += updateMethod;
    }

    public static void Unsubscribe(Action updateMethod)
    {
        OnUpdate -= updateMethod;
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void OnDisable()
    {
        OnUpdate = null;
        instance = null;
    }
}
