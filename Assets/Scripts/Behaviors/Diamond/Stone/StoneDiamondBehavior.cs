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

    private void Awake()
    {
        base.FeedData(_staticData);
    }

    public override void Initialize(Observable<DiamondKnowledgeState> state, Observable<int> level,
        DiamondOwnerBehavior diamondOwnerBehavior)
    {
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
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentChacne = _chance.Value;
        var nextChance = _chance.NextLevelValue;

        var currentChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentChacne + "%", "black"));
        var nextChanceShow = NoteUtils.AddColor(nextChance + "%", upgradeColor);

        //------------------------------------------------

        var currentImpactDamage = _sampleFallingStoneBehavior.ImpactDamage.Value.ToLong();
        var nextImpactDamage = _sampleFallingStoneBehavior.ImpactDamage.NextLevelValue.ToLong();

        var currentImpactDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentImpactDamage + "dps", "black"));
        var nextImpactDamageShow = NoteUtils.AddColor(nextImpactDamage + "dps", upgradeColor);

        //------------------------------------------------

        var currentStunDuration = _sampleFallingStoneBehavior.StunDuration.Value.ToLong();
        var nextStunDuration = _sampleFallingStoneBehavior.StunDuration.Value.ToLong();

        var currentStunDurationShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentStunDuration + "s", "black"));
        var nextStunDurationShow = NoteUtils.AddColor(nextStunDuration + "s", upgradeColor);

        //------------------------------------------------

        var currentCriticalChance = _sampleFallingStoneBehavior.CriticalChance.Value.ToLong();
        var nextCriticalChance = _sampleFallingStoneBehavior.CriticalChance.Value.ToLong();

        var currentCriticalChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalChance + "%", "black"));
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", upgradeColor);

        //------------------------------------------------

        var currentCriticalDamage = _sampleFallingStoneBehavior.CriticalDamage.Value.ToLong();
        var nextCriticalDamage = _sampleFallingStoneBehavior.CriticalDamage.Value.ToLong();

        var currentCriticalDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalDamage + "%", "black"));
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"It has a chance to fall a stone on the enemy target.\n\n" +
            $" Chance: {currentChanceShow} (next: {nextChanceShow})\n" +
            $" Impact Damage: {currentImpactDamageShow} (next: {nextImpactDamageShow})\n" +
            $" Duration: {currentStunDurationShow} (next: {nextStunDurationShow})\n" +
            $" Critical chance: {currentCriticalChanceShow} (next: {nextCriticalChanceShow})\n" +
            $" Critical damage: {currentCriticalDamageShow} (next: {nextCriticalDamageShow})";
        return description;
    }
}
