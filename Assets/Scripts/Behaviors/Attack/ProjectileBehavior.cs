using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(MovementBehavior))]
public class ProjectileBehavior : MonoBehaviour
{
    //[SerializeField] private UnityEvent _InitializeEvent;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private ThreePartAdvancedNumber _damage = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private ThreePartAdvancedNumber _criticalChance = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _criticalDamage = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _projectileFlightSpeed = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private bool _done = false;
    private ProjectileStaticData _staticData = default;
    private AudioClip _hitSound = default;
    private BodyBehavior _bodyBehavior = default;
    private MovementBehavior _movementBehavior = default;

    public ThreePartAdvancedNumber Damage => _damage;
    public ThreePartAdvancedNumber CriticalChance => _criticalChance;
    public ThreePartAdvancedNumber CriticalDamage => _criticalDamage;
    public ThreePartAdvancedNumber ProjectileFlightSpeed => _projectileFlightSpeed;
    public MovementBehavior MovementBehavior => _movementBehavior;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; set; }
    public OrderedList<Action<HitParameters>> OnHitActions { get; } = new OrderedList<Action<HitParameters>>();

    public void Initialize(ProjectileStaticData staticData,
        float startDamage, float startCriticalChance,
        float startCriticalDamage, AudioClip hitSound, 
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _staticData = staticData;
        _bodyBehavior = GetComponent<BodyBehavior>();
        _movementBehavior = GetComponent<MovementBehavior>();
        _movementBehavior.FeedData(_staticData.MovementStaticData);
        _criticalChance.FeedData(startCriticalChance);
        _criticalDamage.FeedData(startCriticalDamage);
        _damage.FeedData(startDamage);
        _hitSound = hitSound;
        IsTargetEnemyFunction = isTargetEnemyFunction;
        _bodyBehavior.FeedData();
        _bodyBehavior.OnEnterActions.Add(HitIfEnemy);
        //_InitializeEvent.Invoke();
    }

    //public void FeedData(ProjectileStaticData staticData)
    //{
    //    _staticData = staticData;
    //    _movementBehavior.FeedData(_staticData.MovementStaticData);
    //}

    public void HitIfEnemy(Collider2D otherCollider)
    {
        var enemy = IsTargetEnemyFunction(otherCollider.gameObject);
        if (enemy != null)
            HitEnemy(enemy, otherCollider);
    }

    private void HitEnemy(GameObject enemy, Collider2D otherCollider)
    {
        var healthBehavior = enemy.GetComponent<HealthBehavior>();
        if (healthBehavior == null)
            return;

        if (_done == true)
            return;
        _done = true;
        MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_hitSound, 0.2f);
        Debug.Log($"_criticalChance: {_criticalChance.Value} | _criticalDamage: {_criticalDamage.Value}");

        var criticalEffect = new CriticalEffect(-Damage.Value, 
            _criticalChance.Value, _criticalDamage.Value);
        var damage = criticalEffect.Result;
        var healthChangeType = criticalEffect.IsApplied ?
            HealthChangeType.CRITICAL_PHYSICAL_DAMAGE :
            HealthChangeType.PHYSICAL_DAMAGE;
        healthBehavior.Health.Change(damage, name, healthChangeType);
        Destroy(gameObject);
        var hitParameters = new HitParameters(gameObject, otherCollider, enemy);
        OnHitActions.CallActionsSafely(hitParameters);
    }
}
