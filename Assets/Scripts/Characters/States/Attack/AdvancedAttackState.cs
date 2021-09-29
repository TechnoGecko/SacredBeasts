using System;
using System.Collections;
using System.Collections.Generic;
using Animancer.Examples.Basics;
using PlatformerGameKit;
using UnityEngine;


namespace Characters.States.Attack
{
    [AddComponentMenu(MenuPrefix + "Advanced Attack State")]


    public sealed class AdvancedAttackState : AttackState
    {
        const string TooltipPrefix = "The animation combo used when the character is";

        [SerializeField]
        [Tooltip(TooltipPrefix + "grounded")]
        private AttackTransition[] _GroundAnimations;

        [SerializeField]
        [Tooltip(TooltipPrefix + "grounded and trying to move upwards")]
        private AttackTransition[] _GroundUpAnimations;

        [SerializeField]
        [Tooltip(TooltipPrefix + "airborne")]
        private AttackTransition[] _AirAnimations;

        [SerializeField]
        [Tooltip(TooltipPrefix + "airborne and trying to move upwards")]
        private AttackTransition[] _AirUpAnimations;

        private int _CurrentIndex;
        private bool _Combo;


        private void Awake()
        {
            Action onEnd = OnAnimationEnd;

            for (int i = 0; i < _GroundAnimations.Length; i++)
                _GroundAnimations[i].Events.OnEnd += onEnd;
            for (int i = 0; i < _GroundUpAnimations.Length; i++)
                _GroundUpAnimations[i].Events.OnEnd += onEnd;

            for (int i = 0; i < _AirAnimations.Length; i++)
                _AirAnimations[i].Events.OnEnd += onEnd;

            for (int i = 0; i < _AirUpAnimations.Length; i++)
                _AirUpAnimations[i].Events.OnEnd += onEnd;

            Character.Body.OnGroundedChanged += OnGroundedChanged;
            
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
                if (Character.Body.IsGrounded)
                {
                    if (Character.MovementDirectionY > 0.5f && _GroundUpAnimations.Length > 0)
                        return _GroundUpAnimations;
                    else
                        return _GroundAnimations;
                    
                }
                else
                {
                    if (Character.MovementDirectionY > 0.5f && _AirUpAnimations.Length > 0)
                        return _AirUpAnimations;
                    else
                        return _AirAnimations;
                }
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