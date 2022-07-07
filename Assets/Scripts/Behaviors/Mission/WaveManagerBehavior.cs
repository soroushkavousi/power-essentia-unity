﻿using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerBehavior : MonoBehaviour, ISubject<DemonBehavior>, IObserver
{
    private static WaveManagerBehavior _instance = default;
    [SerializeField] private WaveManagerStaticData _staticData = default;
    [SerializeField] private List<Transform> _rowSpawnLocationList = default;
    [SerializeField] private int _rowCount = 5;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Observable<int> _waveNumber;
    [SerializeField] private bool _finished = default;
    [SerializeField] private WaveDescription _description = default;
    [SerializeField] private List<int> _pickedRandomNumbers = default;
    [SerializeField] private Observable<int> _totalDemonCount;
    [SerializeField] private Observable<int> _deadDemonCount;
    private Dictionary<RowNumber, Transform> _rowSpawnLocations = default;
    private Observable<int> _selectedDemonLevel = default;
    protected readonly ObserverCollection<DemonBehavior> _observers = new();

    public static WaveManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public Dictionary<RowNumber, Transform> RowSpawnLocations => _rowSpawnLocations;
    public bool Finished => _finished;
    public Observable<int> WaveNumber => _waveNumber;
    public Observable<int> TotalDemonCount => _totalDemonCount;
    public Observable<int> DeadDemonCount => _deadDemonCount;

    private void Awake()
    {
        _selectedDemonLevel = PlayerBehavior.Main.DynamicData.SelectedItems.DemonLevel;
        _rowSpawnLocations = new Dictionary<RowNumber, Transform>
            {
                { RowNumber.ONE, _rowSpawnLocationList[0] },
                { RowNumber.TWO, _rowSpawnLocationList[1] },
                { RowNumber.THREE, _rowSpawnLocationList[2] },
                { RowNumber.FOUR, _rowSpawnLocationList[3] },
                { RowNumber.FIVE, _rowSpawnLocationList[4] },
            };
        _waveNumber = new(1);
        _totalDemonCount = new(0);
        _deadDemonCount = new(0);
    }

    public void Initialize(WaveDescription waveDescription)
    {
        _description = waveDescription;
        _waveNumber.Value = _description.WaveNumber;
        _finished = false;

        StartCoroutine(SpawnDemonsRandomly(waveDescription));
    }

    private IEnumerator SpawnDemonsRandomly(WaveDescription waveDescription)
    {
        _pickedRandomNumbers = new List<int>();
        var level = _selectedDemonLevel.Value;
        foreach (var demonBunch in waveDescription.DemonBunches)
        {
            var prefab = GameManagerBehavior.Instance.StaticData.Prefabs.DemonPrefabs[demonBunch.DemonName];
            for (int i = 0; i < demonBunch.Count; i++)
            {
                StartCoroutine(SpawnDemon(prefab, level, waveDescription.ColumnCount));
            }
        }
        yield return null;
    }

    private IEnumerator SpawnDemon(DemonBehavior prefab, int level, int columnCount)
    {
        var (rowSpawnLocation, position) = PickUniqueRandomPositionInWave(columnCount);
        var demonBehavior = Instantiate(prefab,
                position, transform.rotation, rowSpawnLocation);
        demonBehavior.Attach(this);
        demonBehavior.Initialize(level);
        _totalDemonCount.Value += 1;
        Notify(demonBehavior);
        yield return null;
    }

    private (Transform, Vector3) PickUniqueRandomPositionInWave(int columnCount)
    {
        int randomNumber;
        var cellCount = _rowCount * columnCount;
        lock (_pickedRandomNumbers)
        {
            do
            {
                randomNumber = Random.Range(0, cellCount);
            }
            while (_pickedRandomNumbers.Contains(randomNumber));
            _pickedRandomNumbers.Add(randomNumber);
        }
        var rowNumber = (randomNumber % _rowCount + 1).To<RowNumber>();
        var rowSpawnLocation = RowSpawnLocations[rowNumber];
        var rowSpawnLocationPosition = rowSpawnLocation.position;
        var yPosition = rowSpawnLocationPosition.y;

        var columnNumber = randomNumber / _rowCount;
        var xPosition = rowSpawnLocationPosition.x + columnNumber * _staticData.ColumnDistance;
        var zPosition = rowSpawnLocationPosition.z;
        return (rowSpawnLocation, new Vector3(xPosition, yPosition, zPosition));
    }

    private void CheckWaveStatus()
    {
        var totalDemonCount = _totalDemonCount.Value;
        var deadDemonCount = _deadDemonCount.Value;
        if (deadDemonCount == totalDemonCount && totalDemonCount != 0)
        {
            _finished = true;
        }
    }

    public void Attach(IObserver<DemonBehavior> observer) => _observers.Add(observer);
    public void Detach(IObserver<DemonBehavior> observer) => _observers.Remove(observer);
    public void Notify(DemonBehavior demonBehavior) => _observers.Notify(this, demonBehavior);

    public void OnNotify(ISubject subject)
    {
        if (subject is DemonBehavior demonBehavior)
        {
            if (demonBehavior.State == DemonState.DEAD)
            {
                _deadDemonCount.Value += 1;
                CheckWaveStatus();
            }
        }
    }
}
