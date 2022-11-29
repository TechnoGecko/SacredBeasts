using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameModes
{
    public class ExplorationState : GameState
    {
        public override string sceneName => "Exploration";
        public override void OnEnterState()
        {
            base.OnEnterState();
            SceneManager.LoadSceneAsync(sceneName);

        }

        public override void OnExitState()
        {
            base.OnExitState();
        }
    }
}
