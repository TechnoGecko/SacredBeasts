using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using Animancer.Units;
using static Animancer.Validate;
using PlatformerGameKit;
using PlatformerUtilities = Utilities.PlatformerUtilities;

namespace Characters.States.Jump {
    
    [AddComponentMenu(MenuPrefix + "Hold Jump State")]

    public class HoldJumpState : JumpState
    {
        [SerializeField, MetersPerSecond(Rule = Validate.Value.IsNotNegative)]
        [Tooltip("The continuous acceleration applied while holding the jump button")]
        private float _HoldAcceleration = 40;

        public float HoldAcceleration => _HoldAcceleration;
        
        
        #if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _HoldAcceleration);
        }
        #endif


        protected virtual void FixedUpdate()
        {
            Character.Body.Velocity += new Vector2(0, _HoldAcceleration * Time.deltaTime);
        }
    }

}