using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDeplacement : MonoBehaviour
{
    public attack attacks;
    public GameObject playerObj;
    [Header("Movement")]
    public float moveSpeed;
    

    public float groundDrag;

    public float dashSpeed;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

     public float walkSpeed;
     public float sprintSpeed;
    public bool jump;
    public bool iddleBool;
    public bool walkingBool;
    public bool playerAttackBoolSpeed = false;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [HideInInspector] public TextMeshProUGUI text_speed;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        dashing,
        iddle
    }
    public bool iddle;
    public bool walking;
    public bool dashing;
    public bool sprinting;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        
        Vector3 flatVel2 = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
       
        
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (sprinting == true && dashing == false)
        {
            state = MovementState.sprinting;
        }
        if (sprinting == false && dashing == false)
        {
            state = MovementState.walking;
            
        }
        if (sprinting == false && dashing == true)
        {
            state = MovementState.dashing;
            
        }
        if(dashing == false && sprinting == false && walking == true)
        {
            playerObj.GetComponent<Animator>().SetBool("iddle", false);
            playerObj.GetComponent<Animator>().SetBool("sprinting", false);
            playerObj.GetComponent<Animator>().SetBool("dash", false);
            playerObj.GetComponent<Animator>().SetBool("walking", true);
        }
        if (flatVel2.magnitude < 1)
        {
            playerObj.GetComponent<Animator>().SetBool("iddle", true);
            playerObj.GetComponent<Animator>().SetBool("sprinting", false);
            playerObj.GetComponent<Animator>().SetBool("dash", false);
            playerObj.GetComponent<Animator>().SetBool("walking", false);
            iddle = true;
            state = MovementState.iddle;
            iddleBool = true;
        }
        if (flatVel2.magnitude > 1)
        {
            walkingBool = true;
            playerObj.GetComponent<Animator>().SetBool("walking", true);
        }
        

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            sprinting = true;
            playerObj.GetComponent<Animator>().SetBool("sprinting", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprinting = false;
            playerObj.GetComponent<Animator>().SetBool("sprinting", false);
        }
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed && sprinting == false && dashing == false && attacks.performingAnAttack == false)
        {

            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        else if (flatVel.magnitude > moveSpeed && sprinting == true && dashing == false)
        {
            Vector3 limitedVel = flatVel.normalized * sprintSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        else if (flatVel.magnitude > moveSpeed && dashing == true )
        {
            Vector3 limitedVel = flatVel.normalized * dashSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        else if (flatVel.magnitude > moveSpeed && attacks.performingAnAttack == true)
        {
            Vector3 limitedVel = flatVel.normalized * 0.25f;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ennemie" && attacks.performingAnAttack)
        {
            print("hit!");
        }
    }

    private void Jump()
    {
        jump = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
