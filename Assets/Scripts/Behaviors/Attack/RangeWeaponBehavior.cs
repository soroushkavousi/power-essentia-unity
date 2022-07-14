using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponBehavior : WeaponBehavior, ISubject<ProjectileBehavior>, IObserver<HitParameters>
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(RangeWeaponBehavior) + Constants.HeaderEnd)]

    [SerializeField] private RangeWeaponStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] protected ProjectileBehavior _projectileBehavior = default;
    [SerializeField] protected AudioClip _fireSound;
    [SerializeField] protected Stack<ProjectileBehavior> _roundProjectiles = new();
    protected readonly ObserverCollection<ProjectileBehavior> _projectileObservers = new();

    public RangeWeaponStaticData StaticData => _staticData;

    public override void Initialize(Level level,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        base.FeedData(_staticData);
        base.Initialize(level, isTargetEnemyFunction);
        _projectileBehavior = _staticData.ProjectileBehavior;
        _fireSound = _staticData.FireSound;
    }

    public virtual ProjectileBehavior CreateProjectile(Transform parent)
    {
        var newProjectileBehavior = Instantiate(_projectileBehavior,
            parent.transform.position, parent.transform.rotation,
            parent);
        InitializeProjectile(newProjectileBehavior);
        Notify(newProjectileBehavior);
        _roundProjectiles.Push(newProjectileBehavior);
        return newProjectileBehavior;
    }

    public virtual void FireRoundProjectiles(Vector2 targetPosition)
    {
        while (_roundProjectiles.Count != 0)
        {
            var projectileBehavior = _roundProjectiles.Pop();
            projectileBehavior.transform.SetParent(MissionManagerBehavior.Instance.ProjectileBox, true);
            projectileBehavior.MovementBehavior.MoveToPosition(targetPosition);
            if (_fireSound != null)
                MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_fireSound, 0.2f);
        }
    }

    private void InitializeProjectile(ProjectileBehavior projectileBehavior)
    {
        projectileBehavior.Initialize(
            _attackDamage.Value,
            _criticalChance.Value,
            _criticalDamage.Value,
            IsTargetEnemyFunction,
            _level);

        projectileBehavior.Attach(this);
    }

    public void Attach(IObserver<ProjectileBehavior> observer) => _projectileObservers.Add(observer);
    public void Detach(IObserver<ProjectileBehavior> observer) => _projectileObservers.Remove(observer);
    public void Notify(ProjectileBehavior hitParameters) => _projectileObservers.Notify(this, hitParameters);

    public void OnNotify(ISubject<HitParameters> subject, HitParameters hitParameters)
    {
        if (subject is not ProjectileBehavior)
            return;

        Notify(hitParameters);
    }
}
