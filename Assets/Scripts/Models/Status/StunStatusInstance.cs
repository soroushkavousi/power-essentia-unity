using System;
using UnityEngine;

[Serializable]
public class StunStatusInstance
{
    [SerializeField] private GameObject _refGameObject = default;
    [SerializeField] private float _damage;
    [SerializeField] private float _duration;
    [SerializeField] private float _criticalChance;
    [SerializeField] private float _criticalDamage;

    public GameObject RefGameObject => _refGameObject;
    public float Damage => _damage;
    public float Duration => _duration;
    public float CriticalChance => _criticalChance;
    public float CriticalDamage => _criticalDamage;

    public StunStatusInstance(GameObject refGameObject, float damage,
        float duration, float criticalChance, float criticalDamage)
    {
        _refGameObject = refGameObject;
        _damage = damage;
        _duration = duration;
        _criticalChance = criticalChance;
        _criticalDamage = criticalDamage;
    }
}
