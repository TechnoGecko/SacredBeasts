using System;
using Animancer;
using UnityEngine;

namespace Characters.Brains
{
    
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public class CharacterBrain : MonoBehaviour
    {
       

        /// <summary>Run inputs before everything else.</summary>
        public const int DefaultExecutionOrder = -10000;
        
        
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
