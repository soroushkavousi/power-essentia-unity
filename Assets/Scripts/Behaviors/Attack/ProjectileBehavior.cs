using Assets.Scripts.Models;
using System;
using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(MovementBehavior))]
public class ProjectileBehavior : MonoBehaviour, IObserver<CollideData>, ISubject<HitParameters>
{
    [SerializeField] private ProjectileStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private float _damage;
    [SerializeField] private float _criticalChance;
    [SerializeField] private float _criticalDamage;
    [SerializeField] private float _projectileFlightSpeed;
    [SerializeField] private bool _done = false;
    private BodyBehavior _bodyBehavior = default;
    private MovementBehavior _movementBehavior = default;
    private readonly ObserverCollection<HitParameters> _hitObservers = new();

    public float Damage => _damage;
    public float CriticalChance => _criticalChance;
    public float CriticalDamage => _criticalDamage;
    public float ProjectileFlightSpeed => _projectileFlightSpeed;
    public MovementBehavior MovementBehavior => _movementBehavior;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; set; }

    public void Initialize(float damage, float criticalChance,
        float criticalDamage, Func<GameObject, GameObject> isTargetEnemyFunction,
        Level level = null)
    {
        _bodyBehavior = GetComponent<BodyBehavior>();
        _movementBehavior = GetComponent<MovementBehavior>();
        _movementBehavior.FeedData(_staticData.MovementStaticData, level);
        _bodyBehavior.FeedData();
        _bodyBehavior.Attach(this);

        _damage = damage;
        _criticalChance = criticalChance;
        _criticalDamage = criticalDamage;
        IsTargetEnemyFunction = isTargetEnemyFunction;
        //_InitializeEvent.Invoke();
    }

    public void HitIfEnemy(Collider2D otherCollider)
    {
        var enemy = IsTargetEnemyFunction(otherCollider.gameObject);
        if (enemy != null)
            HitEnemy(enemy, otherCollider);
    }

    private void HitEnemy(GameObject enemy, Collider2D otherCollider)
    {
        var enemyHealthBehavior = enemy.GetComponent<HealthBehavior>();
        if (enemyHealthBehavior == null)
            return;

        if (_done == true)
            return;
        _done = true;
        MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_staticData.HitSound, 0.3f);

        var criticalEffect = new CriticalEffect(Damage,
            _criticalChance, _criticalDamage);
        var damage = new Damage(DamageType.PHYSICAL, criticalEffect.Result,
            criticalEffect.IsApplied);
        enemyHealthBehavior.Health.Damage(damage);
        gameObject.SetActive(false);
        var hitParameters = new HitParameters(gameObject, otherCollider, enemy);
        Notify(hitParameters);
        Destroy(gameObject, 2f);
    }

    public void OnNotify(ISubject<CollideData> subject, CollideData collideData)
    {
        if (collideData.IsCollidingDisabled)
            return;

        if (ReferenceEquals(subject, _bodyBehavior))
        {
            switch (collideData.Type)
            {
                case CollideType.ENTER:
                    HitIfEnemy(collideData.TargetCollider2D);
                    break;
            }
        }
    }

    public void Attach(IObserver<HitParameters> observer) => _hitObservers.Add(observer);
    public void Detach(IObserver<HitParameters> observer) => _hitObservers.Remove(observer);
    public void Notify(HitParameters hitParameters) => _hitObservers.Notify(this, hitParameters);
}
