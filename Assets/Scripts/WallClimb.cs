using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour
{
    [Range(0f, 5f)]
    [Tooltip("How much force you want the player to have while climbing this ladder")]
    [SerializeField] private float climbSpeed = 1f;

    public KeyCode wallClimbKey = KeyCode.W;
    public KeyCode wallClimbKey2 = KeyCode.S;

    public KeyCode wallDismountKey = KeyCode.E;

    private bool OnWall;
    private bool WallZone;
    private Vector3 playerInput;
    private Rigidbody rb;
    private Vector3 gravity;
    private CombinedCharacterController characterController;
    private float maxSpeed;
    private float maxAcceleration;
    private float maxAirAcceleration;
    private float jumpHeight;
    private Collider playerForDestroy;

    private bool inTrigger;

    private Collider player;
    private float VDivider = 5;
    private float HMultipler = 5;

    void Start()
    {
        OnWall = false;
        WallZone = false;
        inTrigger = false;
        playerInput = Vector3.zero;
        gravity = Physics.gravity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void Update()
    {
        if (WallZone)
        {
            if (Input.GetKey(wallClimbKey) || Input.GetKey(wallClimbKey2)) OnWall = true;
        }
        if (OnWall)
        {
            if (Input.GetKey(wallDismountKey))
            {
                OnWall = false;
                WallZone = true;
            }
            playerInput.y = Input.GetAxisRaw("Vertical");
            playerInput.y *= (climbSpeed * VDivider);
            playerInput.x = Input.GetAxisRaw("Horizontal");
            playerInput.x *= (climbSpeed * HMultipler);
        }
        if (Physics.gravity != Vector3.zero) gravity = Physics.gravity;
        if (inTrigger)
        {
            StayOnWall(player);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        player = other;
        inTrigger = true;
        GetOnWall(other);
    }

    void OnTriggerExit(Collider other)
    {
        OnWall = false;
        WallZone = false;
        inTrigger = false;
        GetOffWall(other);

    }

    void OnTriggerStay(Collider other)
    {

        StayOnWall(other);
    }

    private void GetOffWall(Collider player)
    {
        playerForDestroy = player;
        rb = player.gameObject.GetComponent<Rigidbody>();
        characterController = player.gameObject.GetComponent<CombinedCharacterController>();
        if (characterController != null && rb != null)
        {
            Physics.gravity = gravity;
            TurnOnOffAbilities(true);
        }
    }

    private void StayOnWall(Collider player)
    {
        playerForDestroy = player;
        rb = player.gameObject.GetComponent<Rigidbody>();
        characterController = player.gameObject.GetComponent<CombinedCharacterController>();
        if (WallZone && OnWall && characterController != null && rb != null)
        {
            WallZone = false;
            rb.velocity = Vector3.zero;
            TurnOnOffAbilities(false);
            characterController.airJumps = 0;
        }
        if (OnWall && rb != null)
        {
            Physics.gravity = Vector3.zero;
            rb.velocity = playerInput;
            if (playerInput.y == 0 && Mathf.Abs(rb.velocity.y) > 0.01f)
            {
                rb.velocity = Vector3.zero;
            }
        }
        else if(characterController != null && rb != null)
        {
            Physics.gravity = gravity;
            TurnOnOffAbilities(true);
        }
    }

    private void GetOnWall(Collider player)
    {
        playerForDestroy = player;
        characterController = player.gameObject.GetComponent<CombinedCharacterController>();
        if (characterController != null)
        {
            WallZone = true;
            maxAirAcceleration = characterController.maxAirAcceleration;
        }
    }

    void OnDisable()
    {
        if(playerForDestroy!=null) rb = playerForDestroy.gameObject.GetComponent<Rigidbody>();
        if (playerForDestroy != null) characterController = playerForDestroy.gameObject.GetComponent<CombinedCharacterController>();
        Physics.gravity = gravity;
        if (characterController != null && rb != null)
        {
            TurnOnOffAbilities(true);
        }
    }


    private void TurnOnOffAbilities(bool OnOff)
    {
        //if (player.gameObject.GetComponent<JetpackAbility>() != null) player.gameObject.GetComponent<JetpackAbility>().enabled = OnOff;
        //if (player.gameObject.GetComponent<ReverseGravityAbility>() != null) player.gameObject.GetComponent<ReverseGravityAbility>().enabled = OnOff;
    }
}
