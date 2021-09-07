using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacter2DController : MonoBehaviour
{ 
    [SerializeField] private LayerMask platformLayerMask;

    [SerializeField] float runSpeed = 95f;
    [SerializeField] float jumpForce = 45f;
    public Vector2 direction;
    private bool facingRight = true;

    public float maxSpeed = 50f;
    public float linearDrag = 20f;

    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    private string PLAYER_RUN = "Player_run1";
    private string PLAYER_IDLE = "Player_Idle1";
    private string PLAYER_JUMP = "Player_jump1";



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
        
       

    }

    
 
    private void FixedUpdate()
    {
        MoveCharacter(direction.x);
        ModifyPhysics();
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


        if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
        {
            rb2d.drag = linearDrag;
        } else
        {
            rb2d.drag = 0f; 
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }


    private bool IsGrounded()
    {
        float extraHeightText = 5f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask );
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * (boxCollider.bounds.extents.y + extraHeightText), rayColor);
        Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }   


     


    
}
