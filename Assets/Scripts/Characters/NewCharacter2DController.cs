using UnityEngine;

namespace Characters
{
    public class NewCharacter2DController : MonoBehaviour
    { 
        [Header("Movement")]
        [SerializeField] float runSpeed = 10f;
        public Vector2 direction;
        public bool facingRight = true;
        private float jumpTimer;

        [Header("Jumping")]
        [SerializeField] private LayerMask platformLayerMask;
        [SerializeField] float jumpForce = 10.5f;
        [SerializeField] float bootyWeight = 0.7f;
        [SerializeField] float variableJump = 1.2f;
        //[SerializeField] float jumpHeight = 5f;
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
    
    

        [Header("Walljump")]
        [SerializeField] float wallJumpDelay = 0.35f;
        [SerializeField] float wallJumpForce = 3.2f;
        [SerializeField] float wallJumpDirection = -1f;
        [SerializeField] Vector2 wallJumpAngle = new Vector2(9f, 3.2f);
        private float wallJumpTimer;


        [Header("Physics")]
        public float maxSpeed = 5f;
        public float linearDrag = 2.5f;
        public float defaultGravity = 1.5f;
        public float fallSpeed = -10f;

        public Animation anim;
        public Animator animator;
        Rigidbody2D rb2d;
        SpriteRenderer spriteRenderer;
        BoxCollider2D boxCollider;


        [Header("Animation handler")]
        private string currentState;

        //Animation states
        const string PLAYER_IDLE = "Player_Idle1";
        const string PLAYER_JUMP = "Player_jump1";
        const string PLAYER_RUN = "Player_run1";
        const string PLAYER_WALL = "Player_wallslide1";
        const string PLAYER_LEDGE = "Player_ledgegrab1";

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
                if (rb2d.velocity.y < fallSpeed)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, fallSpeed);
                }
                if(rb2d.velocity.y < 0 && !isTouchingWall)
                {
                    rb2d.gravityScale = defaultGravity + bootyWeight;
                } else if (rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    rb2d.gravityScale = defaultGravity + variableJump;
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
            float extraHeightText = .1f;
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

            if (rb2d.velocity.y < 0 && isGrounded == false)
            {
                isFalling = true;
            } else if (isGrounded == true && isFalling == true)
            {
                isFalling = false;
            }

            if (canJump == false && isGrounded == true)
            {
                canJump = true;
                isFalling = false;
            }



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
            float extraLengthText = .1f;
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

        /*void ChangeAnimationState( string newState)
    {

        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;*/
        //}

    }
}
