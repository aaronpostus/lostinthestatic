using Drawing;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor : SerializedMonoBehaviour, IPhysical, ICameraTargetable
{
    [Header("Input Source")]
    [OdinSerialize] private IInputProvider inputProvider;
    [SerializeField] private Transform cameraTarget;
        
    public int priority => 5;
    private InputState inputState;
    [Header("Physical Properties")]
    [SerializeField] private float gravity;
    [SerializeField] private float terminalVelocity, friction, airFriction;
    [Header("Movement Properties")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float groundAccelerationScalar, airAccelerationScalar;
    [SerializeField, Range(0, 1)] private float rotationLerp;
    private float currentMaxSpeed;
    [SerializeField, Range(0, 90)] private float maxSlope;
    [SerializeField, Range(0, 1)] private float slopeJumpBias;
    private float slopeDotProduct;
    [SerializeField, Range(0, 10)] private float jumpHeight;
    [SerializeField] LayerMask groundMask;
    public Vector3 getNormal { get; private set; }
    public Vector3 getSteepNormal { get; private set; }

    [Header("Read Only")]
    [SerializeField] private Vector3 contactNormal; 
    [SerializeField] private Vector3 steepContactNormal;
    [SerializeField] private int groundContactCount;
    private bool shouldJump;
    private bool isGrounded => groundContactCount > 0;
    public Vector3 velocity =>rb.velocity;
    private Vector3 targetVelocity;
    public Vector3 position => transform.position;

    [SerializeField] private float targetRotationAngle;
    private bool previouslyGrounded;
    private Rigidbody rb;
    private ContactPoint[] contactBuffer = new ContactPoint[10];
    
    void OnValidate() {
        slopeDotProduct = Mathf.Cos(maxSlope * Mathf.Deg2Rad);
    }

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 50;
        rb.sleepThreshold = 0.0f;
        currentMaxSpeed = maxSpeed;
        OnValidate();
        GameManager.Instance.Player = this;
    }

    private void OnEnable() => inputProvider.OnJump.started += TryJump;

    private void OnDisable() => inputProvider.OnJump.started -= TryJump;

    void Update()
    {
        inputState = inputProvider.GetState();
    }

    void LateUpdate() {
        using (Draw.WithColor(Color.blue))
        {
            Draw.ArrowheadArc(transform.position, inputState.moveDirection, 0.5f, 30f);
            Draw.Cross(inputState.aimPoint);
        }

        using (Draw.WithColor(Color.green))
        {
            Draw.ArrowheadArc(transform.position, transform.forward, 0.5f);
            Draw.Arrow(transform.position - Vector3.up, transform.position - Vector3.up+getNormal);
        }
    }

    void FixedUpdate() {
        contactNormal = getNormal = isGrounded ? contactNormal.normalized : Vector3.up;
        getSteepNormal = steepContactNormal.normalized;
 
        UpdateTargetsFromInput();
        ApplyGravity();
        //RotatePlayer();
        MovePlayer();
        ApplyFriction();

        if (shouldJump)
        {
            shouldJump = false;
            Jump();
        }

        previouslyGrounded = isGrounded;
        groundContactCount = 0;
        contactNormal = Vector3.zero;
        steepContactNormal = Vector3.zero;
    }

    private void UpdateTargetsFromInput()
    {
        targetVelocity = Vector3.zero;
        if (inputState.moveDirection.magnitude == 0) return;
        targetVelocity = inputState.moveDirection * currentMaxSpeed;
        targetRotationAngle = (Mathf.Rad2Deg * Mathf.Atan2(-inputState.moveDirection.z, inputState.moveDirection.x) + 360+90) % 360;
    }

    private void MovePlayer() {
        if (!isGrounded && targetVelocity.magnitude < 0.01f) return;
        float maxAcceleration = maxSpeed * (isGrounded ? groundAccelerationScalar : airAccelerationScalar);
        if (getSteepNormal.magnitude > 0 && Vector3.Dot(targetVelocity, getSteepNormal) < 0) targetVelocity = Vector3.ProjectOnPlane(targetVelocity, getSteepNormal);
        rb.AddForce(Vector3.ClampMagnitude(ProjectOnContactPlane(targetVelocity - velocity), maxAcceleration * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }

    private void RotatePlayer() {
        float velocityToNext = Mathf.DeltaAngle(transform.eulerAngles.y, targetRotationAngle) * Mathf.Deg2Rad/Time.fixedDeltaTime*rotationLerp;
        rb.AddTorque(Vector3.up * (velocityToNext - rb.angularVelocity.y), ForceMode.VelocityChange);
    }

    private void ApplyFriction()
    {
        Vector3 frictionForce = ProjectOnContactPlane(-velocity);
        if (getSteepNormal.magnitude > 0 && Vector3.Dot(frictionForce, getSteepNormal) < 0)
        {
            frictionForce = Vector3.ProjectOnPlane(frictionForce, getSteepNormal);
        }
        rb.AddForce(Vector3.ClampMagnitude(frictionForce, (isGrounded ? friction : airFriction) * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }

    private void ApplyGravity() {
        if (isGrounded || velocity.y < -terminalVelocity) return;
        rb.AddForce(Vector3.ClampMagnitude((-terminalVelocity - velocity.y ) * Vector3.up, gravity * Time.fixedDeltaTime), ForceMode.VelocityChange);
    }

    private void TryJump() {
        if (isGrounded) shouldJump = true;
    }

    private void Jump() {
        float jumpSpeed = Mathf.Sqrt(2f * gravity * jumpHeight);
        if (velocity.y != 0) rb.AddForce(new Vector3(0, -velocity.y, 0), ForceMode.VelocityChange);
        rb.AddForce(Vector3.Lerp(Vector3.up, getNormal, slopeJumpBias) * jumpSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision) => EvaluateGroundCollision(collision);

    private void OnCollisionStay(Collision collision) => EvaluateGroundCollision(collision);

    private void EvaluateGroundCollision(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (Utilities.IsInLayer(groundMask, layer)) {
            for (int i = 0; i < collision.GetContacts(contactBuffer); i++) {
                Vector3 normal = collision.GetContact(i).normal;
                if (normal.y >= slopeDotProduct) {
                    groundContactCount++;
                    contactNormal += normal;
                } else {
                    steepContactNormal += normal;
                }
            }
        }
    }

    private Vector3 ProjectOnContactPlane(Vector3 vector) {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    public Transform GetTarget() => cameraTarget;
}
