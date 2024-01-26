using UnityEngine;

[System.Serializable]
public class Wheel
{
    public Transform transform;
    public bool IsPowered, IsSteerable;
    public AnimationCurve PacejkaCurve = new AnimationCurve();
    public float GripFactor;
}
