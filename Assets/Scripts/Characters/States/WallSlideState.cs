using System;
using Animancer;
using PlatformerGameKit;
using UnityEngine;
using static Utilities.PlatformerUtilities;

namespace Characters.States
{
    
    [AddComponentMenu(MenuPrefix + "Wall Slide State")]

    public class WallSlideState : CharacterState
    {
    /************************************************************************************************************************/

        [SerializeField]
        private ClipTransition _Animation;

        [SerializeField]
        [Range(0, 90)]
        [Tooltip("The maximum angle allowed between horizontal and a contact normal for it to be considered a wall")]
        private float _Angle = 40;

        [SerializeField]
        [Tooltip("The amount of friction used while moving normally")]
        private float _Friction = 8;

        [SerializeField]
        [Tooltip("The amount of friction used while attempting to run")]
        private float _RunFriction = 12;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.Clamp(ref _Angle, 0, 90);
            PlatformerUtilities.NotNegative(ref _Friction);
            PlatformerUtilities.NotNegative(ref _RunFriction);
        }
#endif

        /************************************************************************************************************************/

        public override bool CanEnterState
        {
            get
            {
                if (Character.MovementDirectionX == 0 ||
                    Character.Body.IsGrounded ||
                    Character.Body.Velocity.y > 0)
                    return false;

                var filter = Character.Body.TerrainFilter;
                var angle = Character.MovementDirectionX > 0 ? 180 : 0;
                filter.SetNormalAngle(angle - _Angle, angle + _Angle);

                var count = Character.Body.Rigidbody.GetContacts(filter, PlatformerUtilities.OneContact);
                return count > 0;
            }
        }

        /************************************************************************************************************************/

        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(_Animation);
            FixedUpdate();
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            var velocity = Character.Body.Velocity;
            var friction = Character.Run ? _RunFriction : _Friction;
            velocity.y *= 1 - Math.Min(friction * Time.deltaTime, 1);
            Character.Body.Velocity = velocity;
        }

        /************************************************************************************************************************/

        public override float MovementSpeedMultiplier => 1;

        /*************/
    }
}
