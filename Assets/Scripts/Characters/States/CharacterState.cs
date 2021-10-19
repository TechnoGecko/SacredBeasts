using Animancer.FSM;
using UnityEngine;

namespace Characters.States
{
    public class CharacterState : StateBehaviour, IOwnedState<CharacterState>, IPrioritizable
    {
        public virtual float MovementSpeedMultiplier => 0;

        [SerializeField] private Character _Character;
        public Character Character => _Character;

        public StateMachine<CharacterState> OwnerStateMachine => _Character.StateMachine;

        private IPrioritizable _prioritize;


        public virtual bool CanTurn => MovementSpeedMultiplier != 0;
        public virtual float Priority { get; }

    }
}
