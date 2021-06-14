using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LevelResourceSystemBehavior))]
[RequireComponent(typeof(LoseSystemBehavior))]
[RequireComponent(typeof(WinSystemBehavior))]
public class LevelManagerBehavior : MonoBehaviour
{
    private static LevelManagerBehavior _instance = default;
    [SerializeField] private LevelManagerStaticData _staticData = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private OnePartAdvancedNumber _totalWaveCount = new OnePartAdvancedNumber();
    [SerializeField] private LevelDescriptionStaticData _levelDescriptionStaticData = default;
    [SerializeField] private float _timer = 0f;
    [SerializeField] private AdvancedBoolean _finished = new AdvancedBoolean();
    private bool _initialized = default;
    private LevelResourceSystemBehavior _levelResourceSystemBehavior = default;
    private LoseSystemBehavior _loseSystemBehavior = default;
    private WinSystemBehavior _winSystemBehavior = default;

    public static LevelManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public OnePartAdvancedNumber TotalWaveCount => _totalWaveCount;
    public AdvancedBoolean Finished => _finished;

    public void Initialize(LevelDescriptionStaticData levelDescriptionStaticData)
    {
        _instance ??= FindObjectOfType<LevelManagerBehavior>(true);
        _instance = FindObjectOfType<LevelManagerBehavior>(true);
        if (!_initialized)
        {
            _levelResourceSystemBehavior = GetComponent<LevelResourceSystemBehavior>();
            _loseSystemBehavior = GetComponent<LoseSystemBehavior>();
            _winSystemBehavior = GetComponent<WinSystemBehavior>();
            _initialized = true;
        }
        else
        {

        }
        _timer = 0;
        _levelDescriptionStaticData = levelDescriptionStaticData;
        _finished.FeedData(false);
        _levelResourceSystemBehavior.FeedData();
        _loseSystemBehavior.FeedData();
        _winSystemBehavior.FeedData();
        StartCoroutine(StartWaves());
        Time.timeScale = 1;
    }

    private IEnumerator StartWaves()
    {
        WaveDescription waveDescription;
        float deadLine;
        _totalWaveCount.FeedData(_levelDescriptionStaticData.WaveDescriptions.Count);
        for (int i = 0; i < _totalWaveCount.IntValue; i++)
        {
            waveDescription = _levelDescriptionStaticData.WaveDescriptions[i];
            deadLine = _staticData.WaveTime * (i + 1); 
            WaveManagerBehavior.Instance.Initialize(waveDescription);
            yield return new WaitUntil( () =>
                    _timer > deadLine
                    || WaveManagerBehavior.Instance.Finished
                );
        }
        yield return new WaitUntil(() => WaveManagerBehavior.Instance.Finished);
        _finished.Change(true, name, "LEVEL_FINISHED");
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }
}
