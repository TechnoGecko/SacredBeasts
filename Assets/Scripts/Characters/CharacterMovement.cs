using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
    public class CharacterMovement : MonoBehaviour
    {

        [SerializeField] private Character _Character;
        public Character Character => _Character;
        
        [SerializeField] float _RunSpeed = 10f;

        [SerializeField] private float _MaxSpeed = 5f;

        [SerializeField] private float _FallSpeed = -10f;

        [SerializeField] private float _dashForce = 6f;

        
        

        private void FixedUpdate()
        {
            MoveCharacter(Character.InputDirection.x);
   
            // Cap Fall Speed
            if (Character.Body.Velocity.y < _FallSpeed)
            {
                Character.Body.Rigidbody2D.velocity = new Vector2 (Character.Body.Velocity.x,_FallSpeed);
            }
            //Exposing Bool for jump detection (possibly not needed)
            if (Character.HasJumped && Character.Body.Velocity.y < 0)
            {
                Character.LandingFromJump = true;
            }

            if (Character.dashing & Character.canDash)
            {
                Vector2 dashDirection;
                if (Character.Animancer.FacingLeft)
                {
                    dashDirection = -transform.right;
                }
                else 
                {
                    dashDirection = transform.right;
                }
                
                
                Character.Body.Rigidbody2D.AddForce(dashDirection * _dashForce,
                    ForceMode2D.Impulse);
            }
            
            
            
            
        }

        private void MoveCharacter(float horizontal)
        {
            Character.Body.Rigidbody2D.AddForce(Vector2.right * horizontal * _RunSpeed);

            if (Mathf.Abs(Character.Body.Rigidbody2D.velocity.x) > _MaxSpeed)
            {
                Character.Body.Rigidbody2D.velocity = new Vector2(Mathf.Sign(Character.Body.Velocity.x) * _MaxSpeed, Character.Body.Rigidbody2D.velocity.y);
            }
            
            

            
            
            /*if (Character.LandingFromJump &&
                Character.Body.IsGrounded )
            {
                Character.LandingFromJump = false;
                Character.HasJumped = false;
                
                if (Character.InputDirection.x != 0 &&
                    Character.InitialVelocity.x != 0)
                {
                    Debug.Log($"nameof(Character.InitialVelocity) + applied");
                                                Character.Body.Rigidbody2D.velocity = new Vector2((Character.InitialVelocity.x * Input.GetAxisRaw("Horizontal")) , 0);
                                                Character.InitialVelocity = new Vector2(0, 0);
                }
            }*/
               
                        
            
        }
    }
}
