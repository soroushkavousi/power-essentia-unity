using TMPro;
using UnityEngine;

[RequireComponent(typeof(InteractionLayerPageBehavior))]
[RequireComponent(typeof(AudioSource))]
public class DesicionCanvasManager : MonoBehaviour
{
    private static DesicionCanvasManager _instance = default;
    [SerializeField] private DesicionCanvasStaticData _staticData = default;
    [SerializeField] private GameObject _winCanvas = default;
    [SerializeField] private TextMeshProUGUI _winTitle = default;
    [SerializeField] private GameObject _loseCanvas = default;
    [SerializeField] private TextMeshProUGUI _loseTitle = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private InteractionLayerPageBehavior _interactionLayerPageBehavior = default;
    private AudioSource _audioSource = default;
    private Observable<int> _selectedDemonLevel = default;

    public static DesicionCanvasManager Instance => Utils.GetInstance(ref _instance);

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
        if (WinSystemBehavior.Instance.Win)
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
            _winTitle.text = $"Congragulations! You have completed level {_selectedDemonLevel.Value}.";
            _selectedDemonLevel.Value += 1;
            _audioSource.PlayOneShot(_staticData.WinAudioClip, 0.3f);
        }
        else if (LoseSystemBehavior.Instance.Lose)
        {
            _winCanvas.SetActive(false);
            _loseCanvas.SetActive(true);
            _loseTitle.text = $"You have failed on level {_selectedDemonLevel.Value}.";
            _audioSource.PlayOneShot(_staticData.LoseAudioClip, 0.3f);
        }
        _interactionLayerPageBehavior.Enable();
        Time.timeScale = 0f;
    }

    public void Hide()
    {
        _interactionLayerPageBehavior.Disable();
    }
}
