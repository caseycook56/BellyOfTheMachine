using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    #region Structs
    public struct _SceneData
    {
        public _SceneData(string t, Vector3 p)
        {
            Title = t;
            StartPosition = p;
        }
        public bool CompareTo(string other)
        {
            return Title.CompareTo(other) == 0;
        }
        public string Title;
        public Vector3 StartPosition;
    }
    public struct ControllerState
    {
        public int CurrentScene;
    }
    #endregion
    
    private List<_SceneData> SceneOrder = new List<_SceneData>();
    private ControllerState State;
    private bool GameRunning = false;
    private GameObject Player;
    private TimeManager _TimeManager;
    public static event Action ResetGame;
    public static event Action SceneChange;

    #region UI
    public AsyncOperation UIOperation;
    private GameObject UIPlayer;
    private GameObject UIPause;
    private GameObject UIGameOver;
    private GameObject UIControls;
    private GameObject UIStartMenu;
    private GameObject UIEventSystem;
    private UIController _UIController;

    private int i = 0;

    private void LoadUI()
    {
        UIControls = GameObject.FindWithTag("UIControls");
        UIPause = GameObject.Find("UIPause");
        UIGameOver = GameObject.Find("UIGameOver");
        UIPlayer = GameObject.Find("UIPlayer");
        _UIController = GameObject.Find("UIController").GetComponent<UIController>();
        UIStartMenu = GameObject.Find("UIStart");
        UIEventSystem = GameObject.Find("UIEventSystem");

        _UIController.AddAbilities(Player);
        StartMenuState();
    }
    private void StartMenuState()
    {
        UIStartMenu.SetActive(true);
        UIControls.SetActive(false);
        UIPause.SetActive(false);
        UIGameOver.SetActive(false);
        UIPlayer.SetActive(false);
    }
    private void PauseMenuState()
    {
        UIPause.SetActive(!UIPause.activeSelf);
        UIControls.SetActive(false);
        UIGameOver.SetActive(false);
        UIStartMenu.SetActive(false);
        UIPlayer.SetActive(!UIPause.activeSelf);
    }
    private void ControlsMenuState()
    {
        UIControls.SetActive(!UIControls.activeSelf);
        UIPause.SetActive(false);
        UIGameOver.SetActive(false);
        UIStartMenu.SetActive(false);
        UIPlayer.SetActive(!UIControls.activeSelf);
    }
    private void PlayMenuState()
    {
        UIPlayer.SetActive(true);
        UIControls.SetActive(false);
        UIPause.SetActive(false);
        UIGameOver.SetActive(false);
        UIStartMenu.SetActive(false);
    }
    private void GameOverMenuState()
    {
        UIGameOver.SetActive(true);
        UIControls.SetActive(false);
        UIPause.SetActive(false);
        UIStartMenu.SetActive(false);
        UIPlayer.SetActive(false);
    }
    private IEnumerator LoadUIScene(Action callback)
    {
        UIOperation = SceneManager.LoadSceneAsync("MasterUI", LoadSceneMode.Additive);
        while (!UIOperation.isDone)
        {
            yield return null;
        }
        if (callback != null)
        {
            callback();
        }
    }

    #region PlayerManagement
    public AsyncOperation SceneChangeOperation;
    private void ResetPlayerState()
    {
        Player.TryGetComponent<PhysicsBody>(out PhysicsBody body);
        if (body != null)
        {
            body.points.Clear();
            body.UpdateVelocity(Vector3.zero, 0);
            body.transform.position = SceneOrder[State.CurrentScene].StartPosition;
        }
    }
    #endregion

    #endregion
    #region Callbacks 
    public void OnStart()
    {
        _TimeManager.ResetTimeScale();

        // unload the start scene
        UnloadScene(State.CurrentScene);

        // load the first scene
        StartCoroutine(LoadScene(1, ResetPlayerState));
        PlayMenuState();

        State.CurrentScene = 1;

    }
    public void OnGameOver()
    {
        GameOverMenuState();
    }
    public void OnRestart()
    {
        ResetGame?.Invoke();
        OnStart();        
    }
    public void OnPause()
    {
        if (State.CurrentScene != 0)
        {
            ControlsMenuState();
        }
        if (GameObject.FindGameObjectWithTag("Tutorial") != null)
        {
            i++;
        }
        if (i > 2)
        {
            Destroy(GameObject.FindGameObjectWithTag("Tutorial"));
            PlayMenuState();
            i = 0;
        }

    }

    public void OnEscape()
    {
        if (State.CurrentScene != 0)
        {
            PauseMenuState();
        }
        if (GameObject.FindGameObjectWithTag("Tutorial") != null)
        {
            i++;
        }
        if (i > 2)
        {
            Destroy(GameObject.FindGameObjectWithTag("Tutorial"));
            PlayMenuState();
            i = 0;
        }

    }
    #endregion

    private void Awake()
    {
        // setup the scene manager singleton
        // load the first scene and teleport the player to its starting position
        if (!GameRunning)
        {
            instance = this;
            Player = GameObject.Find("Player");
            _TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

            // Load MasterUI scene where all UI prefabs exist
            StartCoroutine(LoadUIScene(LoadUI));

            // scene transition order
            ConfigureSceneOrder();

            StartCoroutine(LoadScene(0, ResetPlayerState));

            RegisterCallbacks();

            GameRunning = true;
        }
    }
    private void RegisterCallbacks()
    {
        StartGame.StartButton += OnStart;
        Health.GameOver += OnGameOver;
        RestartGame.Restart += OnRestart;
    }
    private void ConfigureSceneOrder()
    {
        SceneOrder.Add(new _SceneData("Start", new Vector3(-5.364f, 5.547f, .511f)));
        SceneOrder.Add(new _SceneData("Fluid", new Vector3(5.121f, 1.08f, -2.185f)));
        SceneOrder.Add(new _SceneData("Vents", new Vector3(6.19f, 3.85f, 0.95f)));
        SceneOrder.Add(new _SceneData("Jail", new Vector3(6.4f, 0.217f, 1.169f)));
        SceneOrder.Add(new _SceneData("LightScene", new Vector3(-5.77f, 5.64f, 0.52f)));
    }

    #region Public Scene Management 
    public void LoadNextScene(string CurrentScene)
    {
        int CurrentSceneIndex = SceneOrder.FindIndex(s => s.CompareTo(CurrentScene));
        int NextSceneIndex = CurrentSceneIndex + 1;
        if (SceneOrder.Count - 1 >= NextSceneIndex)
        {
            // Debugging!
            ResetPlayerState();
            // 
            StartCoroutine(LoadScene(NextSceneIndex, ResetPlayerState));
            UnloadScene(CurrentSceneIndex);
        }
        else
        {
            Debug.LogError("Trying to access a scene that does not exist", this);
        }
    }
    private IEnumerator LoadScene(int scene, Action callback)
    {
        SceneChangeOperation = SceneManager.LoadSceneAsync(SceneOrder[scene].Title, LoadSceneMode.Additive);
        while (!SceneChangeOperation.isDone)
        {
            yield return null;
        }
        State.CurrentScene = scene;

        // used to reset player data
        if (callback != null)
        {
            callback();
        }
    }
    public void EnableCamera()
    {
        // used by the camera to turn on once the player has data / position been reset
        SceneChange?.Invoke();
    }
    public void UnloadScene(int scene)
    {
        StartCoroutine(Unload(scene));
    }
    private IEnumerator Unload(int scene)
    {
        yield return null;
        SceneManager.UnloadSceneAsync(SceneOrder[scene].Title);
    }
    #endregion
}
