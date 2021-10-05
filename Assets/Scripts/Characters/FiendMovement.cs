using System;
using UnityEngine;
using Animancer;
using Characters.Brains;

namespace Characters
{

    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public class FiendMovement : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Vector2 _direction;
        private bool facingRight;

        public const int DefaultExecutionOrder =
            (CharacterBrain.DefaultExecutionOrder + CharacterBody2D.DefaultExecutionOrder) / 2;

        public const float MinimumSpeed = 0.01f;

        [SerializeField] private Character _Character;
        
        [SerializeField]  private float runSpeed = 10f;
        [SerializeField] private float maxSpeed = 5f;

        public ref Character Character => ref _Character;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif


        protected void Awake()
        {
            rb = Character.Body.Rigidbody;
            _direction = Character.MovementDirection;

        }

        protected void Start()
        {
            throw new NotImplementedException();
        }

        protected void FixedUpdate()
        {
            var previousVelocity = _Character.Body.Velocity;
            
            MoveCharacter(_direction.x);

        }


        void MoveCharacter(float horizontal)
        {
            rb.AddForce(Vector2.right * horizontal * runSpeed);

            
            //===================== Handled in AnimancerComponent ======================
            
            /*if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
            {
                Flip();
            }*/
            
            //==========================================================================    
            
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }


        }
    }
}