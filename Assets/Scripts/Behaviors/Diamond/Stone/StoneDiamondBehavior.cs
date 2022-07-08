using Assets.Scripts.Models;
using UnityEngine;

public class StoneDiamondBehavior : DiamondBehavior, IObserver<HitParameters>
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(StoneDiamondBehavior) + Constants.HeaderEnd)]

    [SerializeField] private StoneDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private FallingStoneBehavior _sampleFallingStoneBehavior = default;
    [SerializeField] private Number _chance;

    public override void Initialize(Observable<DiamondKnowledgeState> state, Observable<int> level,
        DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(state, level, diamondOwnerBehavior);
        _chance = new(_staticData.Chance, _level, _staticData.ChanceLevelPercentage,
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

    private void HandleHitEvent(HitParameters hitParameters)
    {
        if (Random.value * 100 > _chance.Value)
            return;
        CreateFallingStone(hitParameters.Destination);
    }

    private FallingStoneBehavior CreateFallingStone(GameObject targetEnemy)
    {
        var fallingStoneBehavior = Instantiate(_staticData.FallingStoneBehavior,
            DiamondEffectsParent);
        fallingStoneBehavior.Initialize(_level, targetEnemy, IsTargetEnemyFunction);

        return fallingStoneBehavior;
    }

    public void OnNotify(ISubject<HitParameters> subject, HitParameters hitParameters)
    {
        if (ReferenceEquals(subject, OwnerAttackerBehavior))
        {
            HandleHitEvent(hitParameters);
        }
    }

    protected override string GetDescription()
    {
        //------------------------------------------------

        var currentChacne = _chance.Value;
        var nextChance = _chance.NextLevelValue;

        var currentChanceShow = NoteUtils.AddColor(currentChacne + "%", "black");
        currentChanceShow = NoteUtils.ChangeSize($"Chance: {currentChanceShow}", NoteUtils.NumberSizeRatio);
        var nextChanceShow = NoteUtils.AddColor(nextChance + "%", NoteUtils.UpgradeColor);
        nextChanceShow = NoteUtils.ChangeSize($"({nextChanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentImpactDamage = _sampleFallingStoneBehavior.ImpactDamage.Value.ToLong();
        var nextImpactDamage = _sampleFallingStoneBehavior.ImpactDamage.NextLevelValue.ToLong();

        var currentImpactDamageShow = NoteUtils.AddColor(currentImpactDamage + "dps", "black");
        currentImpactDamageShow = NoteUtils.ChangeSize($"Impact Damage: {currentImpactDamageShow}", NoteUtils.NumberSizeRatio);
        var nextImpactDamageShow = NoteUtils.AddColor(nextImpactDamage + "dps", NoteUtils.UpgradeColor);
        nextImpactDamageShow = NoteUtils.ChangeSize($"({nextImpactDamageShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentStunDuration = _sampleFallingStoneBehavior.StunDuration.Value.ToLong();
        var nextStunDuration = _sampleFallingStoneBehavior.StunDuration.Value.ToLong();

        var currentStunDurationShow = NoteUtils.AddColor(currentStunDuration + "s", "black");
        currentStunDurationShow = NoteUtils.ChangeSize($"Stun Duration: {currentStunDurationShow}", NoteUtils.NumberSizeRatio);
        var nextStunDurationShow = NoteUtils.AddColor(nextStunDuration + "s", NoteUtils.UpgradeColor);
        nextStunDurationShow = NoteUtils.ChangeSize($"({nextStunDurationShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCriticalChance = _sampleFallingStoneBehavior.CriticalChance.Value.ToLong();
        var nextCriticalChance = _sampleFallingStoneBehavior.CriticalChance.Value.ToLong();

        var currentCriticalChanceShow = NoteUtils.AddColor(currentCriticalChance + "%", "black");
        currentCriticalChanceShow = NoteUtils.ChangeSize($"Critical Chance: {currentCriticalChanceShow}", NoteUtils.NumberSizeRatio);
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", NoteUtils.UpgradeColor);
        nextCriticalChanceShow = NoteUtils.ChangeSize($"({nextCriticalChanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCriticalDamage = _sampleFallingStoneBehavior.CriticalDamage.Value.ToLong();
        var nextCriticalDamage = _sampleFallingStoneBehavior.CriticalDamage.Value.ToLong();

        var currentCriticalDamageShow = NoteUtils.AddColor(currentCriticalDamage + "%", "black");
        currentCriticalDamageShow = NoteUtils.ChangeSize($"Critical Damage: {currentCriticalDamageShow}", NoteUtils.NumberSizeRatio);
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", NoteUtils.UpgradeColor);
        nextCriticalDamageShow = NoteUtils.ChangeSize($"({nextCriticalDamageShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var description = $"" +
            $"It has a chance to fall a stone on the enemy target.\n" +
            $"\nStats:\n" +
            $"   - {currentChanceShow}    {nextChanceShow}\n" +
            $"   - {currentImpactDamageShow}    {nextImpactDamageShow}\n" +
            $"   - {currentStunDurationShow}    {nextStunDurationShow}\n" +
            $"   - {currentCriticalChanceShow}    {nextCriticalChanceShow}\n" +
            $"   - {currentCriticalDamageShow}    {nextCriticalDamageShow}";
        return description;
    }
}
