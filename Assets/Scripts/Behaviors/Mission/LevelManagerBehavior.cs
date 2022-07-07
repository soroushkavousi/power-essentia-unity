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
    private bool _initialized = default;
    private LevelResourceSystemBehavior _levelResourceSystemBehavior = default;
    private LoseSystemBehavior _loseSystemBehavior = default;
    private WinSystemBehavior _winSystemBehavior = default;

    public static LevelManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public Observable<int> TotalWaveCount => _totalWaveCount;
    public Observable<bool> Finished => _finished;

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
        _finished.Value = false;
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
        _totalWaveCount.Value = _levelDescriptionStaticData.WaveDescriptions.Count;
        for (int i = 0; i < _totalWaveCount.Value; i++)
        {
            waveDescription = _levelDescriptionStaticData.WaveDescriptions[i];
            deadLine = _staticData.WaveTime * (i + 1);
            WaveManagerBehavior.Instance.Initialize(waveDescription);
            yield return new WaitUntil(() =>
                   _timer > deadLine
                   || WaveManagerBehavior.Instance.Finished
                );
        }
        yield return new WaitUntil(() => WaveManagerBehavior.Instance.Finished);
        _finished.Value = true;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }
}
