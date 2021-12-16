using Animancer;
using UnityEngine;
using Animancer.FSM;
using Characters.Brains;
using Characters.States;
using Combat;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        
        
        [Header("Animations")]
        [SerializeField] private CharacterAnimancerComponent _Animancer;

        public CharacterAnimancerComponent Animancer => _Animancer;

        [SerializeField] private CharacterState _Idle;

        public CharacterState Idle => _Idle;

        [SerializeField] private Health _Health;
        public Health Health => _Health;

        
        [Header("Physics")]
        [SerializeField] private PlayerBody _Body;
        public PlayerBody Body => _Body;

        [SerializeField] private PlayerInputBrain _Brain;
        public PlayerInputBrain Brain => _Brain;

        private Vector2 _InputDirection;

        public Vector2 InputDirection
        {
            get => _InputDirection;

            set => _InputDirection = value;
        }

        private bool _CanJump;

        private bool _LandingFromJump;
        public bool LandingFromJump { get; set; }

        private bool _HasJumped;
        public bool HasJumped { get; set; }

        public bool dashing = false;
        
       [SerializeField] private bool _canDash = true;

       public bool canDash
       {
           get { return _canDash;}

           set { _canDash = value; }
       }

        public bool hasDashed;
        [SerializeField, Range(0,2)] public float dashTimer = 0.2f;

        [SerializeField] private float _dashTimeLimit = 3f;
        
        public float dashTimeLimit => _dashTimeLimit;

        public bool CanJump
        {
            get => _CanJump;

            set
            {
                
            }
        }



            [SerializeField] private Vector2 _InitialVelocity;

        public Vector2 InitialVelocity
        {
            get => _InitialVelocity;
            set => _InitialVelocity = value;
        }
        

        private Vector2 _MovementDirection;
        public Vector2 MovementDirection
        {
            get => _MovementDirection;
            set
            {
                _MovementDirection.x = Mathf.Clamp(value.x, -1, 1);
                _MovementDirection.y = Mathf.Clamp(value.y, -1, 1);
            }
        }

        /// <summary>The horizontal direction this character wants to move.</summary>
        public float MovementDirectionX
        {
            get => _MovementDirection.x;
            set => _MovementDirection.x = Mathf.Clamp(value, -1, 1);
        }

        /// <summary>The vertical direction this character wants to move.</summary>
        public float MovementDirectionY
        {
            get => _MovementDirection.y;
            set => _MovementDirection.y = Mathf.Clamp(value, -1, 1);
        }
        

        public readonly StateMachine<CharacterState>.WithDefault
            StateMachine = new StateMachine<CharacterState>.WithDefault();

        private void Awake()
        {
            StateMachine.DefaultState = _Idle;
        }
        
        
        
    }
}
