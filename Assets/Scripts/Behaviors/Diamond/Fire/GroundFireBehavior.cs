using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(AudioSource))]
public class GroundFireBehavior : MonoBehaviour, IObserver<CollideData>
{
    [SerializeField] private GroundFireStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Number _duration;
    [SerializeField] private Number _damage;
    [SerializeField] private Number _slow;
    [SerializeField] private Number _criticalChance;
    [SerializeField] private Number _criticalDamage;
    [SerializeField] private GameObject _enemy = default;
    [SerializeField] private bool _isSample = default;
    protected Observable<int> _level = default;
    private BodyBehavior _bodyBehavior = default;
    private ParticleSystem _particleSystem = default;
    private AudioSource _audioSource = default;

    public Number Duration => _duration;
    public Number Damage => _damage;
    public Number Slow => _slow;
    public Number CriticalChance => _criticalChance;
    public Number CriticalDamage => _criticalDamage;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; private set; }

    public void Initialize(Observable<int> level, GameObject enemy,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        _bodyBehavior = GetComponent<BodyBehavior>();
        _bodyBehavior.FeedData();
        _bodyBehavior.Attach(this);

        _level = level;

        _duration = new(_staticData.Duration, level,
            _staticData.DurationLevelPercentage);

        _damage = new(_staticData.Damage, level,
            _staticData.DamageLevePercentage, min: 0f);

        _slow = new(_staticData.Slow, level,
            _staticData.SlowLevelPercentage, min: 0f);

        _criticalChance = new(_staticData.CriticalChance, level,
            _staticData.CriticalChanceLevelPercentage, min: 0f, max: 100f);

        _criticalDamage = new(_staticData.CriticalDamage, level,
            _staticData.CriticalDamageLevelPercentage, min: 0f);

        _enemy = enemy;
        IsTargetEnemyFunction = isTargetEnemyFunction;

        transform.position = _enemy.transform.position + (Vector3)_staticData.SpawnOffset;
        _isSample = _enemy == OutBoxBehavior.Instance.Location1.gameObject;
        if (_isSample)
        {
            gameObject.SetActive(false);
            return;
        }
        StartCoroutine(OnAfterInitialization());
    }

    private IEnumerator OnAfterInitialization()
    {
        yield return null;
        StartCoroutine(DestroyAfterLifetime());
        //_bodyBehavior.IsColliderDisabled = true;
        //var mainParticleSystem = _particleSystem.main;
        //mainParticleSystem.playOnAwake = false;
        //_audioSource.playOnAwake = false;
    }

    private void OnBodyEnter(Collider2D collider2D)
    {
        var enemy = IsTargetEnemyFunction(collider2D.gameObject);
        if (enemy == null)
            return;

        var statusOwner = enemy.GetComponent<StatusOwnerBehavior>();
        if (statusOwner == null)
            return;

        statusOwner.BurnStatusBehavior.AddNewInstance(gameObject,
            _damage.Value, _slow.Value, _criticalChance.Value, _criticalDamage.Value);
    }

    private void OnBodyExit(Collider2D collider2D)
    {
        var enemy = IsTargetEnemyFunction(collider2D.gameObject);
        if (enemy == null)
            return;

        var statusOwner = enemy.GetComponent<StatusOwnerBehavior>();
        if (statusOwner == null)
            return;

        statusOwner.BurnStatusBehavior.RemoveInstance(gameObject);
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(_duration.Value);
        _particleSystem.Stop();
        _bodyBehavior.IsCollidingDisabled = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void OnNotify(ISubject<CollideData> subject, CollideData collideData)
    {
        if (ReferenceEquals(subject, _bodyBehavior))
        {
            switch (collideData.Type)
            {
                case CollideType.ENTER:
                    OnBodyEnter(collideData.TargetCollider2D);
                    break;
                case CollideType.EXIT:
                    OnBodyExit(collideData.TargetCollider2D);
                    break;
            }
        }
    }
}
