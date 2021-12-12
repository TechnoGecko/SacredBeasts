using Animancer;
using PlatformerGameKit;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace Characters.States
{
    public class BaseJumpState : CharacterState
    {
        [SerializeField] private ClipTransition _Animation;
        public ClipTransition Animation => _Animation;

        [SerializeField, Range(0, 1)] private float _Inertia = 0.25f;
        public float Inertia => _Inertia;
        
        
        [SerializeField] private float _JumpForce = 10.5f;
        [SerializeField] private float _BootyWeight = 0.7f;
        [SerializeField] private float _VariableJump = 1.2f;

        
        #if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.Clamp(ref _Inertia, 0, 1);
            PlatformerUtilities.NotNegative(ref _JumpForce);
            PlatformerUtilities.NotNegative(ref _BootyWeight);
            PlatformerUtilities.NotNegative(ref _VariableJump);
        }
        #endif


        protected virtual void Awake()
        {
            _Animation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;
        }

        public override bool CanEnterState => Character.Body.IsGrounded;

        public override void OnEnterState()
        {
            base.OnEnterState();

            Character.HasJumped = true;

            Character.InitialVelocity = Character.Body.Velocity;

            Character.Body.Rigidbody2D.velocity = new Vector2(Character.Body.Rigidbody2D.velocity.x, 0);
            Character.Body.Rigidbody2D.AddForce(Vector2.up * _JumpForce, ForceMode2D.Impulse);

            Character.Animancer.Play(_Animation);
        }

        
        
        

        public override float MovementSpeedMultiplier => 0.75f;
    }
}
