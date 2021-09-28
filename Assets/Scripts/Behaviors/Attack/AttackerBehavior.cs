using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AttackerBehavior : MonoBehaviour
{
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private bool _isAttacking = default;
    [SerializeField] private ThreePartAdvancedNumber _attackDamage = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private ThreePartAdvancedNumber _attackSpeed = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private ThreePartAdvancedNumber _criticalChance = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _criticalDamage = new ThreePartAdvancedNumber();
    [SerializeField] private GameObject _currentEnemy = default;
    private AttackerStaticData _staticData = default;
    private readonly string _attackSpeedName = "AttackSpeed";
    private Animator _animator = default;
    private MovementBehavior _movementBehavior = default;
    private AudioSource _audioSource = default;

    public bool IsAttacking => _isAttacking;
    public ThreePartAdvancedNumber AttackDamage => _attackDamage;
    public ThreePartAdvancedNumber AttackSpeed => _attackSpeed;
    public ThreePartAdvancedNumber CriticalChance => _criticalChance;
    public ThreePartAdvancedNumber CriticalDamage => _criticalDamage;
    public AudioClip AttackSound => _staticData.AttackSound;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; set; }
    public OrderedList<Action<GameObject>> OnStartAttackingActions { get; } = new OrderedList<Action<GameObject>>();
    public OrderedList<Action> OnAttackingActions { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnStopAttackingActions { get; } = new OrderedList<Action>();
    public OrderedList<Action<HitParameters>> OnHitActions { get; } = new OrderedList<Action<HitParameters>>();
    public GameObject CurrentEnemy { get => _currentEnemy; set => _currentEnemy = value; }

    public void FeedData(AttackerStaticData staticData,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _animator = GetComponent<Animator>();
        _movementBehavior = GetComponent<MovementBehavior>();
        _audioSource = GetComponent<AudioSource>();

        _staticData = staticData;
        _attackDamage.FeedData(_staticData.StartDamage);
        _attackSpeed.FeedData(_staticData.StartSpeed);
        _attackSpeed.Current.OnNewValueActions.Add(SubmitAttackSpeedChange);
        _criticalChance.FeedData(_staticData.StartCriticalChance);
        _criticalDamage.FeedData(_staticData.StartCriticalDamage);
        _animator.SetFloat(_attackSpeedName, _attackSpeed.Value);
        IsTargetEnemyFunction = isTargetEnemyFunction;
    }

    public void StartAttacking()
    {
        if (_isAttacking == true)
            return;
        _isAttacking = true;
        if (_movementBehavior != null)
            _movementBehavior.StopMoving();
        OnStartAttackingActions.CallActionsSafely(_currentEnemy);
    }

    private void Attack()
    {
        //if(_staticData.AttackSound != null)
        //    _audioSource.PlayOneShot(_staticData.AttackSound, 0.5f);
        //_audioSource.Play();
            
        OnAttackingActions.CallActionsSafely();
    }

    public void StopAttacking()
    {
        if (_isAttacking == false)
            return;
        _isAttacking = false;
        OnStopAttackingActions.CallActionsSafely();
    }

    public void HandleHitEvent(HitParameters hitParameters)
    {
        OnHitActions.CallActionsSafely(hitParameters);
    }

    private void SubmitAttackSpeedChange(NumberChangeCommand changeCommand)
    {
        _animator.SetFloat(_attackSpeedName, _attackSpeed.Value);
    }
}
