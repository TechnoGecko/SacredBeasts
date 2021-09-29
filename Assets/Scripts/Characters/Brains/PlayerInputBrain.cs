using System;
using System.Collections;
using System.Collections.Generic;
using Characters.States;
using UnityEngine;

namespace Characters.Brains 
{
    [AddComponentMenu(MenuPrefix + "Player Input Brain")]

    public class PlayerInputBrain : CharacterBrain
    {
        [Header("Input Names")]

        [SerializeField]
        [Tooltip("Space by default")]
        private string _JumpButton = "Jump";

        [SerializeField]
        [Tooltip("Left Click by default")]
        private string _PrimaryAttackButton = "Fire1";

        [SerializeField]
        [Tooltip("Right Click by default")]
        private string _SecondaryAttackButton = "Fire2";

        [SerializeField]
        [Tooltip("Left Shift by default")]
        private string _RunButton = "Fire3";

        [SerializeField]
        [Tooltip("A/D and Left/Right Arrows by default")]
        private string _XAxisName = "Horizontal";

        [SerializeField]
        [Tooltip("W/S and Up/Down Arrows by default")]
        private string _YAxisName = "Vertical";

        [Header("Actions")]
        [SerializeField] private CharacterState _Jump;
        [SerializeField] private CharacterState _PrimaryAttack;
        /*
        [SerializeField] private CharacterState _SecondaryAttack;
        */

        private CharacterState _CurrentJumpState;

        private void Update()
        {
            if (_Jump != null)
            {
                if (Input.GetButtonDown(_JumpButton) &&
                    Character.StateMachine.TrySetState(_Jump))
                    _CurrentJumpState = Character.StateMachine.CurrentState;

                if (_CurrentJumpState == Character.StateMachine.CurrentState &&
                    Input.GetButtonUp(_JumpButton))
                    Character.StateMachine.TrySetDefaultState();
            }

            if (_PrimaryAttack != null && Input.GetButtonDown(_PrimaryAttackButton))
                Character.StateMachine.TryResetState(_PrimaryAttack);
            
            /*if (_SecondaryAttack != null && Input.GetButtonDown(_SecondaryAttackButton))
                Character.StateMachine.TryResetState(_SecondaryAttack);*/

            Character.Run = Input.GetButton(_RunButton);

            Character.MovementDirection = new Vector2(
                Input.GetAxisRaw(_XAxisName),
                Input.GetAxisRaw(_YAxisName));
        }
        
    }
    

}
