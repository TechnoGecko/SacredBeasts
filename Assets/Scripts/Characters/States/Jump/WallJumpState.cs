using UnityEngine;

namespace Characters.States.Jump
{
    public class WallJumpState : BaseJumpState
    {
    
        [SerializeField] float wallJumpForce = 40f;
        [SerializeField] Vector2 wallJumpAngle = new Vector2(15, 33);
    
        //[SerializeField] private ClipTransition _WallJumpAnimation;
        //public ClipTransition WallJumpAnimation => _WallJumpAnimation;
        public override bool CanEnterState
        {

            get
            {
                if (Character.Body.IsGrounded ||
                    !Character.Body.IsTouchingWall )
                    return false;

                if (Character.Body.IsTouchingWall 
                    && !Character.Body.IsGrounded 
                    && Character.Body.Velocity.y <= 0)
                    return true;

                return false;
            }
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            Debug.Log(Character.Body.WallJumpDirection);
            Character.Body.Rigidbody2D.velocity = new Vector2(Character.Body.HorizontalVelocity, 0);
            Character.Body.Rigidbody2D.AddForce(new Vector2(wallJumpForce * Character.Body.WallJumpDirection * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y));
        
        }
    
    }
}
