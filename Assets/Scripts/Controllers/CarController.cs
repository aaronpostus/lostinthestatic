using Drawing;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : SerializedMonoBehaviour, ICameraTargetable
{
    [OdinSerialize] private IMoveInputProvider inputProvider;
    [SerializeField] private Transform cameraTarget;

    private float[,] forces;
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
        forces = new float[3, wheels.Length];
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

            Draw.Arrow(wheels[i].transform.position, wheels[i].transform.position + rb.GetPointVelocity(wheels[i].transform.position));
        }

        Draw.Arrow(transform.position, transform.position+rb.velocity);
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
            
            //Friction
            Vector3 tireGroundVel = Vector3.ProjectOnPlane(tireVel, hit.normal);
            if (tireGroundVel.magnitude > 0) {
                
                float slipRatio = 1 - Mathf.Abs(Vector3.Dot(tireGroundVel.normalized, wTransform.forward));
                float gripFactor = wheels[i].PacejkaCurve.Evaluate(slipRatio);
                Vector3 friction = Vector3.Project(-tireGroundVel, wTransform.right);
                float additionalFriction = tireGroundVel.magnitude/maxVelocity;

                Vector3 frictionForce = Vector3.ClampMagnitude(friction, (slipRatio + additionalFriction) * gripFactor  * Time.fixedDeltaTime);
                Draw.Ray(wTransform.position, frictionForce.normalized, Color.cyan);
                rb.AddForceAtPosition(frictionForce, wTransform.position, ForceMode.VelocityChange);
                //forces[0, i] = force;
            }

            //Power
            if (wheels[i].IsPowered)
            {
                float speed = Mathf.Abs(Vector3.Dot(rb.velocity, transform.forward));
                float power = powerCurve.Evaluate(Mathf.InverseLerp(0, maxVelocity, speed));
                force = power * powerScalar * inputState.moveDirection.y;
                rb.AddForceAtPosition(Vector3.ProjectOnPlane(force * wTransform.forward, hit.normal), wTransform.position);
                forces[2, i] = force;
            }
        }
    }

    public Transform GetTarget() => cameraTarget;
}
