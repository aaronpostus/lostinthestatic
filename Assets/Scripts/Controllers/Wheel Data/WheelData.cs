using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wheel Data", menuName = "ScriptableObjects/Wheel Data")]
public class WheelData : ScriptableObject
{
    public AnimationCurve PacejkaCurve = new AnimationCurve();
    public float GripFactor;
    public AnimationCurve RollResistance;
}
