using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefabContainerBehavior : MonoBehaviour
{
    private static PrefabContainerBehavior _instance = default;
    [SerializeField] private List<DiamondBehavior> _diamondPrefabs = default;
    [SerializeField] private List<DemonBehavior> _demonPrefabs = default;
    private Dictionary<DiamondName, DiamondBehavior> _diamondPrefabDictionary = default;
    private Dictionary<DemonName, DemonBehavior> _demonPrefabDictionary = default;

    public static PrefabContainerBehavior Instance => Utils.GetInstance(ref _instance);
    public Dictionary<DiamondName, DiamondBehavior> DiamondPrefabs => _diamondPrefabDictionary;
    public Dictionary<DemonName, DemonBehavior> DemonPrefabs => _demonPrefabDictionary;
    //public DiamondNameConverter DiamonNameConverter => _diamonNameConverter;

    public void FeedData()
    {
        MakeDiamondPrefabDictionary();
        MakeDemonPrefabDictionary();
    }

    private void MakeDiamondPrefabDictionary()
    {
        _diamondPrefabDictionary = new Dictionary<DiamondName, DiamondBehavior>();
        foreach (var diamondBehavior in _diamondPrefabs)
        {
            _diamondPrefabDictionary.Add(diamondBehavior.Name, diamondBehavior);
        }
    }

    private void MakeDemonPrefabDictionary()
    {
        _demonPrefabDictionary = new Dictionary<DemonName, DemonBehavior>();
        foreach (var invaderBehavior in _demonPrefabs)
        {
            _demonPrefabDictionary.Add(invaderBehavior.Name, invaderBehavior);
        }
    }
}
