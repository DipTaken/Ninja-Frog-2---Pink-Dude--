using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    PlayerChecks playerChecks;
    public bool isActive;
    [SerializeField]
    public bool enableDoubleJump = false;
    public bool canRun = true;
    private bool cancelledDoubleJump = false;
    private bool isRunning, isJumping, isFalling, isWallSliding;
    private bool canDoubleJump = true;
    [SerializeField]
    private float speed, jumpForce, wallSlideSpeed;
    [SerializeField]
    private float wallJumpCooldown, wallJumpCooldownTimer;
    [SerializeField]
    private float maxVelocity;

    // Start is called before the first frame update
    void Start()
    {
        //grab components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerChecks = GetComponent<PlayerChecks>();
    }

    // Update is called once per frame
    void Update()
    {
        bool canMove = isActive && !GameManagerUI.instance.isPaused;
        Animations();
        
        if (canMove) //when player is active and can move
        {
            if (!playerChecks.WallDetected())
            {
                Run();
                DoubleJump();
                CancelDoubleJump();
                Jump();
                isWallSliding = false;
                wallJumpCooldownTimer = 0;
            }
            else
            {
                WallSlide();
                if (isWallSliding)
                {
                    WallJump();
                }
            }
        }
        else if (!isActive) //when player is not active
        {
            rb.velocity = new Vector2(playerChecks.groundVelocity.x, rb.velocity.y); // maintain ground velocity when not active

            // reset all movement states
            if (playerChecks.isGrounded())
            {
                canDoubleJump = true;
            }
            isRunning = false;
            isJumping = false;
            isFalling = false;
            isWallSliding = false;
        }

        ClampYVelocity();
    }
    void ClampYVelocity()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxVelocity, maxVelocity)); // limit vertical speed
    }

    void Run()
    {
        bool chooseSpeed = playerChecks.isGrounded() || Mathf.Abs(rb.velocity.x) < speed; // choose speed based on whether the player is grounded or if speed is less than the set speed
        float runSpeed = chooseSpeed ? speed : Mathf.Abs(rb.velocity.x); // if the player is grounded or speed is less than the set speed, use the set speed, otherwise use the current horizontal velocity
        
        if (!canRun)
        {
            isRunning = false;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0) //move right
        {

            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1) * 1.333333f; // look right and scale it
            isRunning = true;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0) //move left
        {
            rb.velocity = new Vector2(-runSpeed, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1) * 1.333333f; // look left and scale it
            isRunning = true;
        }
        else //if no input is given
        {
            bool chooseGroundVelocityX = playerChecks.isGrounded() || canDoubleJump || cancelledDoubleJump || Mathf.Abs(rb.velocity.x) < speed; // choose ground velocity if any of these conditions are met
            float velocityX = chooseGroundVelocityX ? playerChecks.groundVelocity.x : rb.velocity.x; // if chooseGroundVelocityX is true, use the ground velocity, otherwise use the current horizontal velocity

            rb.velocity = new Vector2(velocityX, rb.velocity.y); // maintain ground velocity or current horizontal velocity
            isRunning = false; 
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && playerChecks.TrueGroundDistance() < 0.1f) //press jump button and is on ground
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // jump
            isJumping = true;
            SoundManager.instance.jump.Play();
        }
        if (rb.velocity.y < 0 && (!playerChecks.isGrounded() || playerChecks.TrueGroundDistance() < 0.1f) || playerChecks.groundVelocity.y != 0) // if the player is falling or the ground velocity is not zero
        {
            isJumping = false;
        }
    }

    void WallSlide()
    {
        wallJumpCooldownTimer += Time.deltaTime;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue)); // limit vertical speed while wall sliding
        isWallSliding = true;
        isRunning = false;
    }

    void WallJump()
    {
        bool canWallJump = wallJumpCooldownTimer > wallJumpCooldown && isWallSliding;

        if (canWallJump && Input.GetButtonDown("Jump")) // if the player can wall jump and presses the jump button
        {
            canRun = false;
            wallJumpCooldownTimer = 0;
            SoundManager.instance.jump.Play();
            Vector2 jumpDirection = new Vector2(-transform.localScale.x * speed, jumpForce); // jump in the opposite direction of the wall
            rb.velocity = jumpDirection;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // Flip direction
            Invoke("StopWallJump", 0.21f);
            isJumping = true;
        }
        if (rb.velocity.y < 0)
        {
            isJumping = false;
        }
    }
    void StopWallJump()
    {
        canRun = true;
    }

    void DoubleJump()
    {
        //if double jump is enabled and the player can double jump
        if (enableDoubleJump && canDoubleJump && Input.GetButtonDown("Jump") && !playerChecks.isGrounded() && !isWallSliding && playerChecks.TrueGroundDistance() > 0.1f)
        {
            Vector2 playerDirectionX = new Vector2(Mathf.Clamp(transform.localScale.x, -1f, 1f), 0); // get the player's direction based on their scale
            rb.velocity = new Vector2(rb.velocity.x + playerDirectionX.x * 3, jumpForce * 0.8f); // jump towards the player's direction with a reduced jump force
            isJumping = true;
            canRun = false;
            SoundManager.instance.jump.Play();
            canDoubleJump = false;
        }
        if (playerChecks.isGrounded()) //if the player is on the ground, reset double jump
        {
            canDoubleJump = true;
            cancelledDoubleJump = false;
            canRun = true;
        }
        if (!canDoubleJump && Mathf.Abs(rb.velocity.x) < speed)
        {
            canRun = true; 
        }
    }

    void CancelDoubleJump()
    {
        //when the player is running in the opposite direction of their current facing direction, cancel double jump
        float playerDirection = Mathf.Clamp(transform.localScale.x, -1f, 1f);
        if (!canDoubleJump && !cancelledDoubleJump && Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") != playerDirection && !isWallSliding)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            cancelledDoubleJump = true;
            canRun = true;
        }
    }

    void Animations()
    {
        isFalling = rb.velocity.y < 0 && !playerChecks.isGrounded();
        animator.SetBool("Running", isRunning && !isJumping && !isFalling);
        animator.SetBool("Jumping", isJumping);
        animator.SetBool("Falling", isFalling);
        animator.SetBool("Sliding", isWallSliding);
        animator.SetBool("DoubleJump", !canDoubleJump && !cancelledDoubleJump && !isWallSliding);
    }
}