using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameModes
{
    public class BossState : GameState
    {
        public override string sceneName => "Boss";
        public override void OnEnterState()
        {
            base.OnEnterState();
            
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }
    }
}
