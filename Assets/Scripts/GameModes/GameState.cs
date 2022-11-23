using Animancer.FSM;
using UnityEngine;

namespace GameModes
{
    public class GameState : StateBehaviour, IOwnedState<GameState>, IPrioritizable
    {
        [SerializeField] private GameManager _gameManager;
        public GameManager gameManager => _gameManager;

        public StateMachine<GameState> OwnerStateMachine => _gameManager.StateMachine;

        private IPrioritizable _prioritize;
    
        public virtual float Priority { get; }

        
    }
}

