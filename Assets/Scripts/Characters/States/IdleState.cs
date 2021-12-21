using Animancer;

using UnityEngine;

namespace Characters.States
{
   

    public class IdleState : CharacterState
    {
        [SerializeField] private ClipTransition _Animation;
        
        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _Animation?.Clip.EditModeSampleAnimation(Character);
        }
        #endif


        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(_Animation);
        }
    }

}