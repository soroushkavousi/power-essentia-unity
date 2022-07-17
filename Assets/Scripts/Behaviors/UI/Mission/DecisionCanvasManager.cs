using TMPro;
using UnityEngine;

[RequireComponent(typeof(InteractionLayerPageBehavior))]
[RequireComponent(typeof(AudioSource))]
public class DecisionCanvasManager : MonoBehaviour
{
    private static DecisionCanvasManager _instance = default;
    [SerializeField] private DecisionCanvasStaticData _staticData = default;
    [SerializeField] private GameObject _winCanvas = default;
    [SerializeField] private TextMeshProUGUI _winTitle = default;
    [SerializeField] private GameObject _loseCanvas = default;
    [SerializeField] private TextMeshProUGUI _loseTitle = default;
    [SerializeField] private ButtonBehavior _goNextLevelButton = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private InteractionLayerPageBehavior _interactionLayerPageBehavior = default;
    private AudioSource _audioSource = default;
    private Level _selectedDemonLevel = default;

    public static DecisionCanvasManager Instance => Utils.GetInstance(ref _instance);

    private void Awake()
    {
        _interactionLayerPageBehavior = GetComponent<InteractionLayerPageBehavior>();
        _audioSource = GetComponent<AudioSource>();
        _selectedDemonLevel = PlayerBehavior.MainPlayer.DynamicData.SelectedItems.DemonLevel;
    }

    public void GoNextLevel()
    {
        SceneManagerBehavior.Instance.RestartCurrentScene();
    }

    public void GoPreviousLevel()
    {
        _selectedDemonLevel.Value -= 1;
        SceneManagerBehavior.Instance.RestartCurrentScene();
    }

    public void RepeatOrTryAgainLevel()
    {
        if (WinSystemBehavior.Instance.Win && !_selectedDemonLevel.IsMax)
            _selectedDemonLevel.Value -= 1;
        SceneManagerBehavior.Instance.RestartCurrentScene();
    }

    public void EndMission()
    {
        SceneManagerBehavior.Instance.LoadCountry();
    }

    public void Show()
    {
        if (WinSystemBehavior.Instance.Win)
        {
            _winCanvas.SetActive(true);
            _loseCanvas.SetActive(false);
            if (_selectedDemonLevel.IsMax)
            {
                _winTitle.text = $"Victory!!!! You have finished the game!!!!";
                _goNextLevelButton.Owner.SetActive(false);
            }
            else
            {
                _winTitle.text = $"Congragulations! You have completed level {_selectedDemonLevel.Value}.";
            }
        }
        else if (LoseSystemBehavior.Instance.Lose)
        {
            _winCanvas.SetActive(false);
            _loseCanvas.SetActive(true);
            _loseTitle.text = $"You have failed at level {_selectedDemonLevel.Value}.";
        }
        _interactionLayerPageBehavior.Enable();
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        _interactionLayerPageBehavior.Disable();
    }

    private void OnEnable()
    {
        if (WinSystemBehavior.Instance.Win)
        {
            if (_selectedDemonLevel.IsMax)
            {
                _audioSource.PlayOneShot(_staticData.VictorySoundClip, 0.8f);
                StartCoroutine(MusicPlayerBehavior.Instance.StopAllSounds(0.6f));
            }
            else
            {
                _selectedDemonLevel.Value += 1;
                _audioSource.PlayOneShot(_staticData.WinAudioClip, 0.3f);
                StartCoroutine(MusicPlayerBehavior.Instance.StopAllSounds(4.5f));
            }
        }
        else if (LoseSystemBehavior.Instance.Lose)
        {
            _audioSource.PlayOneShot(_staticData.LoseAudioClip, 0.6f);
            StartCoroutine(MusicPlayerBehavior.Instance.StopAllSounds(4f));
        }
    }
}
