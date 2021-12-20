using System;
using UnityEngine;
using Animancer;
using Utilities;
using Scripts;

namespace Characters.States
{
    public class WallSlideState : CharacterState
    {
        [SerializeField] private ClipTransition _Animation;

       
        
        [SerializeField]
        [Range(0, 90)]
        [Tooltip("The maximum angle allowed between horizontal and a contact normal for it to be considered a wall")]
        private float _Angle = 40;

        [SerializeField] [Tooltip("The amount of friction used while moving normally")]
        private float _Friction = 8;

        [SerializeField] [Tooltip("The amount of friction used while attempting to run")]
        private float _RunFriction = 12;
        
        [SerializeField] private float wallSlideGravity = .2f;

        [SerializeField] private float maxWallSlideSpeed = 3f;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Scripts.PlatformerUtilities.Clamp(ref _Angle, 0, 90);
            Scripts.PlatformerUtilities.NotNegative(ref _Friction);
            Scripts.PlatformerUtilities.NotNegative(ref _RunFriction);
        }
#endif


        public override bool CanEnterState
        {
            
            
            get
            {
                var body = Character.Body;

                if (!body.IsWallSliding || !body.IsGrounded)
                {
                    
                    
                    return false;
                    
                }
                
                
                
                return (body.IsTouchingWall);


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
            FixedUpdate();
            Character.Animancer.Play(_Animation);
            Debug.Log("Wallslide state entered");
            

        }

        private void FixedUpdate()
        {
            var body = Character.Body.Rigidbody2D;
            
            body.gravityScale = wallSlideGravity;
            if (body.velocity.y < -maxWallSlideSpeed)
            {
                body.velocity = new Vector2(body.velocity.x, -maxWallSlideSpeed);
            }
        }

        public override float MovementSpeedMultiplier => 1;

    }

}
