using Animancer.FSM;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameModes
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

         [SerializeField] private GameObject _loaderCanvas;
         [SerializeField] private Image _progressBar;
        
        // [SerializeField] private GameState _farmState;
        // public GameState farmState => _farmState;
        //
        // [SerializeField] private GameState _explorationState;
        // public GameState explorationState => _explorationState;
        //
        // [SerializeField] private GameState _bossFightState;
        // public GameState bossFightState => _bossFightState;
        //
        //
        //
        // public readonly StateMachine<GameState>.WithDefault
        //     StateMachine = new StateMachine<GameState>.WithDefault();

        private void Awake()
        {
            _loaderCanvas.SetActive(false);
            _progressBar.fillAmount = 0.0f;
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
             //StateMachine.DefaultState = _farmState;
        }

        private void Update()
        {
            if (Input.GetKey("z"))
            {
                LoadScene("Farm");
            } else if (Input.GetKey("x"))
            {
                LoadScene("Exploration");
            }
            else if (Input.GetKey("c"))
            {
                LoadScene("BossFight");
            }
        }

        private void Pause()
        {
            //TODO Pause menu logic will go here
        }

        public async void LoadScene(string newScene)
        {
            var scene = SceneManager.LoadSceneAsync(newScene);
            scene.allowSceneActivation = false;
            _loaderCanvas.SetActive(true);

            do
            {
                _progressBar.fillAmount = scene.progress;
            } while (scene.progress < 0.9f);

            scene.allowSceneActivation = true;
            _loaderCanvas.SetActive(false);
            // StateMachine.TrySetState(newScene);
        }
        
    }
}
