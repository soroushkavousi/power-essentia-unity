using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionMenuBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _demonLevelText = default;
    [SerializeField] private TextMeshProUGUI _waveNumberText = default;
    [SerializeField] private TextMeshProUGUI _waveDemonsText = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]
    private OnePartAdvancedNumber _demonLevel = default;
    private OnePartAdvancedNumber _totalWaveCount = default;
    private OnePartAdvancedNumber _waveNumber = default;
    private OnePartAdvancedNumber _totalDemonCount = default;
    private OnePartAdvancedNumber _deadDemonCount = default;
    private float fixedDeltaTime;

    private void Awake()
    {
        fixedDeltaTime = Time.fixedDeltaTime;

        _demonLevel = PlayerBehavior.Main.DynamicData.SelectedItems.DemonLevel;
        _totalWaveCount = LevelManagerBehavior.Instance.TotalWaveCount;
        _waveNumber = WaveManagerBehavior.Instance.WaveNumber;
        _totalDemonCount = WaveManagerBehavior.Instance.TotalDemonCount;
        _deadDemonCount = WaveManagerBehavior.Instance.DeadDemonCount;
        _demonLevel.OnNewValueActions.Add(HandleDemonLevelChange);
        _totalWaveCount.OnNewValueActions.Add(HandleWaveNumberChange);
        _waveNumber.OnNewValueActions.Add(HandleWaveNumberChange);
        _totalDemonCount.OnNewValueActions.Add(HandleDemonCountChange);
        _deadDemonCount.OnNewValueActions.Add(HandleDemonCountChange);
        HandleDemonLevelChange(null);
        HandleWaveNumberChange(null);
        HandleDemonCountChange(null);
    }

    private void HandleWaveNumberChange(NumberChangeCommand changeCommand)
    {
        _waveNumberText.text = $"Wave {_waveNumber.IntValue} / {_totalWaveCount.IntValue}";
    }

    private void HandleDemonLevelChange(NumberChangeCommand changeCommand)
    {
        _demonLevelText.text = $"Level {_demonLevel.IntValue}";
    }

    private void HandleDemonCountChange(NumberChangeCommand changeCommand)
    {
        _waveDemonsText.text = $"Demons {_deadDemonCount.Value} / {_totalDemonCount.Value}";
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        //Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        PauseMenuBehavior.Instance.Show();
    }
}
