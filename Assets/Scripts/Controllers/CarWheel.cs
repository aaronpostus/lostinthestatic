using UnityEngine;

[System.Serializable]
public class Wheel
{
    public Transform transform;
    public bool IsPowered, IsSteerable;
    public AnimationCurve gripCurve = new AnimationCurve();
    public float gripFactor, mass;
}
