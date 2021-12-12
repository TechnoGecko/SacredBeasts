using UnityEngine;
using Animancer;
using System.Collections.Generic;
using Hit = Combat.Hit;
using HitData = Combat.HitData;
using HitTrigger = Combat.HitTrigger;


namespace Characters
{
    public sealed class CharacterAnimancerComponent : AnimancerComponent
    {
        [SerializeField]
        private SpriteRenderer _Renderer;

        public SpriteRenderer Renderer => _Renderer;

        [SerializeField] private Character _Character;
        public Character Character => _Character;
        
        #if UNITY_EDITOR

        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Renderer);
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
        #endif
        
        #if UNITY_ASSERTIONS

        private void Awake()
        {
            DontAllowFade.Assert(this);
        }
        #endif

        public bool FacingLeft
        {
            get => _Renderer.flipX;
            set => _Renderer.flipX = value;
        }

        public float FacingX
        {
            get => _Renderer.flipX ? -1f : 1f;
            set
            {
                if (value != 0)
                    _Renderer.flipX = value < 0;
            }
        }

        public Vector2 Facing
        {
            get => new Vector2(FacingX, 0);
            set => FacingX = value.x;
        }

        private void Update()
        {
            if(Character.StateMachine.CurrentState.CanTurn)
                Facing = Character.MovementDirection;
        }

        public static CharacterAnimancerComponent GetCurrent() => Get(AnimancerEvent.CurrentState);

        public static CharacterAnimancerComponent Get(AnimancerNode node) => Get(node.Root);

        public static CharacterAnimancerComponent Get(AnimancerPlayable animancer) =>
            animancer.Component as CharacterAnimancerComponent;
        
        #region Hit Boxes


        private Dictionary<HitData, HitTrigger> _ActiveHits;
        private HashSet<Hit.ITarget> _IgnoreHits;


        public void AddHitBox(HitData data)
        {
            if (_IgnoreHits == null)
            {
                ObjectPool.Acquire(out _ActiveHits);
                ObjectPool.Acquire(out _IgnoreHits);
                
            }
            _ActiveHits.Add(data, HitTrigger.Activate(Character, data, FacingLeft, _IgnoreHits));
            
        }

        public void RemoveHitBox(HitData data)
        {
            if (_ActiveHits.TryGetValue(data, out var trigger))
            {
                trigger.Deactivate();
                _ActiveHits.Remove(data);
            }
        }

        public void EndHitSequence()
        {
            if (_IgnoreHits == null)
                return;

            ClearHitBoxes();
            ObjectPool.Release(ref _ActiveHits);
            ObjectPool.Release(ref _IgnoreHits);
        }

        public void ClearHitBoxes()
        {
            if (_ActiveHits != null)
            {
                foreach (var trigger in _ActiveHits.Values)
                    trigger.Deactivate();
                _ActiveHits.Clear();
            }
        }

        protected override void OnDisable()
        {
            EndHitSequence();
            base.OnDisable();
        }

        #endregion

    }
}
