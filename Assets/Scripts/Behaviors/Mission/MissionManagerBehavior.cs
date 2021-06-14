using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManagerBehavior : MonoBehaviour
{
    private static MissionManagerBehavior _instance = default;
    [SerializeField] private MissionManagerStaticData _staticData = default;
    [SerializeField] private Transform _battleField = default;
    [SerializeField] private Transform _diamondEffectsParent = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private OnePartAdvancedNumber _selectedDemonLevel = default;
    [SerializeField] private int _startDemonLevel = default;

    public static MissionManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public Vector2 GameAreaMaxPosition => _staticData.MissionAreaMaxPosition;
    public Transform BattleField => _battleField;
    public Transform DiamondEffectsParent => _diamondEffectsParent;
    public int StartDemonLevel => _startDemonLevel;

    private void Awake()
    {
        Time.timeScale = 1;
        _selectedDemonLevel = PlayerBehavior.Main.DynamicData.SelectedItems.DemonLevel;
    }

    private void Start()
    {
        StartSelectedLevel();
    }

    public void StartSelectedLevel()
    {
        _startDemonLevel = _selectedDemonLevel.IntValue;
        var levelStaticData = _staticData.Levels[_startDemonLevel];
        LevelManagerBehavior.Instance.Initialize(levelStaticData);
    }
}
