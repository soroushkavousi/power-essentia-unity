using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerBehavior : MonoBehaviour
{
    private static SceneManagerBehavior _instance = default;
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private SceneName _currentSceneName = default;

    public static SceneManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public SceneName CurrentSceneName
    {
        get
        {
            if (_currentSceneName == default)
                _currentSceneName = SceneManager.GetActiveScene().buildIndex.To<SceneName>();

            return _currentSceneName;
        }
    }

    private void Start()
    {
        //If run at custom scene.
        InitializeCurrentScene();
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

    private void InitializeCurrentScene()
    {
        if (_currentSceneName == SceneName.MISSION)
            MusicPlayerBehavior.Instance.AudioSource.Stop();
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
        if (_currentSceneName == SceneName.START)
            return;
        //GameSessionBehavior.Instance.ResetGame();
        if (MusicPlayerBehavior.Instance.AudioSource.isPlaying == false)
            MusicPlayerBehavior.Instance.AudioSource.Play();
        LoadScene(SceneName.START);
    }

    public void LoadCountry()
    {
        if (_currentSceneName == SceneName.COUNTRY)
            return;
        //GameSessionBehavior.Instance.ResetGame();
        if (MusicPlayerBehavior.Instance.AudioSource.isPlaying == false)
            MusicPlayerBehavior.Instance.AudioSource.Play();
        LoadScene(SceneName.COUNTRY);
    }

    public void LoadPreparation()
    {
        //if (_currentSceneName == SceneName.PREPARATION)
        //    return;
        ////GameSessionBehavior.Instance.ResetGame();
        //if (MusicPlayerBehavior.Instance.AudioSource.isPlaying == false)
        //    MusicPlayerBehavior.Instance.AudioSource.Play();
        //LoadScene(SceneName.PREPARATION);
    }

    public void LoadMission()
    {
        if (_currentSceneName == SceneName.MISSION)
            return;
        //GameSessionBehavior.Instance.ResetGame();
        Debug.Log($"Loading level 1.");
        MusicPlayerBehavior.Instance.AudioSource.Stop();
        LoadScene(SceneName.MISSION);
    }

    public void RestartCurrentScene()
    {
        LoadScene(_currentSceneName);
    }

    public void LoadScene(SceneName sceneName)
    {
        _currentSceneName = sceneName;
        SceneManager.LoadScene(sceneName.To<int>());
        MusicPlayerBehavior.Instance.StartAllSounds();
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
