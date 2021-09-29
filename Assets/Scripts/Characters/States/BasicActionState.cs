using UnityEngine;
using Animancer;
using Characters.States.Attack;


namespace Characters.States{
    
    
    public class BasicActionState : AttackState
    {

        [SerializeReference] private ITransitionWithEvents _Animation;

        
        protected virtual void Awake()
        {
            _Animation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;

        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(_Animation);
        }
        
        
    }
}