using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabStaticData
{
    [SerializeField] private List<DiamondBehavior> _diamondPrefabs;
    [SerializeField] private List<DemonBehavior> _demonPrefabs;
    private Dictionary<DiamondName, DiamondBehavior> _diamondPrefabDict;
    private Dictionary<DemonName, DemonBehavior> _demonPrefabDict;
    public CriticalShowBehavior CriticalShowBehavior = default;

    public Dictionary<DiamondName, DiamondBehavior> DiamondPrefabs
    {
        get
        {
            if (_diamondPrefabDict != null)
                return _diamondPrefabDict;

            _diamondPrefabDict = new Dictionary<DiamondName, DiamondBehavior>();
            foreach (var diamondBehavior in _diamondPrefabs)
            {
                _diamondPrefabDict.Add(diamondBehavior.Name, diamondBehavior);
            }
            return _diamondPrefabDict;
        }
    }
    public Dictionary<DemonName, DemonBehavior> DemonPrefabs
    {
        get
        {
            if (_demonPrefabDict != null)
                return _demonPrefabDict;

            _demonPrefabDict = new Dictionary<DemonName, DemonBehavior>();
            foreach (var demonBehavior in _demonPrefabs)
            {
                _demonPrefabDict.Add(demonBehavior.Name, demonBehavior);
            }
            return _demonPrefabDict;
        }
    }
}
