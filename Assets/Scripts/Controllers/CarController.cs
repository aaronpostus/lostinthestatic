using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Drawing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : SerializedMonoBehaviour
{
    [OdinSerialize] private IMoveInputProvider inputProvider;

    
    
    private float[,] forces;
    private Color[] forceColors;
    [OdinSerialize] LayerMask groundMask;

    [Header("Engine Properties")]
    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField] private float powerScalar, maxVelocity;

    [Header("Wheel Properties")]
    [OdinSerialize] Wheel[] wheels;
    [SerializeField, Range(0,90)] private float maxSteerAngle, steerSpeed;
    
    [Header("Suspension Properties")]
    [OdinSerialize] private float suspensionRestDist;
    [OdinSerialize] private float springConst, springDamp;
    
    private Rigidbody rb;
    private InputState inputState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forces = new float[3,wheels.Length];
        forceColors = new Color[3] { Color.red, Color.green, Color.blue };
    }

    private void Update()
    {
        inputState = inputProvider.GetState();

        for(int i=0;i<wheels.Length;i++)
        {
            Transform wTransform = wheels[i].transform;
            //Steer wheels
            if (wheels[i].IsSteerable) {
                Vector3 wheelEulers = wTransform.localEulerAngles;
                wheelEulers.y = Mathf.Lerp(wheelEulers.y, wheelEulers.y+Mathf.DeltaAngle(wheelEulers.y,maxSteerAngle*inputState.moveDirection.x), steerSpeed * Time.deltaTime);
                wheels[i].transform.localEulerAngles = wheelEulers;
            }

            Vector3[] forceDirections = new Vector3[3] { wTransform.right, wTransform.up, wTransform.forward };
            for (int j = 0; j< forces.GetLength(0); j++) {
                using (Draw.WithColor(forceColors[j]))
                {
                    Draw.Ray(wTransform.position, forceDirections[j]);
                    Draw.Label2D(wTransform.position + forceDirections[j], ((int)forces[j,i]).ToString(), 20f);
                }
            }
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            Transform wTransform = wheels[i].transform;
            Vector3 tireVel = rb.GetPointVelocity(wTransform.position);

            //Check if tire is on the ground
            Vector3 springDir = wTransform.up;
            if (!Physics.Raycast(wTransform.position, -springDir, out RaycastHit hit, suspensionRestDist, groundMask)) continue;

            //Suspension
            
            float offset = suspensionRestDist - hit.distance;
            float vel = Vector3.Dot(springDir, tireVel);
            float force = (offset * springConst) - (vel * springDamp);
            rb.AddForceAtPosition(springDir * force, wTransform.position);
            forces[1, i] = force;

            //Grip
            Vector3 steerDir = wTransform.right;
            float steerVel = tireVel.magnitude==0? 1:Vector3.Dot(tireVel, steerDir)/tireVel.magnitude;
            float gripFactor = wheels[i].gripCurve.Evaluate(steerVel);
            float steerAcc = gripFactor * -steerVel/Time.fixedDeltaTime;
            force = wheels[i].mass * steerAcc*gripFactor;
            rb.AddForceAtPosition(force*steerDir, wTransform.position);
            forces[0, i] = force;

            //Power
            if (wheels[i].IsPowered)
            {
                float speed = Mathf.Abs(Vector3.Dot(rb.velocity, transform.forward));
                float power = powerCurve.Evaluate(Mathf.InverseLerp(0, maxVelocity, speed));
                force = power * powerScalar * inputState.moveDirection.y;
                rb.AddForceAtPosition(force * wTransform.forward, wTransform.position);
                forces[2, i] = force;
            }
        }
    }
}
