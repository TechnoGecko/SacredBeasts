using System;
using Animancer;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    
      
    
    public sealed class CharacterAnimancerComponent : AnimancerComponent
    {
        private static NewCharacter2DController _Controller;

        private bool _facingRight = _Controller.facingRight;
        
        [SerializeField]
        private SpriteRenderer _Renderer;
        public SpriteRenderer Renderer => _Renderer;

        [SerializeField]
        private Character _Character;
        public Character Character => _Character;
        
        
        void Flip()
        {
            _facingRight = !_facingRight;
            transform.rotation = Quaternion.Euler(0, _facingRight ? 0 : 180, 0);
        }

        private void Update()
        {
            throw new NotImplementedException();
        }
    }
    
    
    
}
