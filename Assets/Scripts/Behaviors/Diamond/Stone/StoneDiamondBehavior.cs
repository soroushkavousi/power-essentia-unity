using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoneDiamondBehavior : PeriodicDiamondBehavior, IObserver<HitParameters>
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(StoneDiamondBehavior) + Constants.HeaderEnd)]

    [SerializeField] private StoneDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private FallingStoneBehavior _sampleFallingStoneBehavior = default;
    [SerializeField] private Number _chance;

    public override void Initialize(Observable<DiamondKnowledgeState> state, Level level,
        DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(state, level, diamondOwnerBehavior);
        _chance = new(_level, _staticData.ChanceLevelInfo,
            min: 0f, max: 100f);
        _sampleFallingStoneBehavior = CreateFallingStone(OutBoxBehavior.Instance.Location1.gameObject);
    }

    protected override void DoActivationWork()
    {
        OwnerAttackerBehavior.Attach(this);
    }

    protected override void DoDeactivationWork()
    {
        OwnerAttackerBehavior.Detach(this);
    }

    private IEnumerator HandleHitEvent(HitParameters hitParameters)
    {
        if (Random.Range(0f, 100f) > _chance.Value)
            yield break;
        var targetEnemy = hitParameters.Destination;
        CreateFallingStone(targetEnemy);
        yield return new WaitForSeconds(0.1f);
        var closestEnemy = FindClosestEnemyToWall(targetEnemy);
        if (closestEnemy != null && closestEnemy.activeSelf == true)
            CreateFallingStone(closestEnemy);
    }

    private FallingStoneBehavior CreateFallingStone(GameObject targetEnemy)
    {
        var fallingStoneBehavior = Instantiate(_staticData.FallingStoneBehavior,
            DiamondEffectsParent);
        fallingStoneBehavior.Initialize(_level, targetEnemy, IsTargetEnemyFunction);

        return fallingStoneBehavior;
    }

    private GameObject FindClosestEnemyToWall(GameObject targetEnemy)
    {
        var targetDemon = targetEnemy.GetComponent<DemonBehavior>();
        var aliveDemons = new List<DemonBehavior>(WaveManagerBehavior.Instance.AliveEnemies);
        if (aliveDemons.Count == 1)
            return targetEnemy;
        aliveDemons.Remove(targetDemon);
        while (aliveDemons.Count != 0)
        {
            var indexOfClosestDemon = aliveDemons.Select(e => e.transform.position.x).ToList().IndexOfMin();
            var closestDemon = aliveDemons[indexOfClosestDemon];
            if (closestDemon != null && !closestDemon.GetComponent<HealthBehavior>().IsDead)
                return closestDemon.gameObject;
            aliveDemons.Remove(closestDemon);
        }
        return targetEnemy;
    }

    public void OnNotify(ISubject<HitParameters> subject, HitParameters hitParameters)
    {
        if (ReferenceEquals(subject, OwnerAttackerBehavior))
        {
            StartCoroutine(HandleHitEvent(hitParameters));
        }
    }

    protected override string GetDescription()
    {
        var description = $"" +
            $"It has a chance to summon two stones that fall into enemies and stun them." +
            $"\n\nThe first stone will hit the target enemy, and the second stone will hit the closest enemy to the wall." +
            $"\n\nYou can activate this diamond by clicking on its icon in your deck when fighting.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        var damageType = NoteUtils.AddColor(DamageType.PHYSICAL.ToString(), "red");
        var damageTypeShow = NoteUtils.ChangeSize($"Damage Type: {damageType}", NoteUtils.NumberSizeRatio);

        //------------------------------------------------

        var currentActiveTime = _activeTime.Value.ToLong();
        var nextActiveTime = _activeTime.NextLevelValue.ToLong();

        var currentActiveTimeShow = NoteUtils.AddColor(currentActiveTime + "s", "black");
        currentActiveTimeShow = NoteUtils.ChangeSize($"Active Time: {currentActiveTimeShow}", NoteUtils.NumberSizeRatio);
        var nextActiveTimeShow = NoteUtils.AddColor(nextActiveTime + "s", NoteUtils.UpgradeColor);
        nextActiveTimeShow = NoteUtils.ChangeSize($"({nextActiveTimeShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCooldownTime = _cooldownTime.Value.Round();
        var nextCooldownTime = _cooldownTime.NextLevelValue.Round();

        var currentCooldownTimeShow = NoteUtils.AddColor(currentCooldownTime + "s", "black");
        currentCooldownTimeShow = NoteUtils.ChangeSize($"Cooldown: {currentCooldownTimeShow}", NoteUtils.NumberSizeRatio);
        var nextCooldownTimeShow = NoteUtils.AddColor(nextCooldownTime + "s", NoteUtils.UpgradeColor);
        nextCooldownTimeShow = NoteUtils.ChangeSize($"({nextCooldownTimeShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentChance = _chance.Value.Round();
        var nextChance = _chance.NextLevelValue.Round();

        var currentChanceShow = NoteUtils.AddColor(currentChance + "%", "black");
        currentChanceShow = NoteUtils.ChangeSize($"Chance: {currentChanceShow}", NoteUtils.NumberSizeRatio);
        var nextChanceShow = NoteUtils.AddColor(nextChance + "%", NoteUtils.UpgradeColor);
        nextChanceShow = NoteUtils.ChangeSize($"({nextChanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentImpactDamage = _sampleFallingStoneBehavior.ImpactDamage.Value.ToLong();
        var nextImpactDamage = _sampleFallingStoneBehavior.ImpactDamage.NextLevelValue.ToLong();

        var currentImpactDamageShow = NoteUtils.AddColor(currentImpactDamage, "black");
        currentImpactDamageShow = NoteUtils.ChangeSize($"Impact Damage: {currentImpactDamageShow}", NoteUtils.NumberSizeRatio);
        var nextImpactDamageShow = NoteUtils.AddColor(nextImpactDamage + "dps", NoteUtils.UpgradeColor);
        nextImpactDamageShow = NoteUtils.ChangeSize($"({nextImpactDamageShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentStunDuration = _sampleFallingStoneBehavior.StunDuration.Value.Round();
        var nextStunDuration = _sampleFallingStoneBehavior.StunDuration.NextLevelValue.Round();

        var currentStunDurationShow = NoteUtils.AddColor(currentStunDuration + "s", "black");
        currentStunDurationShow = NoteUtils.ChangeSize($"Stun Duration: {currentStunDurationShow}", NoteUtils.NumberSizeRatio);
        var nextStunDurationShow = NoteUtils.AddColor(nextStunDuration + "s", NoteUtils.UpgradeColor);
        nextStunDurationShow = NoteUtils.ChangeSize($"({nextStunDurationShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCriticalChance = _sampleFallingStoneBehavior.CriticalChance.Value.ToLong();
        var nextCriticalChance = _sampleFallingStoneBehavior.CriticalChance.NextLevelValue.ToLong();

        var currentCriticalChanceShow = NoteUtils.AddColor(currentCriticalChance + "%", "black");
        currentCriticalChanceShow = NoteUtils.ChangeSize($"Critical Chance: {currentCriticalChanceShow}", NoteUtils.NumberSizeRatio);
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", NoteUtils.UpgradeColor);
        nextCriticalChanceShow = NoteUtils.ChangeSize($"({nextCriticalChanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCriticalDamage = _sampleFallingStoneBehavior.CriticalDamage.Value.ToLong();
        var nextCriticalDamage = _sampleFallingStoneBehavior.CriticalDamage.NextLevelValue.ToLong();

        var currentCriticalDamageShow = NoteUtils.AddColor(currentCriticalDamage + "%", "black");
        currentCriticalDamageShow = NoteUtils.ChangeSize($"Critical Damage: {currentCriticalDamageShow}", NoteUtils.NumberSizeRatio);
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", NoteUtils.UpgradeColor);
        nextCriticalDamageShow = NoteUtils.ChangeSize($"({nextCriticalDamageShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var statsDescription = $"" +
            $"{damageTypeShow}\n" +
            $"{currentActiveTimeShow}    {nextActiveTimeShow}\n" +
            $"{currentCooldownTimeShow}    {nextCooldownTimeShow}\n" +
            $"{currentChanceShow}    {nextChanceShow}\n" +
            $"{currentImpactDamageShow}    {nextImpactDamageShow}\n" +
            $"{currentStunDurationShow}    {nextStunDurationShow}\n" +
            $"{currentCriticalChanceShow}    {nextCriticalChanceShow}\n" +
            $"{currentCriticalDamageShow}    {nextCriticalDamageShow}";
        return statsDescription;
    }
}
