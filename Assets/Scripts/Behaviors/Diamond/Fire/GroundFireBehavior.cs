using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(AudioSource))]
public class GroundFireBehavior : MonoBehaviour
{
    [SerializeField] private GroundFireStaticData _staticData = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private ThreePartAdvancedNumber _duration = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _damage = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _slow = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _criticalChance = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _criticalDamage = new ThreePartAdvancedNumber();
    [SerializeField] private GameObject _enemy = default;
    [SerializeField] private bool _isSample = default;
    private BodyBehavior _bodyBehavior = default;
    private ParticleSystem _particleSystem = default;
    private AudioSource _audioSource = default;

    public ThreePartAdvancedNumber Duration => _duration;
    public ThreePartAdvancedNumber Damage => _damage;
    public ThreePartAdvancedNumber Slow => _slow;
    public ThreePartAdvancedNumber CriticalChance => _criticalChance;
    public ThreePartAdvancedNumber CriticalDamage => _criticalDamage;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; private set; }

    public void Initialize(GameObject enemy, Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        _bodyBehavior = GetComponent<BodyBehavior>();

        _duration.FeedData(_staticData.StartDuration);
        _damage.FeedData(_staticData.StartDamage);
        _slow.FeedData(_staticData.StartSlow);
        _criticalChance.FeedData(_staticData.StartCriticalChance);
        _criticalDamage.FeedData(_staticData.StartCriticalDamage);

        _enemy = enemy;
        IsTargetEnemyFunction = isTargetEnemyFunction;

        transform.position = _enemy.transform.position + (Vector3)_staticData.SpawnOffset;

        _bodyBehavior.FeedData();
        _isSample = enemy == OutBoxBehavior.Instance.Location1.gameObject;
        if (_isSample)
            return;

        _bodyBehavior.OnEnterActions.Add(OnBodyEnter);
        _bodyBehavior.OnExitActions.Add(OnBodyExit);
        StartCoroutine(DestroyAfterLifetime());
    }

    private void Start()
    {
        if (_isSample)
            gameObject.SetActive(false);

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

        statusOwner.BurnStatusBehavior.AddNewInstance(gameObject, _damage, _slow, _criticalChance, _criticalDamage);
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
        _bodyBehavior.IsColliderDisabled = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
