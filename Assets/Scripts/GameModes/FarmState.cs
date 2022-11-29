using UnityEngine.SceneManagement;

namespace GameModes
{
    public class FarmState : GameState
    {
        public override string sceneName => "Farm";

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
