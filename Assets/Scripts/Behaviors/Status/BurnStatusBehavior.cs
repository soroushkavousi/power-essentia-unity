using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BurnStatusBehavior : MonoBehaviour, ISubject, ISubject<BurnStatusInstance>
{
    [SerializeField] private StatusOwnerBehavior _statusOwnerBehavior = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private BurnStatusState _state;
    [SerializeField] private List<BurnStatusInstance> _instances = new List<BurnStatusInstance>();
    private ParticleSystem _particleSystem = default;
    private HealthBehavior _healthBehavior = default;
    private MovementBehavior _movementBehavior = default;
    private readonly ObserverCollection _observers = new();
    private readonly ObserverCollection<BurnStatusInstance> _newStatusObservers = new();

    public bool IsAffected => _instances.Count != 0;

    public void FeedData()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _healthBehavior = _statusOwnerBehavior.GetComponent<HealthBehavior>();
        _movementBehavior = _statusOwnerBehavior.GetComponent<MovementBehavior>();

        _state = BurnStatusState.CLEAR;
    }

    public void AddNewInstance(GameObject refGameObject, float dps,
        float movementSlow, float criticalChance, float criticalDamage)
    {
        var instance = new BurnStatusInstance(refGameObject, dps,
            criticalChance, criticalDamage, movementSlow);
        Notify(instance);
        if (instance.Dps == 0 || instance.MovementSlow == 0)
            return;
        StartCoroutine(ApplyInstance(instance));
    }

    private IEnumerator ApplyInstance(BurnStatusInstance instance)
    {
        if (IsAffected == false)
        {
            StartStatus();
        }
        _instances.Add(instance);
        StartCoroutine(Damage(instance));
        StartCoroutine(Slow(instance));
        yield return null;
    }

    public void RemoveInstance(GameObject refGameObject)
    {
        var instance = _instances
            .Single(i => i.RefGameObject == refGameObject);
        RemoveInstance(instance);
    }

    private void RemoveInstance(BurnStatusInstance instance)
    {
        _instances.Remove(instance);
        if (IsAffected == false)
        {
            FinishStatus();
        }
    }

    public void StartStatus()
    {
        if (_state == BurnStatusState.BURNING)
            return;
        _particleSystem.Play();
        _state = BurnStatusState.BURNING;
        Notify();
    }

    public void FinishStatus()
    {
        _particleSystem.Stop();
        _state = BurnStatusState.CLEAR;
        Notify();
    }

    private IEnumerator Damage(BurnStatusInstance instance)
    {
        var dps = instance.Dps;
        yield return new WaitForSeconds(1);
        while (_instances.Contains(instance) && instance != null && instance.RefGameObject != null)
        {
            var criticalEffect = new CriticalEffect(instance.Dps,
            instance.CriticalChance, instance.CriticalDamage);
            var damage = new Damage(DamageType.MAGIC, criticalEffect.Result,
                criticalEffect.IsApplied);
            _healthBehavior.Health.Damage(damage);
            yield return new WaitForSeconds(1);
        }
        _healthBehavior.Health.Damage(
                new Damage(DamageType.MAGIC, dps));
        if (_instances.Contains(instance))
            RemoveInstance(instance);
    }

    private IEnumerator Slow(BurnStatusInstance instance)
    {
        _movementBehavior.Speed.Decrease(instance.MovementSlow);

        yield return new WaitUntil(
            () => _instances.Contains(instance) == false
            || instance == null || instance.RefGameObject == null);

        _movementBehavior.Speed.Increase(instance.MovementSlow);
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);

    public void Attach(IObserver<BurnStatusInstance> observer) => _newStatusObservers.Add(observer);
    public void Detach(IObserver<BurnStatusInstance> observer) => _newStatusObservers.Remove(observer);
    public void Notify(BurnStatusInstance hitParameters) => _newStatusObservers.Notify(this, hitParameters);
}
