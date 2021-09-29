using UnityEngine;
using Animancer;

namespace Characters.States
{
    public class LocomotionState : CharacterState
    {
    
        [SerializeField]
        private ClipTransition _Idle;
        public ClipTransition Idle => _Idle;
        
        [SerializeField]
        private ClipTransition _Walk;
        public ClipTransition Walk => _Walk;
        
        [SerializeField]
        private ClipTransition _Run;
        public ClipTransition Run => _Run;
        
        [SerializeField]
        private ClipTransition _Fall;
        public ClipTransition Fall => _Fall;
        
#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            _Idle?.Clip.EditModeSampleAnimation(Character);
        }
#endif

        public ClipTransition CurrentAnimation
        {
            get
            {
                if (!Character.Body.IsGrounded && _Fall.IsValid)
                    return _Fall;

                if (Character.MovementDirection.x != 0)
                    return Character.Run && _Run.IsValid ? _Run : _Walk;

                return _Idle;
            }
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(CurrentAnimation);
        }

        public virtual void Update()
        {
            Character.Animancer.Play(CurrentAnimation);
        }

        public override float MovementSpeedMultiplier => 1;

    }

}

