using UnityEngine;

namespace Characters.States.Jump
{
    public class WallJumpState : BaseJumpState
    {
    
        [SerializeField] float wallJumpForce = 2f;
        [SerializeField] Vector2 wallJumpAngle = new Vector2(1.6f, 4.5f);
    
        //[SerializeField] private ClipTransition _WallJumpAnimation;
        //public ClipTransition WallJumpAnimation => _WallJumpAnimation;
        public override bool CanEnterState
        {

            get
            {
                if (Character.Body.IsGrounded ||
                    !Character.Body.IsWallSliding )
                    return false;
                    
                
                if (Character.Body.wallJumpTimer > Time.time
                    && !Character.Body.IsGrounded)
                {
                    Debug.Log("walljump state enabled");
                    return true;
                    
                }

                return false;
            }
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Body.Rigidbody2D.velocity = new Vector2(Character.Body.HorizontalVelocity, 0);
            Character.Body.Rigidbody2D.AddForce(new Vector2(wallJumpForce * Character.Body.WallJumpDirection * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y ), ForceMode2D.Impulse);
            Debug.Log("WallJump state entered");
        }
    
    }
}
