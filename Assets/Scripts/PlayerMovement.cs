
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    // get the rigidbody from the player
    private Rigidbody2D rb { get; set;}

    // get the animator from the player
    private Animator anim { get; set; }

    // get the sprite from the player
    private SpriteRenderer sprite { get; set; }

    // get collider from the player
    private BoxCollider2D coll { get; set; }

    // hold the value when going positive (right) or negative (left)
    private float dirX = 0f;

    private ItemCollector ic;
    private bool doubleJumpAvailable;
    private bool hasDoubledJumped = false;

    private bool isWallSliding;
    private float wallSlideSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(7f, 10f);

    //movement diff
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpForceV2 = 5f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    

    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();    
        this.anim = GetComponent<Animator>();
        this.sprite = GetComponent<SpriteRenderer>();
        this.coll = GetComponent<BoxCollider2D>();
        this.ic = GetComponent<ItemCollector>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxis (gradually slows) vs GetAxisRaw (instant stops)
        dirX = Input.GetAxisRaw("Horizontal");

        // move left or right, and keep the same y level
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        //if jump is pressed (w or space)
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                doubleJumpAvailable = true;
                //hasDoubledJumped = false;
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            else if (doubleJumpAvailable && this.ic.GetCherry() > 0)
            {
                this.ic.SetCherry(ic.GetCherry() - 1);
                this.ic.UpdateCherryText();
                hasDoubledJumped = true;
                doubleJumpAvailable = false;
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForceV2);

            }
        }

        WallSlide();
        WallJump();

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        //check if moving left or right
        if (dirX > 0f)
        {
            state = MovementState.Running;
            this.sprite.flipX = false;
            this.wallCheck.position = new Vector2(this.transform.position.x + .7f, this.wallCheck.position.y);
            
        }
        else if (dirX < 0f)
        {
            state = MovementState.Running;
            this.sprite.flipX = true; 
            this.wallCheck.position = new Vector2(this.transform.position.x - .7f, this.wallCheck.position.y);
        }
        else
        {
            state = MovementState.Idle;
        }

        //check if going up or down
        if (rb.velocity.y > .1f)
        {
            state = MovementState.Jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
        }


        if (isWallSliding)
        {
            state = MovementState.Sliding;
        }


        anim.SetInteger("state", (int)state);
    }



    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && dirX != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -dirX;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            jumpSoundEffect.Play();
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (dirX != wallJumpingDirection)
            {
                this.sprite.flipX = false;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private bool IsGrounded()
    {
                             //{  creates a (collider) box like the players  }      {moves new box down}
       return Physics2D.BoxCast(this.coll.bounds.center, this.coll.bounds.size, 0f, Vector2.down, .1f, this.jumpableGround);
                                                                            //{direction}              {checks if its overlapping/on the layermask}
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }









}
