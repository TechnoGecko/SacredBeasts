using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacter2DController : MonoBehaviour
{ 
    [SerializeField] private LayerMask platformLayerMask;

    [Header("Movement")]
    [SerializeField] float runSpeed = 95f;
    public Vector2 direction;
    private bool facingRight = true;
    private float jumpTimer;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 95f;
    [SerializeField] float bootyWeight = 10f;
    //[SerializeField] float jumpHeight = 50f;
    public bool isGrounded = true;
    public bool hasJumped;
    public bool isFalling = false;
    public float horizontal;
    public float vertical;
    public float jumpDelay = 0.25f;

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
        }
    }

    
 
    private void FixedUpdate()
    {
        GroundCheck();
        MoveCharacter(direction.x);
        ModifyPhysics();
        if(jumpTimer > Time.time && isGrounded)
        {
            Jump();
        }
        
        if (hasJumped == true && rb2d.velocity.y < 0)
        {
            isFalling = true;
        }

        if (hasJumped == true && isGrounded == true)
        {
            hasJumped = false;
        }

        fallSpeed = rb2d.velocity.y;
        
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
            if(rb2d.velocity.y < 0)
            {
                rb2d.gravityScale = defaultGravity * bootyWeight;
            } else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb2d.gravityScale = defaultGravity * (bootyWeight / 2);
            }
        }

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
        isFalling = raycastHit.collider ? true : false;
       
        
    }   

    private void Jump()
    {
       
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        hasJumped = true;
        jumpTimer = 0;
        
        
        animator.SetFloat("vertical", vertical);
    }
     
    


    
}
