using Animancer;
using UnityEngine;

namespace Characters.States
{
    public class FiendLocomotionState : CharacterState
    {
        [SerializeField] private ClipTransition _Idle;
        public ClipTransition Idle => _Idle;

        [SerializeField] private ClipTransition _Run;
        public ClipTransition Run => _Run;

        [SerializeField] private ClipTransition _Fall;
        public ClipTransition Fall => _Fall;

        [SerializeField] private ClipTransition _Stop;

        public ClipTransition Stop => _Stop;

        [SerializeField] private ClipTransition _WallSlide;
        public ClipTransition WallSlide => _WallSlide;

        [SerializeField] private ClipTransition _Dash;
        public ClipTransition Dash => _Dash;

        

        
        

        public ClipTransition CurrentAnimation
        {
            get
            {

                if (Character.Body.IsWallSliding)
                    return _WallSlide;

                if (!Character.Body.IsGrounded && !Character.Body.IsTouchingWall && _Fall.IsValid)
                        return _Fall;

                if (Character.dashing && Character.canDash)
                    return _Dash;
                
                if (Character.MovementDirection.x != 0)
                    return _Run;

                

                if (Character.Body.IsStopping)
                    return _Stop;
                
                return _Idle;
                
            }
        }

        

        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(CurrentAnimation);

            /*if (Character.Body.IsGrounded && Character.InitialVelocity.x != 0)
            {
                
            }*/
        }

        public virtual void Update()
        {
            Character.Animancer.Play(CurrentAnimation);
            
            
        }

        public override float MovementSpeedMultiplier => 1;
    }
}
