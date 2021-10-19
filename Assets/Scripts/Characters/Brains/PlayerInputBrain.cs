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

        [Header("Actions")]
        [SerializeField] private CharacterState _Jump;
        [SerializeField] private CharacterState _Attack;

        private CharacterState _CurrentJumpState;

        [SerializeField]private bool _IsHoldingJump;
        public bool IsHoldingJump => _IsHoldingJump;

        

        
        private void Update()
        {
            Character.InputDirection = new Vector2(Input.GetAxisRaw(_XAxisName), Input.GetAxisRaw(_YAxisName));

            _IsHoldingJump = Input.GetButton(_JumpButton);
            
            if (_Jump != null)
            {
                if (Input.GetButtonDown(_JumpButton) && Character.StateMachine.TrySetState(_Jump))
                    _CurrentJumpState = Character.StateMachine.CurrentState;

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
    }
    
    
}
