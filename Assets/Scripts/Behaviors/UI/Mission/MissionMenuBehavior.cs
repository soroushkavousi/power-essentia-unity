using TMPro;
using UnityEngine;

public class MissionMenuBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _demonLevelText = default;
    [SerializeField] private TextMeshProUGUI _waveNumberText = default;
    [SerializeField] private TextMeshProUGUI _waveDemonsText = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]
    private Observable<int> _demonLevel = default;
    private Observable<int> _totalWaveCount = default;
    private Observable<int> _waveNumber = default;
    private Observable<int> _totalDemonCount = default;
    private Observable<int> _deadDemonCount = default;
    private float _fixedDeltaTime;

    private void Start()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        _demonLevel = PlayerBehavior.Main.DynamicData.SelectedItems.DemonLevel;
        _demonLevel.Attach(this);

        _totalWaveCount = LevelManagerBehavior.Instance.TotalWaveCount;
        _totalWaveCount.Attach(this);

        _waveNumber = WaveManagerBehavior.Instance.WaveNumber;
        _waveNumber.Attach(this);

        _totalDemonCount = WaveManagerBehavior.Instance.TotalDemonCount;
        _totalDemonCount.Attach(this);

        _deadDemonCount = WaveManagerBehavior.Instance.DeadDemonCount;
        _deadDemonCount.Attach(this);

        HandleDemonLevelChange();
        HandleWaveNumberChange();
        HandleDemonCountChange();
    }

    private void HandleWaveNumberChange()
    {
        _waveNumberText.text = $"Wave {_waveNumber.Value} / {_totalWaveCount.Value}";
    }

    private void HandleDemonLevelChange()
    {
        if (WinSystemBehavior.Instance.Win || LoseSystemBehavior.Instance.Lose)
            return;
        _demonLevelText.text = $"Level {_demonLevel.Value}";
    }

    private void HandleDemonCountChange()
    {
        _waveDemonsText.text = $"Demons {_deadDemonCount.Value} / {_totalDemonCount.Value}";
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        //Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        PauseMenuBehavior.Instance.Show();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _demonLevel)
        {
            HandleDemonLevelChange();
        }
        else if (subject == _totalWaveCount
            || subject == _waveNumber)
        {
            HandleWaveNumberChange();
        }
        else if (subject == _totalDemonCount
            || subject == _deadDemonCount)
        {
            HandleDemonCountChange();
        }
    }
}
