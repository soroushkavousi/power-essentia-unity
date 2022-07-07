using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class StunStatusBehavior : MonoBehaviour
{
    [SerializeField] private StatusOwnerBehavior _statusOwnerBehavior = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private List<StunStatusInstance> _instances = new List<StunStatusInstance>();
    private ParticleSystem _particleSystem = default;
    private HealthBehavior _healthBehavior = default;
    private MovementBehavior _movementBehavior = default;

    public List<StunStatusInstance> Instances => _instances;
    public bool IsAffected => _instances.Count != 0;
    public OrderedList<Action<StunStatusInstance>> OnPreApplyInstanceActions { get; } = new OrderedList<Action<StunStatusInstance>>();
    public OrderedList<Action> OnStatusStarted { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnStatusFinished { get; } = new OrderedList<Action>();

    public void FeedData()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _healthBehavior = _statusOwnerBehavior.GetComponent<HealthBehavior>();
        _movementBehavior = _statusOwnerBehavior.GetComponent<MovementBehavior>();
    }

    public void AddNewInstance(GameObject refGameObject,
        float damage, float duration)
    {
        var instance = new StunStatusInstance(refGameObject,
            damage, duration);
        OnPreApplyInstanceActions.CallActionsSafely(instance);
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
        _healthBehavior.Health.Damage(
            new Damage(DamageType.MAGIC, instance.Damage));
        yield return null;
    }

    public void StartPreStunning()
    {
        if (IsAffected == false)
        {
            _particleSystem.Play();
            _movementBehavior.StopMoving();
            OnStatusStarted.CallActionsSafely();
        }
    }
    public void StopPreStunning()
    {

    }

    public void StartStunning()
    {

    }
    public void StopStunning()
    {

    }

    public void StartPostStunning()
    {

    }
    public void StopPostStunning()
    {
        if (IsAffected == false)
        {
            _particleSystem.Stop();
            OnStatusFinished.CallActionsSafely();
        }
    }
}
