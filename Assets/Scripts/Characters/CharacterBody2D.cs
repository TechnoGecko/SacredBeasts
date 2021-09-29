using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using System;
using Characters.Brains;
using PlatformerGameKit.Characters;

namespace Characters {
    
    [AddComponentMenu(Character.MenuPrefix + "Character Body 2D")]
    
    public class CharacterBody2D : MonoBehaviour
    {
        public const int DefaultExecutionOrder = CharacterBrain.DefaultExecutionOrder + 1000;

        [SerializeField] private Collider2D _Collider;

        public Collider2D Collider => _Collider;


        [SerializeField] private Rigidbody2D _Rigidbody;

        public Rigidbody2D Rigidbody => _Rigidbody;
        
#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Collider);
            gameObject.GetComponentInParentOrChildren(ref _Rigidbody);

            // Ensure that the Rigidbody is configured correctly.
            // Only set the values if they are actually different. Otherwise this can cause issues on prefabs.
            if (_Rigidbody != null && enabled)
            {
                if (_Rigidbody.bodyType != RigidbodyType2D.Dynamic)
                    _Rigidbody.bodyType = RigidbodyType2D.Dynamic;

                if (!_Rigidbody.simulated)
                    _Rigidbody.simulated = true;
            }
        }
#endif


        public Vector2 Position
        {
            get => _Rigidbody.position;

            set => _Rigidbody.position = value;
        }

        public Vector2 Velocity
        {
            get => _Rigidbody.velocity;

            set => _Rigidbody.velocity = value;
        }

        public float Rotation
        {
            get => 0; //_Rigidbody.rotation;
            
            set => throw new NotSupportedException("Rotation is not supported.");// _Rigidbody.rotation = value;

        }

        public virtual Vector2 Gravity => Physics2D.gravity * _Rigidbody.gravityScale;

        private bool _IsGrounded;
        public bool IsGrounded
        {
            get => _IsGrounded;

            set
            {
                if (_IsGrounded == value)
                    return;

                _IsGrounded = value;
                OnGroundedChanged?.Invoke(value);
            }
        }

        public event Action<bool> OnGroundedChanged;


        private ContactFilter2D _TerrainFilter;

        public ContactFilter2D TerrainFilter => _TerrainFilter;


        public virtual float GripAngle
        {
            get => 0;
            set => throw new NotSupportedException($"Can't set {GetType().FullName}.{nameof(GripAngle)}.");
        }

        public virtual float StepHeight
        {
            get => 0;
            set => throw new NotSupportedException($"Can't set {GetType().FullName}.{nameof(StepHeight)}.");
        }

        public virtual PlatformContact2D GroundContact => default;


        protected virtual void Awake()
        {
            _TerrainFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        }

        protected virtual void OnDisable()
        {
            IsGrounded = false;
        }

#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Displays some non-serialized details at the bottom of the Inspector.</summary>
        /// <example>
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/pro">Inspector Gadgets Pro</see> would allow this to
        /// be implemented much easier by simply placing
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/docs/script-inspector/inspectable-attributes">
        /// Inspectable Attributes</see> on the properties we want to display like so:
        /// <para></para><code>
        /// [Inspectable]
        /// public bool IsGrounded ...
        /// </code>
        /// </example>
        [UnityEditor.CustomEditor(typeof(CharacterBody2D), true)]
        public class Editor : UnityEditor.Editor
        {
            /************************************************************************************************************************/

            /// <inheritdoc/>
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!UnityEditor.EditorApplication.isPlaying)
                    return;

                using (new UnityEditor.EditorGUI.DisabledScope(true))
                {
                    var target = (CharacterBody2D)this.target;
                    UnityEditor.EditorGUILayout.Toggle("Is Grounded", target.IsGrounded);
                }
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
#endif

    }
}
