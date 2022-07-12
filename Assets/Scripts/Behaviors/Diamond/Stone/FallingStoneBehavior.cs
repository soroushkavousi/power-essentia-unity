using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MovementBehavior))]
public class FallingStoneBehavior : MonoBehaviour, IObserver<MovementChangeData>
{
    [SerializeField] private FallingStoneStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Number _impactDamage;
    [SerializeField] private Number _stunDuration;
    [SerializeField] private Number _criticalChance;
    [SerializeField] private Number _criticalDamage;
    [SerializeField] private GameObject _enemy = default;
    [SerializeField] private bool _isSample = default;
    protected Observable<int> _level = default;
    private MovementBehavior _movementBehavior = default;
    private AudioSource _audioSource = default;

    public Number ImpactDamage => _impactDamage;
    public Number StunDuration => _stunDuration;
    public Number CriticalChance => _criticalChance;
    public Number CriticalDamage => _criticalDamage;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; private set; }

    public void Initialize(Observable<int> level,
        GameObject enemy, Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        _audioSource = GetComponent<AudioSource>();

        _level = level;

        _stunDuration = new(_staticData.StunDuration, level,
            _staticData.StunDurationLevelPercentage);

        _impactDamage = new(_staticData.ImpactDamage, level,
            _staticData.ImpactDamageLevelPercentage, min: 0f);

        _criticalChance = new(_staticData.CriticalChance, level,
            _staticData.CriticalChanceLevelPercentage, min: 0f, max: 100f);

        _criticalDamage = new(_staticData.CriticalDamage, level,
            _staticData.CriticalDamageLevelPercentage, min: 0f);

        _enemy = enemy;
        IsTargetEnemyFunction = isTargetEnemyFunction;

        transform.position = new Vector2(
            _enemy.transform.position.x + _staticData.SpawnXOffset,
            _staticData.SpawnYPosition);

        _movementBehavior = GetComponent<MovementBehavior>();
        _movementBehavior.FeedData(_staticData.MovementStaticData, _level);
        _movementBehavior.Attach(this);
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
        _movementBehavior.MoveToTarget(_enemy.transform);
    }

    private void ApplyStatusEffect()
    {
        if (_enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        var statusOwner = _enemy.GetComponent<StatusOwnerBehavior>();
        if (statusOwner == null)
        {
            Destroy(gameObject);
            return;
        }

        var healthBehavior = _enemy.GetComponent<HealthBehavior>();
        if (healthBehavior == null || healthBehavior.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_staticData.HitSound, 0.5f);

        statusOwner.StunStatusBehavior.AddNewInstance(gameObject,
            _impactDamage.Value, _stunDuration.Value);

        Destroy(gameObject);
    }

    public void OnNotify(ISubject<MovementChangeData> subject, MovementChangeData data)
    {
        if (ReferenceEquals(subject, _movementBehavior))
        {
            if (data.ChangeState == MovementChangeState.REACHED)
                ApplyStatusEffect();
        }
    }
}
