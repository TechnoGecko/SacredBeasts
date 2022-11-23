using System.Collections;
using Characters.States;
using TMPro;
using UnityEngine;

namespace Characters.Brains
{
    public class PlayerInputBrain : CharacterBrain
    {
        
        
        [SerializeField] private string _JumpButton = "Jump";

        [SerializeField] private string _PrimaryAttackButton = "Fire1";

        [SerializeField] private string _XAxisName = "Horizontal";

        [SerializeField] private string _YAxisName = "Vertical";

        [SerializeField] private string _DashButtonName = "Fire3";

        
        
        [Header("Actions")]
        [SerializeField] private CharacterState _Jump;
        [SerializeField] private CharacterState _Attack;

        private CharacterState _CurrentJumpState;

        [SerializeField]private bool _IsHoldingJump;
        public bool IsHoldingJump => _IsHoldingJump;

        private float _JumpTimer;
        public float JumpTimer => _JumpTimer;

        private float _JumpDelay = 0.25f;

        
        private void Update()
        {
            Character.InputDirection = new Vector2(Input.GetAxisRaw(_XAxisName), Input.GetAxisRaw(_YAxisName));

            _IsHoldingJump = Input.GetButton(_JumpButton);
            
            if (_Jump != null)
            {
                if (Input.GetButtonDown(_JumpButton))
                    _JumpTimer = Time.time + _JumpDelay;
                
                if (_JumpTimer > Time.time && Character.StateMachine.TrySetState(_Jump))
                    _CurrentJumpState = Character.StateMachine.CurrentState;

                if (Input.GetButtonDown(_DashButtonName))
                    StartCoroutine(Dash());

                if (_CurrentJumpState == Character.StateMachine.CurrentState &&
                    Input.GetButtonUp(_JumpButton))
                    Character.StateMachine.TrySetDefaultState();

                if (_Attack != null && Input.GetButtonDown(_PrimaryAttackButton))
                    Character.StateMachine.TryResetState(_Attack);

                
                
                
                
                Character.MovementDirection = new Vector2(
                    Input.GetAxisRaw(_XAxisName),
                    Input.GetAxisRaw(_YAxisName));

                
            }

            

        }

        private IEnumerator Dash()
        {
            Character.dashing = true;

            yield return new WaitForSeconds(Character.dashTimer);

            Character.dashing = false;

            Character.canDash = false;
            

            yield return new WaitForSeconds(Character.dashTimeLimit);

            Character.canDash = true;
            

            yield break;
        }
    }
    
    
}
