using Animancer.Units;
using UnityEngine;
using Utilities;

namespace Characters
{
    public sealed class GroundCharacterMovement : CharacterMovement
    {
        [SerializeField] private float _WalkSpeed = 6f;
        [SerializeField] private float _RunSpeed = 10f;
        [SerializeField, Seconds] private float _WalkSmoothing = 0;
        [SerializeField, Seconds] private float _RunSmoothing = 0.15f;
        [SerializeField, Seconds] private float _AirSmoothing = 0.3f;
        [SerializeField, Seconds] private float _FrictionlessSmoothing = 0.3f;
        [SerializeField] private float _GripFriction = 0.4f;

        private float _SmoothingSpeed;
    
#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _WalkSpeed);
            PlatformerUtilities.NotNegative(ref _RunSpeed);
            PlatformerUtilities.NotNegative(ref _WalkSmoothing);
            PlatformerUtilities.NotNegative(ref _RunSmoothing);
            PlatformerUtilities.NotNegative(ref _AirSmoothing);
            PlatformerUtilities.NotNegative(ref _FrictionlessSmoothing);
            PlatformerUtilities.NotNegative(ref _GripFriction);
        }
#endif

        protected override Vector2 UpdateVelocity(Vector2 velocity)
        {
            var brainMovement = Character.MovementDirection.x;
            var currentState = Character.StateMachine.CurrentState;

            var targetSpeed = Character.Run ? _RunSpeed : _WalkSpeed;
            targetSpeed *= brainMovement * currentState.MovementSpeedMultiplier;

            if (!Character.Body.IsGrounded)
            {
                velocity.x = PlatformerUtilities.SmoothDamp(velocity.x, targetSpeed, ref _SmoothingSpeed, _AirSmoothing);
                return velocity;
            }

            var direction = Vector2.right;
            var ground = Character.Body.GroundContact;

            var smoothing = CalculateGroundSmoothing(ground.Collider.friction);

            var platformVelocity = ground.Velocity;
            velocity -= platformVelocity;
            var currentSpeed = Vector2.Dot(direction, velocity);

            velocity -= direction * currentSpeed;

            currentSpeed = PlatformerUtilities.SmoothDamp(currentSpeed, targetSpeed, ref _SmoothingSpeed, smoothing);

            velocity += direction * currentSpeed + platformVelocity;

            return velocity;
        }

        private float CalculateGroundSmoothing(float friction)
        {
            var target = Character.Run ? _RunSmoothing : _WalkSmoothing;
            if (_GripFriction == 0)
                return target;

            return Mathf.Lerp(_FrictionlessSmoothing, target, friction / _GripFriction);
        }
    }
}
