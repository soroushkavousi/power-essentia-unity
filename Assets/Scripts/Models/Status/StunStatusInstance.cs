using System;
using UnityEngine;

[Serializable]
public class StunStatusInstance
{
    [SerializeField] private GameObject _refGameObject = default;
    [SerializeField] private float _damage;
    [SerializeField] private float _duration;

    public GameObject RefGameObject => _refGameObject;
    public float Damage => _damage;
    public float Duration => _duration;

    public StunStatusInstance(GameObject refGameObject, float damage,
        float duration)
    {
        _refGameObject = refGameObject;
        _damage = damage;
        _duration = duration;
    }
}
