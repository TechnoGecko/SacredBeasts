using Animancer.FSM;
using UnityEngine;

namespace GameModes
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameState _townState;
        public GameState townState => townState;

        [SerializeField] private GameState _explorationState;
        public GameState explorationState => _explorationState;
        
        
    
        public readonly StateMachine<GameState>.WithDefault
            StateMachine = new StateMachine<GameState>.WithDefault();

        private void Awake()
        {
            // StateMachine.DefaultState = ;
        }

        private void Pause()
        {
            //TODO Pause menu logic will go here
        }
    }
}
