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
            $"It has a chance to summon two stones that fall into enemies and stun them.\n" +
            $"The first stone will hit the target enemy, and the second stone will hit the closest enemy to the wall.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        var damageType = NoteUtils.AddColor(DamageType.PHYSICAL.ToString(), "red");
        var damageTypeShow = NoteUtils.ChangeSize($"Damage Type: {damageType}", NoteUtils.NumberSizeRatio);

        //------------------------------------------------

        var currentChacne = _chance.Value.ToLong();
        var nextChance = _chance.NextLevelValue.ToLong();

        var currentChanceShow = NoteUtils.AddColor(currentChacne + "%", "black");
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

        var currentStunDuration = _sampleFallingStoneBehavior.StunDuration.Value.ToLong();
        var nextStunDuration = _sampleFallingStoneBehavior.StunDuration.NextLevelValue.ToLong();

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
            $"{currentChanceShow}    {nextChanceShow}\n" +
            $"{currentImpactDamageShow}    {nextImpactDamageShow}\n" +
            $"{currentStunDurationShow}    {nextStunDurationShow}\n" +
            $"{currentCriticalChanceShow}    {nextCriticalChanceShow}\n" +
            $"{currentCriticalDamageShow}    {nextCriticalDamageShow}";
        return statsDescription;
    }
}
