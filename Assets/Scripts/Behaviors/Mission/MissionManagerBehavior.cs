﻿using System.Collections;
using UnityEngine;

public class MissionManagerBehavior : MonoBehaviour
{
    private static MissionManagerBehavior _instance = default;
    [SerializeField] private MissionManagerStaticData _staticData = default;
    [SerializeField] private Transform _battleField = default;
    [SerializeField] private Transform _diamondEffectsParent = default;
    [SerializeField] private Transform _wallLocation = default;
    [SerializeField] private Transform _projectileBox = default;
    [SerializeField] private Transform _tempBox = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private Observable<int> _selectedDemonLevel = default;
    [SerializeField] private int _startDemonLevel = default;
    private AudioSource _audioSorce = default;

    public static MissionManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public Transform BattleField => _battleField;
    public Transform DiamondEffectsParent => _diamondEffectsParent;
    public Transform WallLocation => _wallLocation;
    public Transform ProjectileBox => _projectileBox;
    public Transform TempBox => _tempBox;
    public int StartDemonLevel => _startDemonLevel;
    public AudioSource AudioSource => _audioSorce;

    private void Awake()
    {
        _audioSorce = GetComponent<AudioSource>();
        Time.timeScale = 1;
        _selectedDemonLevel = PlayerBehavior.MainPlayer.DynamicData.SelectedItems.DemonLevel;
        StartSelectedLevel();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        _audioSorce.Play();
    }

    public void StartSelectedLevel()
    {
        _startDemonLevel = _selectedDemonLevel.Value;
        var levelDescriptionStaticData = _staticData.LevelDescriptions[_startDemonLevel];
        LevelManagerBehavior.Instance.Initialize(levelDescriptionStaticData);
    }
}
