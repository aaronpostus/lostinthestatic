using Drawing;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : SerializedMonoBehaviour, ICameraTargetable
{
    [OdinSerialize] private IMoveInputProvider inputProvider;
    [SerializeField] private Transform cameraTarget;

    private Vector3[,] wheelForces;
    private Color[] forceColors;
    [OdinSerialize] LayerMask groundMask;

    [SerializeField] private AnimationCurve powerCurve;
    [SerializeField] private float powerScalar, maxVelocity;

    [OdinSerialize] Wheel[] wheels;
    [SerializeField, Range(0, 90)] private float maxSteerAngle, steerSpeed;

    [OdinSerialize] private float suspensionRestDist;
    [OdinSerialize] private float springConst, springDamp;

    private Rigidbody rb;
    private InputState inputState;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down * 2;
        wheelForces = new Vector3[3, wheels.Length];
        forceColors = new Color[3] { Color.red, Color.green, Color.blue };
        GameManager.Instance.Car = this;
    }

    private void OnEnable()
    {
        GameManager.PlayerStateChanged += HandleStateChange;
    }

    private void OnDisable()
    {
        GameManager.PlayerStateChanged -= HandleStateChange;
    }

    private void HandleStateChange (PlayerState state){
        rb.isKinematic = state != PlayerState.InCar;
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

            for(int j=0; j<forceColors.Length;j++)
            {
                using (Draw.WithColor(forceColors[j]))
                {
                    Draw.Arrow(wheels[i].transform.position, wheels[i].transform.position + wheelForces[j, i].normalized);
                }
            }
        }
        Draw.Arrow(transform.position, transform.position+rb.velocity, Color.yellow);
    }

    void FixedUpdate()
    {
        Vector3 forceToApply;
        for (int i = 0; i < wheels.Length; i++)
        {
            Transform wTransform = wheels[i].transform;
            Vector3 velocity = rb.GetPointVelocity(wTransform.position);
            
            //Check if tire is on the ground
            Vector3 springDir = wTransform.up;
            if (!Physics.Raycast(wTransform.position, -springDir, out RaycastHit hit, suspensionRestDist, groundMask)) continue;
            
            //Suspension
            float offset = suspensionRestDist - hit.distance;
            float springVelocity = Vector3.Dot(springDir, velocity);
            float force = (offset * springConst) - (springVelocity * springDamp);
            forceToApply = springDir * force;
            rb.AddForceAtPosition(forceToApply, wTransform.position);
            wheelForces[1, i] = forceToApply;
            
            //Friction
            Vector3 groundVelocity = Vector3.ProjectOnPlane(velocity, hit.normal);
            if (groundVelocity.magnitude > 0) {
                
                float slipRatio = 1 - Mathf.Abs(Vector3.Dot(groundVelocity.normalized, wTransform.forward));
                float gripFactor = wheels[i].Data.PacejkaCurve.Evaluate(slipRatio)*wheels[i].Data.GripFactor;
                Vector3 friction = Vector3.Project(-groundVelocity, wTransform.right);
                float additionalFriction = groundVelocity.magnitude/maxVelocity;

                forceToApply = Vector3.ClampMagnitude(friction, (slipRatio + additionalFriction) * gripFactor  * Time.fixedDeltaTime);
                
                rb.AddForceAtPosition(forceToApply, wTransform.position, ForceMode.VelocityChange);
                wheelForces[0, i] = forceToApply;
            }
            //Power

            Vector3 forwardVelocity = Vector3.Project(groundVelocity, wTransform.forward);
            float speed = forwardVelocity.magnitude;
            if (wheels[i].IsPowered && inputState.moveDirection.y != 0)
            {
                float power = powerCurve.Evaluate(speed/maxVelocity);
                force = power * powerScalar * inputState.moveDirection.y;
                forceToApply = Vector3.ProjectOnPlane(force * wTransform.forward, hit.normal);
                rb.AddForceAtPosition(forceToApply, wTransform.position);
            } else {
                float resistance = wheels[i].Data.RollResistance.Evaluate(speed / maxVelocity)* wheels[i].Data.RollResistanceScalar;
                forceToApply = Vector3.ClampMagnitude(-forwardVelocity, speed * resistance * Time.fixedDeltaTime);
                rb.AddForceAtPosition(forceToApply, wTransform.position, ForceMode.Impulse);
            }
            wheelForces[2, i] = forceToApply;
        }
    }

    public Transform GetTarget() => cameraTarget;
}
