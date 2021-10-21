using System;
using UnityEngine;
using Animancer;
using PlatformerGameKit;


namespace Characters.States
{
    public class WallSlideState : CharacterState
    {
        [SerializeField] private ClipTransition _Animation;

        [SerializeField] float wallSlideSpeed = -1.1f;
        
        [SerializeField]
        [Range(0, 90)]
        [Tooltip("The maximum angle allowed between horizontal and a contact normal for it to be considered a wall")]
        private float _Angle = 40;

        [SerializeField] [Tooltip("The amount of friction used while moving normally")]
        private float _Friction = 8;

        [SerializeField] [Tooltip("The amount of friction used while attempting to run")]
        private float _RunFriction = 12;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.Clamp(ref _Angle, 0, 90);
            PlatformerUtilities.NotNegative(ref _Friction);
            PlatformerUtilities.NotNegative(ref _RunFriction);
        }
#endif


        public override bool CanEnterState
        {
            get
            {

                if (!Character.Body.IsTouchingWall && Character.Body.IsGrounded && Character.Body.Velocity.y < 0)
                    return false;

                return true;
                /*if (Character.MovementDirection.x == 0 ||
                    Character.Body.IsGrounded ||
                    Character.Body.Velocity.y > 0)
                    return false;

                

                var filter = Character.Body.TerrainFilter;
                var angle = Character.MovementDirection.x > 0 ? 180 : 0;
                filter.SetNormalAngle(angle - _Angle, angle + _Angle);
                var count = Character.Body.Rigidbody2D.GetContacts(filter, PlatformerUtilities.OneContact);                         
                return count > 0;
                Debug.Log($"nameof(count)");*/

            }
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(_Animation);
            FixedUpdate();
            Debug.Log("Wallslide state entered");

        }

        public void FixedUpdate()
        {
            
                Character.Body.Rigidbody2D.velocity =
                    new Vector2(Character.Body.Rigidbody2D.velocity.x, wallSlideSpeed);

        }

        public override float MovementSpeedMultiplier => 1;

    }

}
