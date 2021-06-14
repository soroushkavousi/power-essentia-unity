using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class BurnStatusInstance
{
    [SerializeField] private GameObject _refGameObject = default;
    [SerializeField] private string _ownerID = default;
    [SerializeField] private ThreePartAdvancedNumber _dps = default;
    [SerializeField] private ThreePartAdvancedNumber _criticalChance = default;
    [SerializeField] private ThreePartAdvancedNumber _criticalDamage = default;
    [SerializeField] private ThreePartAdvancedNumber _movementSlow = default;

    public GameObject RefGameObject => _refGameObject;
    public string OwnerID => _ownerID;
    public ThreePartAdvancedNumber Dps => _dps;
    public ThreePartAdvancedNumber CriticalChance => _criticalChance;
    public ThreePartAdvancedNumber CriticalDamage => _criticalDamage;
    public ThreePartAdvancedNumber MovementSlow => _movementSlow;

    public BurnStatusInstance(GameObject refGameObject,
        ThreePartAdvancedNumber dps, ThreePartAdvancedNumber criticalChance,
        ThreePartAdvancedNumber criticalDamage, ThreePartAdvancedNumber movementSlow)
    {
        _refGameObject = refGameObject;
        _ownerID = "MAIN_PLAYER";
        _dps = dps;
        _criticalChance = criticalChance;
        _criticalDamage = criticalDamage;
        _movementSlow = movementSlow;
    }
}
