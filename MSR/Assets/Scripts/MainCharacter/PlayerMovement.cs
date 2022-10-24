using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float MoveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float attackCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    bool readyToAttack = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode jumpJoystick = KeyCode.Joystick1Button0;
    public KeyCode attackMouse = KeyCode.Mouse0;
    public KeyCode attackJoystick = KeyCode.Joystick1Button2;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    private Animator animator;


    Vector3 moveDirection;

    Rigidbody rb;
    GameObject Orient;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb =  GameObject.Find("Player").GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        
    }


    // Update is called once per frame
    void Update()
    {
        Orient = GameObject.Find("Orientation");
        grounded = Physics.Raycast(  Orient.transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //Debug.DrawLine(new Vector3(200, 200, 200), Vector3.zero, Color.green, 2, false);
        //print("position: " + Orient.transform.position);
        //print("playerHeight: " + playerHeight);
        //print("vector del raycast: " + (playerHeight * 0.5f + 0.2f));
        //print("ESTA EN EL SUELO: " + grounded);



        Myinput();
        SpeedControl();
        

            if (moveDirection != Vector3.zero )
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            // handle drag
            if (grounded)
            {
                animator.SetBool("IsGrounded", true);
                rb.drag = groundDrag;
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }
            else
            {

                rb.drag = 0;
                animator.SetBool("IsGrounded", false);
                animator.SetBool("IsJumping", true);
                if (rb.velocity.y < 0)
                {
                    animator.SetBool("IsJumping", false);
                    animator.SetBool("IsFalling", true);
                }
            }
        

        
    }

    private void FixedUpdate()
    {
        MovePlayer();

       // Debug.DrawLine(Orient.transform.position, Vector3.down , Color.green);
    }

    private void Myinput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump 
        if ((Input.GetKey(jumpKey) || Input.GetKey(jumpJoystick)) && readyToJump && grounded)
        {
            animator.SetBool("IsJumping", true);
            grounded = false;
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

        //when to attack
        if ((Input.GetKey(attackMouse) || Input.GetKey(attackJoystick)) && grounded && readyToAttack)
        {   

            readyToAttack = false;
            attack();

            Invoke(nameof(resetAttack), attackCooldown);
        }

    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


        // on ground 
        if (grounded) 
        { 
            rb.AddForce(moveDirection.normalized * MoveSpeed * 10f, ForceMode.Force);

        }
        else if (!grounded) // in air
            rb.AddForce(moveDirection.normalized * MoveSpeed * 10f * airMultiplier, ForceMode.Force);

    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

         

        // limit velocity if needed
        if (flatVel.magnitude > MoveSpeed)
        { 
            Vector3 limitedVel = flatVel.normalized * MoveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {

        
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
       
    }

    private void resetJump()
    {
        readyToJump = true;
    }

    private void attack()
    {
        animator.SetBool("IsAttackingM1", true);
    }

    private void resetAttack()
    {
        readyToAttack = true;
        animator.SetBool("IsAttackingM1", false);
    }
}
