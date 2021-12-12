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
        

        public ClipTransition CurrentAnimation
        {
            get
            {
                if (!Character.Body.IsGrounded && _Fall.IsValid)
                    return _Fall;

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
