using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class StunStatusBehavior : MonoBehaviour, ISubject, ISubject<StunStatusInstance>
{
    [SerializeField] private StatusOwnerBehavior _statusOwnerBehavior = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private StunStatusState _state;
    [SerializeField] private List<StunStatusInstance> _instances = new();
    private ParticleSystem _particleSystem = default;
    private HealthBehavior _healthBehavior = default;
    private MovementBehavior _movementBehavior = default;
    private readonly ObserverCollection _observers = new();
    private readonly ObserverCollection<StunStatusInstance> _newStatusObservers = new();

    public StunStatusState State => _state;
    public List<StunStatusInstance> Instances => _instances;
    public bool IsAffected => _instances.Count != 0;

    public void FeedData()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _healthBehavior = _statusOwnerBehavior.GetComponent<HealthBehavior>();
        _movementBehavior = _statusOwnerBehavior.GetComponent<MovementBehavior>();

        _state = StunStatusState.CLEAR;
    }

    public void AddNewInstance(GameObject refGameObject,
        float damage, float duration, float criticalChance,
        float criticalDamage)
    {
        var instance = new StunStatusInstance(refGameObject,
            damage, duration, criticalChance, criticalDamage);
        Notify(instance);
        if (instance.Damage == 0 || instance.Duration == 0)
            return;
        StartCoroutine(ApplyInstance(instance));
    }

    //public void RemoveInstance(GameObject refGameObject)
    //{
    //    var instance = _instances
    //        .Single(i => i.RefGameObject == refGameObject);
    //    _instances.Remove(instance);
    //    StopParticleSystemIfNotAnyInstance();
    //}

    private IEnumerator ApplyInstance(StunStatusInstance instance)
    {
        _instances.Add(instance);
        StartCoroutine(Damage(instance));
        yield return new WaitForSeconds(instance.Duration);
        _instances.Remove(instance);
    }

    private IEnumerator Damage(StunStatusInstance instance)
    {
        var criticalEffect = new CriticalEffect(instance.Damage,
            instance.CriticalChance, instance.CriticalDamage);
        var damage = new Damage(DamageType.MAGIC, criticalEffect.Result,
            criticalEffect.IsApplied);
        _healthBehavior.Health.Damage(damage);
        yield break;
    }

    public void StartPreStunning()
    {
        if (_state == StunStatusState.PRE_STUNNING || _state == StunStatusState.STUNNING)
            return;
        _state = StunStatusState.PRE_STUNNING;
        _particleSystem.Play();
        _movementBehavior.StopMoving();
        Notify();
    }

    public void StopPreStunning()
    {

    }

    public void StartStunning()
    {
        _state = StunStatusState.STUNNING;
    }

    public void StopStunning()
    {

    }

    public void StartPostStunning()
    {
        _state = StunStatusState.POST_STUNNING;
    }

    public void StopPostStunning()
    {
        _particleSystem.Stop();
        _state = StunStatusState.CLEAR;
        Notify();
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);

    public void Attach(IObserver<StunStatusInstance> observer) => _newStatusObservers.Add(observer);
    public void Detach(IObserver<StunStatusInstance> observer) => _newStatusObservers.Remove(observer);
    public void Notify(StunStatusInstance hitParameters) => _newStatusObservers.Notify(this, hitParameters);
}
