using System;
using Animancer;
using UnityEngine;

namespace Characters.Brains
{
    public class CharacterBrain : MonoBehaviour
    {
        
        
        [SerializeField] private Character _Character;
        public ref Character Character => ref _Character;
        
        #if UNITY_EDITOR

        protected void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
        #endif
    }
}
