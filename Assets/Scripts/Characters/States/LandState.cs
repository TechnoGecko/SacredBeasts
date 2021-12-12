using Animancer;
using Animancer.Units;
using PlatformerGameKit;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace Characters.States
{
    public class LandState : CharacterState
    {
        [SerializeField] private ClipTransition _Animation;

        [SerializeField, MetersPerSecond] private float _MinimumVerticalSpeed = 7;

        [SerializeField, Range(0, 1)] private float _MovementSpeedMultiplier = 1;

        public override float MovementSpeedMultiplier => _MovementSpeedMultiplier;
    
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _MinimumVerticalSpeed);
            PlatformerUtilities.Clamp(ref _MovementSpeedMultiplier, 0, 1);

            if (GetComponent<Collider2D>() == null)
                Debug.LogWarning($"{nameof(LandState)} must be on the same {nameof(GameObject)} as a {nameof(Collider2D)}" +
                                 $" so that it can receive {nameof(OnCollisionEnter2D)} messages.", this);
        }
#endif

        private void Awake()
        {
            _Animation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;

            Character.Body.OnGroundedChanged += OnGroundedChanged;
        }


        private float _GroundContactTime = float.NaN;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.relativeVelocity.y >= _MinimumVerticalSpeed)
                _GroundContactTime = Time.timeSinceLevelLoad;
        }

        private void OnGroundedChanged(bool isGrounded)
        {
            if (isGrounded &&
                _GroundContactTime >= Time.timeSinceLevelLoad - Time.fixedDeltaTime * 1.5f)
                Character.StateMachine.TrySetState(this);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(_Animation);
        }
    }
}
