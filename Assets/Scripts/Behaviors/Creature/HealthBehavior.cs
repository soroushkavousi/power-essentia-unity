using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(ResistanceBehavior))]
public class HealthBehavior : MonoBehaviour
{
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private HealthStaticData _staticData = default;
    [SerializeField] private ThreePartAdvancedNumber _health = new ThreePartAdvancedNumber(currentDummyMin: 0f, currentDummyMaxBaseOnPeak: true, 
        removeCurrentMinExceed: true, removeCurrentMaxExceed: true, keepRatio: true);
    [SerializeField] private bool _isDead = default;
    [SerializeField] private bool _dontDestroyOnDeath = default;
    private ResistanceBehavior _resistanceBehavior = default;

    public ThreePartAdvancedNumber Health => _health;
    public bool IsDead => _isDead;
    public OrderedList<Action> OnDieActions { get; } = new OrderedList<Action>();
    public ResistanceBehavior ResistanceBehavior => _resistanceBehavior;

    public void FeedData(HealthStaticData staticData, bool dontDestroyOnDeath = false)
    {
        _staticData = staticData;
        _health.FeedData(_staticData.StartHealth);
        _dontDestroyOnDeath = dontDestroyOnDeath;
        _health.Current.OnNewValueActions.Add(DieIfHealthIsZero);
        _resistanceBehavior = GetComponent<ResistanceBehavior>();
        _resistanceBehavior.FeedData(_staticData.ResistanceStaticData);
    }

    private void DieIfHealthIsZero(NumberChangeCommand changeCommand)
    {
        if (Health.Value <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _isDead = true;
        OnDieActions.CallActionsSafely();
        TriggerDeathVfx();
        if(_dontDestroyOnDeath == false)
            Destroy(gameObject);
    }

    private void TriggerDeathVfx()
    {
        if (_staticData.DeathVfxPrefab == null)
            return;

        var deathVfx = Instantiate(_staticData.DeathVfxPrefab, transform.position, transform.rotation);
        Destroy(deathVfx, 2f);
    }
}
