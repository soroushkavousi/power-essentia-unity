using UnityEngine;
using UnityEngine.UI;

public class OptionsCanvasManagerBehavior : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider = default;
    [SerializeField] private Slider _difficultySlider = default;

    private bool _isModified = false;

    private void Awake()
    {
        Options.Instance.OnMasterVolumeChangeActions.Add(OnMasterVolumeChange);
        OnMasterVolumeChange();
        Options.Instance.OnDifficultyChangedActions.Add(OnDifficultyChange);
        OnDifficultyChange();
        _isModified = false;
    }

    public void SetDefaults()
    {
        Options.Instance.ReturnToDefault();
    }

    public void Back()
    {
        if (_isModified)
            Options.Instance.WriteToStorage();
        _isModified = false;
        SceneManagerBehavior.Instance.LoadStart();
    }

    public void OnMasterVolumeSliderChanged()
    {
        _isModified = true;
        Options.Instance.MasterVolume = _masterVolumeSlider.value;
    }

    public void OnMasterDifficultySliderChanged()
    {
        _isModified = true;
        Options.Instance.Difficulty = (int)_difficultySlider.value;
    }

    private void OnMasterVolumeChange()
    {
        _masterVolumeSlider.value = Options.Instance.MasterVolume;
    }

    private void OnDifficultyChange()
    {
        _difficultySlider.value = Options.Instance.Difficulty;
    }
}
