using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacter2DController : MonoBehaviour
{ 
    [Header("Movement")]
    [SerializeField] float runSpeed = 95f;
    public Vector2 direction;
    private bool facingRight = true;
    private float jumpTimer;

    [Header("Jumping")]
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] float jumpForce = 95f;
    [SerializeField] float bootyWeight = 10f;
    //[SerializeField] float jumpHeight = 50f;
    public bool isGrounded = true;
    public bool canJump;
    public bool isFalling = false;
    public float horizontal;
    public float vertical;
    public float jumpDelay = 0.25f;

    [Header("WallSlide")]
    [SerializeField] float wallSlideSpeed = -1.1f;
    [SerializeField] private LayerMask wallLayerMask;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isStuck;
    

    [Header("Walljump")]
    [SerializeField] float wallJumpDelay = 0.35f;
    [SerializeField] float wallJumpForce = 18f;
    [SerializeField] float wallJumpDirection = -1f;
    [SerializeField] Vector2 wallJumpAngle = new Vector2(9f, 3.2f);
    private float wallJumpTimer;


    [Header("Physics")]
    public float maxSpeed = 50f;
    public float linearDrag = 20f;
    public float defaultGravity = 2f;
    public float fallSpeed;

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
            wallJumpTimer = Time.time + wallJumpDelay;
        }
    }

    
 
    private void FixedUpdate()
    {
        GroundCheck();
        WallCheck();
        WallSlide();
        MoveCharacter(direction.x);
        ModifyPhysics();
        if(jumpTimer > Time.time && isGrounded)
        {
            Jump();
        }
        else if(wallJumpTimer > Time.time && isWallSliding)
        {
            WallJump();
            
        }
        
        if (rb2d.velocity.y < 0)
        {
            isFalling = true;
        }

        if (canJump == false && isGrounded == true)
        {
            canJump = true;
        }
        
        horizontal = rb2d.velocity.x;
        vertical = rb2d.velocity.y;
        
    }

    void MoveCharacter(float horizontal)
    {
        rb2d.AddForce(Vector2.right * horizontal * runSpeed);
        
        animator.SetFloat("horizontal", Mathf.Abs(rb2d.velocity.x));
        
        if((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }
        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }

    }

    void ModifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb2d.velocity.x < 0) || (direction.x < 0 && rb2d.velocity.x > 0);

        if (isGrounded)
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb2d.drag = linearDrag;
            }
            else
            {
                rb2d.drag = 0f;
            }
            rb2d.gravityScale = 0; 
        } else
        {
            rb2d.gravityScale = defaultGravity;
            rb2d.drag = linearDrag * 0.15f;
            if(rb2d.velocity.y < 0 && !isTouchingWall)
            {
                rb2d.gravityScale = defaultGravity * bootyWeight;
            } else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb2d.gravityScale = defaultGravity * (bootyWeight / 2);
            }
        }


        //--Old bootyWeight logic for reference--

        /*if ( !isGrounded && rb2d.velocity.y < 0)
        {
            rb2d.gravityScale = defaultGravity * bootyWeight;
            
            rb2d.drag = linearDrag * 0.15f;
        } else if (!isGrounded)
        {
            rb2d.gravityScale = defaultGravity;
            rb2d.drag = linearDrag * 0.15f;
        }*/
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }


    private void GroundCheck()
    {
        float extraHeightText = 1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size , 0f, Vector2.down, extraHeightText, platformLayerMask );
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + extraHeightText), rayColor);
        
        isGrounded = raycastHit.collider ? true : false;
        
       
        
    }   

    private void Jump()
    {
       
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        canJump = false;
        jumpTimer = 0;
        
        
        animator.SetFloat("vertical", vertical);
    }

    private void WallCheck()
    {
        float extraLengthText = 1f;
        RaycastHit2D raycastHitRight = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.right, extraLengthText, wallLayerMask);
        RaycastHit2D raycastHitLeft = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.left, extraLengthText, wallLayerMask);
        Color rayColorRight = Color.red;
        Color rayColorLeft = Color.red;
        if (raycastHitRight.collider != null)
        {
            rayColorRight = Color.blue;
        }
        else if (raycastHitLeft.collider != null)
        {
            rayColorLeft = Color.blue;
            
        } 
        else
        {
            rayColorRight = Color.red;
            rayColorLeft = Color.red;
         }
        Debug.DrawRay(boxCollider.bounds.center, Vector2.right * (boxCollider.bounds.extents.x + extraLengthText), rayColorRight);
        Debug.DrawRay(boxCollider.bounds.center, Vector2.left * (boxCollider.bounds.extents.x + extraLengthText), rayColorLeft);

        isStuck = ((raycastHitRight.collider && raycastHitLeft.collider) ? true : false);


        isTouchingWall = (raycastHitRight.collider || raycastHitLeft.collider) ? true : false;
        if(raycastHitRight.collider && !raycastHitLeft.collider)
        {
            wallJumpDirection = -1;
        } else if(raycastHitLeft.collider && !raycastHitRight.collider)
        {
            wallJumpDirection = 1;
        }
        
        


    }

    private void WallSlide()
    {
        if (isTouchingWall && !isGrounded && rb2d.velocity.y < 0)
        {
            isWallSliding = true;
        } else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, wallSlideSpeed);
        }
    }

    private void WallJump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(wallJumpForce * wallJumpDirection * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
        
        wallJumpTimer = 0;

        Debug.Log("walljump");
        animator.SetFloat("vertical", vertical);

    }

}
