using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BurnStatusBehavior : MonoBehaviour
{
    [SerializeField] private StatusOwnerBehavior _statusOwnerBehavior = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private List<BurnStatusInstance> _instances = new List<BurnStatusInstance>();
    private ParticleSystem _particleSystem = default;
    private HealthBehavior _healthBehavior = default;
    private MovementBehavior _movementBehavior = default;

    public bool IsAffected => _instances.Count != 0;
    public OrderedList<Action<BurnStatusInstance>> OnPreApplyInstanceActions { get; } = new OrderedList<Action<BurnStatusInstance>>();
    public OrderedList<Action> OnStatusStarted { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnStatusFinished { get; } = new OrderedList<Action>();

    public void FeedData()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _healthBehavior = _statusOwnerBehavior.GetComponent<HealthBehavior>();
        _movementBehavior = _statusOwnerBehavior.GetComponent<MovementBehavior>();
    }

    public void AddNewInstance(GameObject refGameObject, float dps,
        float movementSlow, float criticalChance, float criticalDamage)
    {
        var instance = new BurnStatusInstance(refGameObject, dps,
            criticalChance, criticalDamage, movementSlow);
        OnPreApplyInstanceActions.CallActionsSafely(instance);
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
        _particleSystem.Play();
        OnStatusStarted.CallActionsSafely();
    }

    public void FinishStatus()
    {
        _particleSystem.Stop();
        OnStatusFinished.CallActionsSafely();
    }

    private IEnumerator Damage(BurnStatusInstance instance)
    {
        var dps = instance.Dps;
        //enemy.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(1);
        while (_instances.Contains(instance) && instance != null && instance.RefGameObject != null)
        {
            _healthBehavior.Health.Damage(
                new Damage(DamageType.MAGIC, dps));
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
}
