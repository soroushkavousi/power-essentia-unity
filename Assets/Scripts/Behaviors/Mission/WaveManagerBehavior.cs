using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerBehavior : MonoBehaviour
{
    private static WaveManagerBehavior _instance = default;
    [SerializeField] private WaveManagerStaticData _staticData = default;
    [SerializeField] private List<Transform> _rowSpawnLocationList = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private OnePartAdvancedNumber _waveNumber = new OnePartAdvancedNumber();
    [SerializeField] private bool _finished = default;
    [SerializeField] private WaveDescription _description = default;
    [SerializeField] private List<int> _pickedRandomNumbers = default;
    [SerializeField] private OnePartAdvancedNumber _totalDemonCount = new OnePartAdvancedNumber();
    [SerializeField] private OnePartAdvancedNumber _deadDemonCount = new OnePartAdvancedNumber();
    private bool _initialized = default;
    private Dictionary<RowNumber, Transform> _rowSpawnLocations = default;
    private OnePartAdvancedNumber _selectedDemonLevel = default;

    public static WaveManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public Dictionary<RowNumber, Transform> RowSpawnLocations => _rowSpawnLocations;
    public bool Finished => _finished;
    public OnePartAdvancedNumber WaveNumber => _waveNumber;
    public OnePartAdvancedNumber TotalDemonCount => _totalDemonCount;
    public OnePartAdvancedNumber DeadDemonCount => _deadDemonCount;
    public OrderedList<Action> OnNewWaveStartedActions { get; } = new OrderedList<Action>();
    public OrderedList<Action<DemonBehavior>> OnNewSpawnedInvaderActions { get; } = new OrderedList<Action<DemonBehavior>>();
    public OrderedList<Action<DemonBehavior>> OnNewDeadInvaderActions { get; } = new OrderedList<Action<DemonBehavior>>();
   
    public void Initialize(WaveDescription waveDescription)
    {
        if(!_initialized)
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
            _waveNumber.FeedData(1);
            _totalDemonCount.FeedData(0);
            _deadDemonCount.FeedData(0);
            OnNewSpawnedInvaderActions.Add((demonBehavior) => _totalDemonCount.Change(1, name));
            OnNewDeadInvaderActions.Add(0, (demonBehavior) => _deadDemonCount.Change(1, name));
            OnNewDeadInvaderActions.Add(1, CheckWaveStatus);
        }
        else
        {
            _waveNumber.Change(+1, name, "NEXT_WAVE");
        }
        _initialized = true;
        _finished = false;
        _description = waveDescription;

        StartCoroutine(SpawnDemonsRandomly(waveDescription));
    }

    private void CheckWaveStatus(DemonBehavior demonBehavior)
    {
        var totalDemonCount = _totalDemonCount.IntValue;
        var deadDemonCount = _deadDemonCount.IntValue;
        if(deadDemonCount == totalDemonCount && totalDemonCount != 0)
        {
            _finished = true;
        }
    }

    private IEnumerator SpawnDemonsRandomly(WaveDescription waveDescription)
    {
        OnNewWaveStartedActions.CallActionsSafely();
        _pickedRandomNumbers = new List<int>();
        var level = _selectedDemonLevel.IntValue;
        foreach (var invaderBunch in waveDescription.DemonBunches)
        {
            var prefab = PrefabContainerBehavior.Instance.DemonPrefabs[invaderBunch.DemonName];
            for (int i = 0; i < invaderBunch.Count; i++)
            {
                StartCoroutine(SpawnDemon(prefab, level, waveDescription.ColumnCount));
            }
        }
        //var rowSpawnLocation = RowSpawnLocations[waveRowData.RowNumber];
        //for (int i = 0; i < waveRowData.Count; i++)
        //{
        //    var invaderPosition = new Vector2(
        //        rowSpawnLocation.position.x + waveRowData.XOffset + i * waveRowData.Distance,
        //        rowSpawnLocation.position.y);
        //    StartCoroutine(SpawnDemon(waveRowData.DemonBehavior,
        //        invaderLevel, rowSpawnLocation, invaderPosition));
        //}
        yield return null;
    }

    private IEnumerator SpawnDemon(DemonBehavior prefab, int level, int columnCount)
    {
        var (rowSpawnLocation, position) = PickUniqueRandomPositionInWave(columnCount);
        var invaderBehavior = Instantiate(prefab,
                position, transform.rotation, rowSpawnLocation);
        invaderBehavior.Initialize(level);
        yield return null;
    }

    private (Transform, Vector3) PickUniqueRandomPositionInWave(int columnCount)
    {
        int randomNumber;
        var cellCount = 5 * columnCount;
        lock (_pickedRandomNumbers)
        {
            do
            {
                randomNumber = UnityEngine.Random.Range(0, cellCount);
            }
            while (_pickedRandomNumbers.Contains(randomNumber));
            _pickedRandomNumbers.Add(randomNumber);
        }
        var rowNumber = (randomNumber / columnCount + 1).To<RowNumber>();
        var rowSpawnLocation = RowSpawnLocations[rowNumber];
        var rowSpawnLocationPosition = rowSpawnLocation.position;
        var yPosition = rowSpawnLocationPosition.y;

        var columnNumber = randomNumber % columnCount;
        var xPosition = rowSpawnLocationPosition.x + columnNumber * _staticData.ColumnDistance;
        var zPosition = rowSpawnLocationPosition.z;
        return (rowSpawnLocation, new Vector3(xPosition, yPosition, zPosition));
    }
}
