using Animancer;
using Animancer.FSM;
using Characters.States;
using Combat;
using UnityEngine;
using Combat;

namespace Characters
{
    [AddComponentMenu(MenuPrefix + "Character")]
    public class Character : MonoBehaviour
    {
    
    
    
        /// <summary>The menu prefix for <see cref="AddComponentMenu"/>.</summary>
        public const string MenuPrefix = Strings.MenuPrefix + "Characters/";
    
        [SerializeField]
        private CharacterAnimancerComponent _Animancer;
        public CharacterAnimancerComponent Animancer => _Animancer;

        [SerializeField]
        private CharacterBody2D _Body;
        public CharacterBody2D Body => _Body;

        [SerializeField]
        private Health _Health;
        public Health Health => _Health;

        [SerializeField]
        private CharacterState _Idle;
        public CharacterState Idle => _Idle;


        protected virtual void OnValidate()
        { 
            gameObject.GetComponentInParentOrChildren(ref _Animancer);
            gameObject.GetComponentInParentOrChildren(ref _Body);
            gameObject.GetComponentInParentOrChildren(ref _Health);
            gameObject.GetComponentInParentOrChildren(ref _Idle);
        }

        private Vector2 _MovementDirection;

        /// <summary>The direction this character wants to move.</summary>
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

        public bool Run { get; set; }
        
        

        public readonly StateMachine<CharacterState>.WithDefault
            StateMachine = new StateMachine<CharacterState>.WithDefault();

        protected virtual void Awake()
        {
            StateMachine.DefaultState = _Idle;

#if UNITY_ASSERTIONS
            // Add your name to all children to make debugging easier.
            foreach (Transform child in transform)
                child.name = $"{child.name} ({name})";
#endif
        }
        
        #if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Displays some non-serialized details at the bottom of the Inspector.</summary>
        /// <example>
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/pro">Inspector Gadgets Pro</see> would allow this to
        /// be implemented much easier by simply placing
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/docs/script-inspector/inspectable-attributes">
        /// Inspectable Attributes</see> on the fields and properties we want to display like so:
        /// <para></para><code>
        /// [Inspectable]
        /// public Vector2 MovementDirection ...
        /// 
        /// [Inspectable]
        /// public bool Run { get; set; }
        /// 
        /// [Inspectable]
        /// public CharacterState CurrentState
        /// {
        ///     get => StateMachine.CurrentState;
        ///     set => StateMachine.TrySetState(value);
        /// }
        /// </code>
        /// </example>
        [UnityEditor.CustomEditor(typeof(Character), true)]
        public class Editor : UnityEditor.Editor
        {
            /************************************************************************************************************************/

            /// <inheritdoc/>
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!UnityEditor.EditorApplication.isPlaying)
                    return;

                var target = (Character)this.target;

                target.MovementDirection = UnityEditor.EditorGUILayout.Vector2Field("Movement Direction", target.MovementDirection);

                target.Run = UnityEditor.EditorGUILayout.Toggle("Run", target.Run);

                UnityEditor.EditorGUI.BeginChangeCheck();
                var state = UnityEditor.EditorGUILayout.ObjectField(
                    "Current State", target.StateMachine.CurrentState, typeof(CharacterState), true);
                if (UnityEditor.EditorGUI.EndChangeCheck())
                    target.StateMachine.TrySetState((CharacterState)state);
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
#endif

    }
}