using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Drawing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : SerializedMonoBehaviour
{
    [OdinSerialize] Transform[] wheels;
    private float[] springForces, rollForces, gripForces;
    [OdinSerialize] LayerMask groundMask;
    [Header("Suspension Properties")]
    [OdinSerialize] float suspensionRestDist;
    [OdinSerialize] float springConst, springDamp;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        foreach(Transform wheel in wheels)
        {
            Vector3 springDir = wheel.up;
            if (Physics.Raycast(wheel.position, -springDir, out RaycastHit hit, suspensionRestDist, groundMask)) {
                Vector3 tireVel = rb.GetPointVelocity(wheel.position);
                float offset = suspensionRestDist - hit.distance;
                float vel = Vector3.Dot(springDir, tireVel);
                float force = (offset * springConst) - (vel * springDamp);
                rb.AddForceAtPosition(springDir * force, wheel.position);
                
                Draw.Arrow(wheel.position, wheel.position + springDir, Color.green);
                Draw.Arrow(wheel.position, wheel.position + springDir, Color.green);
            }
        }
        
    }
}
