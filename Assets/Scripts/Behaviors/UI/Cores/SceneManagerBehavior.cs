using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerBehavior : MonoBehaviour
{
    private static SceneManagerBehavior _instance = default;
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private Observable<SceneName> _currentSceneName = new(ignoreRepetition: false);

    public static SceneManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public Observable<SceneName> CurrentSceneName => _currentSceneName;

    private void Awake()
    {
        _currentSceneName.Value = SceneManager.GetActiveScene().buildIndex.To<SceneName>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        CloseOnBackButtonPressed();
    }

    private void CloseOnBackButtonPressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    //IEnumerator InitializeGame()
    //{
    //    Debug.Log("Initializing the game...");
    //    yield return new WaitForSeconds(3);
    //    Debug.Log("Game Initialized.");
    //    LoadStart();
    //}

    public void LoadStart()
    {
        if (_currentSceneName.Value == SceneName.START)
            return;
        LoadScene(SceneName.START);
    }

    public void LoadCountry()
    {
        if (_currentSceneName.Value == SceneName.COUNTRY)
            return;
        LoadScene(SceneName.COUNTRY);
    }

    public void LoadMission()
    {
        if (_currentSceneName.Value == SceneName.MISSION)
            return;
        LoadScene(SceneName.MISSION);
    }

    public void RestartCurrentScene()
    {
        LoadScene(_currentSceneName.Value);
    }

    public void LoadScene(SceneName sceneName)
    {
        SceneManager.LoadScene(sceneName.To<int>());
        _currentSceneName.Value = sceneName;
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
