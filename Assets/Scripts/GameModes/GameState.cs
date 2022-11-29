using Animancer.FSM;
using UnityEngine;

namespace GameModes
{
    public class GameState : StateBehaviour, IState, IPrioritizable
    {
        [SerializeField] private LevelManager _levelManager;
        public LevelManager levelManager => _levelManager;

        //public StateMachine<GameState> OwnerStateMachine => _levelManager.StateMachine;

        private IPrioritizable _prioritize;
    
        public virtual float Priority { get; }

        public virtual string sceneName => "";

    }
}

