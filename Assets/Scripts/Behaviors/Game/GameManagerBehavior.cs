using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    private static GameManagerBehavior _instance = default;
    [SerializeField] private GameStaticData _staticData = default;
    [SerializeField] private bool _isTest = default;
    [SerializeField] private List<PlayerSet> _playerSets = default;
    [SerializeField] private GameObject _tempCircle = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public static GameManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public GameStaticData StaticData => _staticData;
    public bool IsTest => _isTest;
    public GameObject TempCircle => _tempCircle;
    public List<PlayerSet> PlayerSets => _playerSets;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (_isTest)
            TestPlayer.ApplyData(PlayerBehavior.MainPlayer.DynamicData);
    }
}
