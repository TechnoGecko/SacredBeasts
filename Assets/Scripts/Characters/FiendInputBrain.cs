using System.Collections;
using System.Collections.Generic;
using Characters.Brains;
using Characters.States;
using UnityEngine;

namespace Characters {
    
    
public class FiendInputBrain : CharacterBrain
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

    private CharacterState _CurrentJumpState;
    /*
    [SerializeField] private CharacterState _SecondaryAttack;
    */

    [Header("Movement")]
   
    public Vector2 direction;
    

    [Header("Jump")]
    public float jumpDelay = 0.25f;
    
    private float _jumpTimer;
    public float jumpTimer => _jumpTimer;

    public bool canJump { get; }

    [Header("WallJump")]
    [SerializeField] float wallJumpDelay = 0.35f;
    private float _wallJumpTimer;
    public float wallJumpTimer => _wallJumpTimer;
    
    
    private void Update()
    {
        direction = new Vector2(Input.GetAxisRaw(_XAxisName), Input.GetAxisRaw(_YAxisName));

        if (Input.GetButtonDown(_JumpButton) && Character.StateMachine.TrySetState(_Jump))
            _CurrentJumpState = Character.StateMachine.CurrentState;

        if (_CurrentJumpState == Character.StateMachine.CurrentState && Input.GetButtonUp(_JumpButton))
            Character.StateMachine.TrySetDefaultState();
        {
            _jumpTimer = Time.time + jumpDelay;
            _wallJumpTimer = Time.time + wallJumpDelay;
            
            
        } 
    }
}

}
