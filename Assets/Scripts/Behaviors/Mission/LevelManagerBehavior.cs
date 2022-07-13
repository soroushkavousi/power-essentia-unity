using Assets.Scripts.Models;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LevelResourceSystemBehavior))]
[RequireComponent(typeof(LoseSystemBehavior))]
[RequireComponent(typeof(WinSystemBehavior))]
public class LevelManagerBehavior : MonoBehaviour
{
    private static LevelManagerBehavior _instance = default;
    [SerializeField] private LevelManagerStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private Observable<int> _totalWaveCount = new();
    [SerializeField] private LevelDescriptionStaticData _levelDescriptionStaticData = default;
    [SerializeField] private float _timer = 0f;
    [SerializeField] private Observable<bool> _finished = new();
    private LevelResourceSystemBehavior _levelResourceSystemBehavior = default;
    private LoseSystemBehavior _loseSystemBehavior = default;
    private WinSystemBehavior _winSystemBehavior = default;
    private Observable<int> _achievedDemonLevel = default;
    private Observable<int> _selectedDemonLevel = default;

    public static LevelManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public Observable<int> TotalWaveCount => _totalWaveCount;
    public Observable<bool> Finished => _finished;

    public void Initialize(LevelDescriptionStaticData levelDescriptionStaticData)
    {
        _levelResourceSystemBehavior ??= GetComponent<LevelResourceSystemBehavior>();
        _loseSystemBehavior ??= GetComponent<LoseSystemBehavior>();
        _winSystemBehavior ??= GetComponent<WinSystemBehavior>();
        _timer = 0;
        _levelDescriptionStaticData = levelDescriptionStaticData;
        _finished.Value = false;
        _levelResourceSystemBehavior.FeedData();
        _loseSystemBehavior.FeedData();
        _winSystemBehavior.FeedData();
        Time.timeScale = 1;
        _totalWaveCount.Value = _levelDescriptionStaticData.WaveDescriptions.Count;
        _achievedDemonLevel = PlayerBehavior.MainPlayer.DynamicData.Achievements.DemonLevel;
        _selectedDemonLevel = PlayerBehavior.MainPlayer.DynamicData.SelectedItems.DemonLevel;
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        WaveDescription waveDescription;
        float deadLine;
        for (int i = 0; i < _totalWaveCount.Value; i++)
        {
            waveDescription = _levelDescriptionStaticData.WaveDescriptions[i];
            waveDescription.WaveNumber = i + 1;
            deadLine = _staticData.WaveTime * (i + 1);
            WaveManagerBehavior.Instance.Initialize(waveDescription);
            yield return new WaitUntil(() =>
                   _timer > deadLine
                   || WaveManagerBehavior.Instance.Finished.Value
                );
        }
        yield return new WaitUntil(() => WaveManagerBehavior.Instance.Finished.Value);
        if (_achievedDemonLevel.Value < _selectedDemonLevel.Value)
            _achievedDemonLevel.Value = _selectedDemonLevel.Value;
        _finished.Value = true;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }
}
