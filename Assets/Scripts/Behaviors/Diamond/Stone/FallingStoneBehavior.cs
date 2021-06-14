using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MovementBehavior))]
public class FallingStoneBehavior : MonoBehaviour
{
    [SerializeField] private FallingStoneStaticData _staticData = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private ThreePartAdvancedNumber _impactDamage = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private ThreePartAdvancedNumber _stunDuration = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private ThreePartAdvancedNumber _criticalChance = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _criticalDamage = new ThreePartAdvancedNumber();
    [SerializeField] private GameObject _enemy = default;
    [SerializeField] private bool _isSample = default;
    private MovementBehavior _movementBehavior = default;
    private AudioSource _audioSource = default;

    public ThreePartAdvancedNumber ImpactDamage => _impactDamage;
    public ThreePartAdvancedNumber StunDuration => _stunDuration;
    public ThreePartAdvancedNumber CriticalChance => _criticalChance;
    public ThreePartAdvancedNumber CriticalDamage => _criticalDamage;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; private set; }

    public void Initialize(GameObject enemy, Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        _audioSource = GetComponent<AudioSource>();

        _impactDamage.FeedData(_staticData.StartImpactDamage);
        _stunDuration.FeedData(_staticData.StartStunDuration);
        _criticalChance.FeedData(_staticData.StartCriticalChance);
        _criticalDamage.FeedData(_staticData.StartCriticalDamage);

        _enemy = enemy;
        IsTargetEnemyFunction = isTargetEnemyFunction;

        var enemyPosition = _enemy.transform.position;
        transform.position = new Vector2(
            _enemy.transform.position.x + _staticData.SpawnXOffset,
            _staticData.SpawnYPosition);

        _movementBehavior.FeedData(_staticData.MovementStaticData);
        _movementBehavior.TargetTransform = _enemy.transform;
        _isSample = enemy == OutBoxBehavior.Instance.Location1.gameObject;
        if (_isSample)
            return;
        _movementBehavior.OnReachActions.Add(ApplyStatusEffect);
        _movementBehavior.StartMoving();
    }

    private void Start()
    {
        if (_isSample)
            gameObject.SetActive(false);
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

        MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_staticData.HitSound, 0.5f);

        statusOwner.StunStatusBehavior.AddNewInstance(gameObject,
            _impactDamage.Value, _stunDuration.Value);

        Destroy(gameObject);
    }
}
