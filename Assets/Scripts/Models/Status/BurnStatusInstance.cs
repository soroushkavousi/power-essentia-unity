using System;
using UnityEngine;

[Serializable]
public class BurnStatusInstance
{
    [SerializeField] private GameObject _refGameObject = default;
    [SerializeField] private string _ownerID = default;
    [SerializeField] private float _dps = default;
    [SerializeField] private float _criticalChance = default;
    [SerializeField] private float _criticalDamage = default;
    [SerializeField] private float _movementSlow = default;

    public GameObject RefGameObject => _refGameObject;
    public string OwnerID => _ownerID;
    public float Dps => _dps;
    public float CriticalChance => _criticalChance;
    public float CriticalDamage => _criticalDamage;
    public float MovementSlow => _movementSlow;

    public BurnStatusInstance(GameObject refGameObject,
        float dps, float criticalChance,
        float criticalDamage, float movementSlow)
    {
        _refGameObject = refGameObject;
        _ownerID = "MAIN_PLAYER";
        _dps = dps;
        _criticalChance = criticalChance;
        _criticalDamage = criticalDamage;
        _movementSlow = movementSlow;
    }
}
