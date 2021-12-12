using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace Characters
{
    public class PlayerBody : MonoBehaviour
    {
        [Header("Physics")] [SerializeField] private float bootyWeight = 0.7f;
        [SerializeField] float variableJump = 2f;
        public float linearDrag = 4f;
        public float defaultGravity = 1f;
        public float fallSpeed = -10f;

        [SerializeField] private LayerMask platformLayerMask;
        [SerializeField] private LayerMask wallLayerMask;

        [SerializeField] private BoxCollider2D _Collider;
        public BoxCollider2D Collider => _Collider;

        [SerializeField] private Rigidbody2D _Rigidbody2D;
        public Rigidbody2D Rigidbody2D => _Rigidbody2D;

        [SerializeField] private bool _TouchingWallRight;

        public bool TouchingWallRight => _TouchingWallRight;
        
        [SerializeField] private bool _TouchingWallLeft;

        public bool TouchingWallLeft => _TouchingWallLeft;
        
        [SerializeField] private bool _IsTouchingWall;

        public bool IsTouchingWall => _IsTouchingWall;
        
        

        [SerializeField] private Character _Character;


        [SerializeField]
        private bool _IsGrounded;

        public bool IsGrounded
        {
            get => _IsGrounded;

            set
            {
                if (_IsGrounded == value)
                    return;

                _IsGrounded = value;
                OnGroundedChanged?.Invoke(value);
            }
        }
        public float Rotation
        {
            get => 0;// _Rigidbody.rotation;
            set => throw new NotSupportedException("Rotation is not supported.");// _Rigidbody.rotation = value;
        }

        /// <summary>The acceleration that gravity is currently applying to this body.</summary>
        public virtual Vector2 Gravity => Physics2D.gravity * _Rigidbody2D.gravityScale;

        public event Action<bool> OnGroundedChanged;


        private int _WallJumpDirection;

        public int WallJumpDirection => _WallJumpDirection;

        private Vector2 _Velocity;
        public Vector2 Velocity => _Velocity;

        private float _HorizontalVelocity;
        public float HorizontalVelocity => _HorizontalVelocity;

        private float _VerticalVelocity;
        public float VerticalVelocity => _VerticalVelocity;
        
        public Vector2 Position
        {
            get => _Rigidbody2D.position;
            set => _Rigidbody2D.position = value;
        }


        private ContactFilter2D _TerrainFilter;

        public ContactFilter2D TerrainFilter => _TerrainFilter;

        private void Awake()
        {
            _Rigidbody2D.sharedMaterial.friction = 0f;
            _TerrainFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        }

        private void FixedUpdate()
        {
            GroundCheck();
            WallCheck();
            ModifyPhysics();


            _Velocity = Rigidbody2D.velocity;
            _VerticalVelocity = _Velocity.y;
            _HorizontalVelocity = _Velocity.x;
            
            
        }

        public void GroundCheck()
        {

            RaycastHit2D raycastHit = Physics2D.BoxCast(_Collider.bounds.center, _Collider.bounds.size, 0f,
                Vector2.down, 0.1f, platformLayerMask);

            Color rayColor;
            if (raycastHit.collider != null)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }

            Debug.DrawRay(_Collider.bounds.center, Vector2.down * (_Collider.bounds.extents.y + 0.1f), rayColor);

            if (raycastHit.collider == null)
            {
                _IsGrounded = false;
            }
            else if (raycastHit.collider)
            {
                _IsGrounded = true;
            }



        }

        public void WallCheck()
        {
            float extraLengthText = .1f;
            RaycastHit2D raycastHitRight = Physics2D.BoxCast(_Collider.bounds.center, _Collider.bounds.size, 0f,
                Vector2.right, extraLengthText, wallLayerMask);
            RaycastHit2D raycastHitLeft = Physics2D.BoxCast(_Collider.bounds.center, _Collider.bounds.size, 0f,
                Vector2.left, extraLengthText, wallLayerMask);
            Color rayColorRight = Color.red;
            Color rayColorLeft = Color.red;
            if (raycastHitRight.collider != null || raycastHitLeft.collider != null)
            {
                if (raycastHitLeft.collider == null && raycastHitRight)
                {
                    rayColorRight = Color.blue;
                }
                else if (raycastHitRight.collider == null && raycastHitLeft)
                {
                    rayColorLeft = Color.blue;
                }
            }
            else
            {
                rayColorRight = Color.red;
                rayColorLeft = Color.red;
            }

            Debug.DrawRay(_Collider.bounds.center, Vector2.right * (_Collider.bounds.extents.x + extraLengthText),
                rayColorRight);
            Debug.DrawRay(_Collider.bounds.center, Vector2.left * (_Collider.bounds.extents.x + extraLengthText),
                rayColorLeft);

            _IsTouchingWall = (raycastHitRight.collider || raycastHitLeft.collider);
            if (raycastHitRight.collider && !raycastHitLeft.collider)
            {
                _WallJumpDirection = -1;
            }
            else if (raycastHitLeft.collider && !raycastHitRight.collider)
            {
                _WallJumpDirection = 1;
            }

        }

        void ModifyPhysics()
        {
             
            var direction = _Character.InputDirection;


            bool changingDirections = (direction.x > 0 && _Rigidbody2D.velocity.x < 0) ||
                                      (direction.x < 0 && _Rigidbody2D.velocity.x > 0);

            if (IsGrounded)
            {


                if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
                {
                    _Rigidbody2D.drag = linearDrag;
                    _Rigidbody2D.sharedMaterial.friction = 1.2f;
                }
                else
                {
                   _Rigidbody2D.sharedMaterial.friction = 0f;
                    _Rigidbody2D.drag = 0f;
                }

                _Rigidbody2D.gravityScale = 0;
            }
            else
            {
                _Rigidbody2D.gravityScale = defaultGravity;
                _Rigidbody2D.drag = linearDrag * 0.15f;
                if (_Rigidbody2D.velocity.y < fallSpeed)
                {
                    _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, fallSpeed);
                }

                if (_Rigidbody2D.velocity.y < 0 && !_IsTouchingWall)
                {
                    _Rigidbody2D.gravityScale = defaultGravity + bootyWeight;
                }
                else if (_Rigidbody2D.velocity.y > 0 && !_Character.Brain.IsHoldingJump)
                {
                    _Rigidbody2D.gravityScale = defaultGravity + variableJump;
                }

            }



        }
        
        public virtual float StepHeight
        {
            get => 0;
            set => throw new NotSupportedException($"Can't set {GetType().FullName}.{nameof(StepHeight)}.");
        }
    }
}
