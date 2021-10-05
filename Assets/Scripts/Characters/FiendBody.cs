using Animancer;
using UnityEngine;

namespace Characters
{
    
    [AddComponentMenu(Character.MenuPrefix + "Character Body 2D")]

    public class FiendBody : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _Rigidbody;
        public Rigidbody2D Rigidbody => _Rigidbody;

        [SerializeField] private Collider2D _Collider;
        public Collider2D Collider => _Collider;

        
        #if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Collider);
            gameObject.GetComponentInParentOrChildren(ref _Rigidbody);

            if (_Rigidbody != null && enabled)
            {
                if (_Rigidbody.bodyType != RigidbodyType2D.Dynamic)
                    _Rigidbody.bodyType = RigidbodyType2D.Dynamic;

                if (!_Rigidbody.simulated)
                {
                    _Rigidbody.simulated = true;
                }
            }
            #endif
            
            
        }
        
        

    }
}
