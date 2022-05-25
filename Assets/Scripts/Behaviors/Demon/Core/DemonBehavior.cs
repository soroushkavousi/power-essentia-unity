using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(HealthBehavior))]
[RequireComponent(typeof(StatusOwnerBehavior))]

public class DemonBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private DemonName _name = default;
    [SerializeField] private UnityEvent _InitializeEvent;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private DemonStaticData _staticData = default;
    [SerializeField] private OnePartAdvancedNumber _level = new OnePartAdvancedNumber();
    [SerializeField] private bool _isInAttackArea = default;

    private BodyBehavior _bodyBehavior = default;
    private HealthBehavior _healthBehavior = default;
    private StatusOwnerBehavior _statusOwnerBehavior = default;
    private ResourceBox _gameResourceBox = default;
    private static int _temp = 0;

    public DemonName Name => _name;
    public bool IsInAttackArea => _isInAttackArea;
    public OnePartAdvancedNumber Level => _level;

    public void Initialize(int level)
    {
        _level.FeedData(level);
        _InitializeEvent.Invoke();
        _temp++;
        WaveManagerBehavior.Instance.OnNewSpawnedInvaderActions.CallActionsSafely(this);
    }

    public void FeedData(DemonStaticData staticData)
    {
        _staticData = staticData;
        var level = _level.IntValue;

        _bodyBehavior = GetComponent<BodyBehavior>();
        _bodyBehavior.FeedData();
        _bodyBehavior.IsColliderDisabled = true;
        _bodyBehavior.OnUnconstrainedEnterActions.Add(ComeIntoAction);

        _healthBehavior = GetComponent<HealthBehavior>();
        //_healthBehavior.FeedData(_staticData.HealthData);
        //_healthBehavior.Health.Base.Change(level * _staticData.HealthPerLevel,
        //    name, HealthChangeType.LEVEL);

        //_resistanceBehavior.PhysicalResistance.Base.Change(level * _staticData.PhysicalResistancePerLevel,
        //    name, ResistanceChangeType.LEVEL);
        //_resistanceBehavior.MagicResistance.Base.Change(level * _staticData.MagicResistancePerLevel,
        //    name, ResistanceChangeType.LEVEL);
        //_resistanceBehavior.StatusResistance.Base.Change(level * _staticData.StatusResistancePerLevel,
        //    name, ResistanceChangeType.LEVEL);

        _statusOwnerBehavior = GetComponent<StatusOwnerBehavior>();
        _statusOwnerBehavior.FeedData();
        _statusOwnerBehavior.BurnStatusBehavior.FeedData();
        _statusOwnerBehavior.StunStatusBehavior.FeedData();

        _gameResourceBox = PlayerBehavior.Main.DynamicData.ResourceBox;
        if (GetComponent<LizardBehavior>() != null)
            StartCoroutine(Test());
    }

    private void InitializeHealthBehavior()
    {
        var maxHealth = new MaxHealth(_staticData.HealthData.StartHealth);
        maxHealth.AddModifier(new MaxHealthLevelModifier(1, _staticData.HealthPerLevel));

        var physicalResistance = new PhysicalResistance(_staticData.HealthData.ResistanceStaticData.StartPhysicalResistance);
        var physicalDamageModifier = new CurrentHealthPhysicalDamageModifier(physicalResistance);

        var magicResistance = new MagicResistance(_staticData.HealthData.ResistanceStaticData.StartMagicResistance);
        var magicDamageModifier = new CurrentHealthMagicDamageModifier(magicResistance);

        var currentHealth = new CurrentHealth(maxHealth, physicalDamageModifier, magicDamageModifier);

        _healthBehavior = GetComponent<HealthBehavior>();
        var death = new Death(_healthBehavior, _staticData.HealthData.DeathVfxPrefab);
        death.Attach(this);

        _healthBehavior.FeedData(maxHealth, currentHealth, death);
    }

    public void Update(ISubject subject)
    {
        //switch (subject)
        //{
        //    case Death death
        //}
        //if (subject.GetType().IsSubclassOf(typeof(Death)))
        //{
        //    if(!_healthBehavior.Death.IsDead)
        //        return;

        //}
        //if (_currentHealth.Value <= 0)
        //    Die();
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(2);
        //_healthBehavior.Health.CurrentValueChangeCommands.Add(
        //    new NumberChangeCommand(-3000, name, HealthChangeType.PHYSICAL_DAMAGE.ToString()));
    }

    private void ComeIntoAction(Collider2D otherCollider)
    {
        if(otherCollider.gameObject.name == "DemonsAttackArea")
        {
            _isInAttackArea = true;
            _bodyBehavior.IsColliderDisabled = false;
        }
    }

    protected virtual void FlipXRandomly()
    {
        if (UnityEngine.Random.value >= 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            transform.RotateAround(transform.position, transform.up, 180f);
            //_movementBehavior.Direction = 1;
        }
    }

    private void OnDestroy()
    {
        if (LoseSystemBehavior.Instance == null || LoseSystemBehavior.Instance.Lose)
            return;

        if (WinSystemBehavior.Instance == null || WinSystemBehavior.Instance.Win)
            return;

        if (_healthBehavior.CurrentHealth.Value > 0)
            return;

        int rewardCoinCount;
        switch (_name)
        {
            case DemonName.LIZARD:
                rewardCoinCount = 500;
                break;
            case DemonName.BLACK_MAGE:
                rewardCoinCount = 2500;
                break;
            default:
                rewardCoinCount = 150;
                break;
        }
        rewardCoinCount += 250 * _level.IntValue;
        _gameResourceBox.ResourceBunches[ResourceType.COIN].Change(rewardCoinCount, name, $"INVADER_DEAD");

        MusicPlayerBehavior.Instance.PlayEnemyDeathGoldRewardSound();
        WaveManagerBehavior.Instance.OnNewDeadInvaderActions.CallActionsSafely(this);
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log($"2222 Teeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeest");
    }
}
