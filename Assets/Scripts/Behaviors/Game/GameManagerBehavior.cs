using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DefaultContainerBehavior))]
[RequireComponent(typeof(PrefabContainerBehavior))]
public class GameManagerBehavior : MonoBehaviour
{
    private static GameManagerBehavior _instance = default;
    [SerializeField] private bool _isTest = default;
    [SerializeField] private List<PlayerSet> _playerSets = default;
    [SerializeField] private GameObject _tempCircle = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    private DefaultContainerBehavior _defaultContainerBehavior = default;
    private PrefabContainerBehavior _prefabContainerBehavior = default;

    public static GameManagerBehavior Instance => Utils.GetInstance(ref _instance);
    public bool IsTest => _isTest;
    public GameObject TempCircle => _tempCircle;
    public List<PlayerSet> PlayerSets => _playerSets;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _defaultContainerBehavior = GetComponent<DefaultContainerBehavior>();
        _prefabContainerBehavior = GetComponent<PrefabContainerBehavior>();

        _defaultContainerBehavior.FeedData();
        _prefabContainerBehavior.FeedData();
    }
}
