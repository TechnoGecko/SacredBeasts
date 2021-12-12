using System;
using PlatformerGameKit;
using UnityEngine;
using Combat;


namespace Characters.States.Attack
{
    public class PlayerAttackState : AttackState
    {
        [SerializeField] private AttackTransition[] _GroundAnimations;

        [SerializeField] private AttackTransition[] _GroundUpAnimations;

        
        private int _CurrentIndex;
        
        private bool _Combo;
        
        
        private void Awake()
        {
            Action onEnd = OnAnimationEnd;

            for (int i = 0; i < _GroundAnimations.Length; i++)
            {
                _GroundAnimations[i].Events.OnEnd += onEnd;


                Character.Body.OnGroundedChanged += OnGroundedChanged;
            }
        }

        private void OnGroundedChanged(bool isGrounded)
        {
            if (Character.StateMachine.CurrentState == this)
                Character.StateMachine.ForceSetDefaultState();
        }


        public override void OnEnterState()
        {
            base.OnEnterState();
            _CurrentIndex = 0;
            _Combo = false;
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            Character.Animancer.EndHitSequence(); 
            var animation = CurrentAnimations[_CurrentIndex];
            Character.Animancer.Play(animation);
        }

        private AttackTransition[] CurrentAnimations
        {
            get
            {
                if (Character.MovementDirectionY > 0.5f && _GroundUpAnimations.Length > 0)
                        return _GroundUpAnimations;
                else
                    return _GroundAnimations;
            }
        }

        public override bool CanExitState
        {
            get
            {
                if (Character.StateMachine.NextState == this)
                    _Combo = true;

                return false;
            }
        }

        private void OnAnimationEnd()
        {
            if (_Combo)
            {
                _Combo = false;
                _CurrentIndex++;

                if (_CurrentIndex < CurrentAnimations.Length)
                {
                    PlayAnimation();
                    return;
                }
            }

            Character.StateMachine.ForceSetDefaultState();
        }

    }
}
