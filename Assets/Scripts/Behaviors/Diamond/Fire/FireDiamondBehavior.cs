using Assets.Scripts.Models;
using UnityEngine;

public class FireDiamondBehavior : DiamondBehavior, IObserver<HitParameters>
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(FireDiamondBehavior) + Constants.HeaderEnd)]

    [SerializeField] private FireDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private GroundFireBehavior _sampleGroundFireBehavior = default;
    [SerializeField] private Number _chance;

    public override void Initialize(Observable<DiamondKnowledgeState> state,
        Observable<int> level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(state, level, diamondOwnerBehavior);
        _chance = new(_staticData.Chance, _level, _staticData.ChanceLevelPercentage,
            min: 0f, max: 100f);
        _sampleGroundFireBehavior = CreateGroundFire(OutBoxBehavior.Instance.Location1.gameObject);
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
        CreateGroundFire(hitParameters.Destination);
    }

    private GroundFireBehavior CreateGroundFire(GameObject targetEnemy)
    {
        var groundFireBehavior = Instantiate(_staticData.GroundFireBehavior,
            DiamondEffectsParent);
        groundFireBehavior.Initialize(_level, targetEnemy, IsTargetEnemyFunction);

        return groundFireBehavior;
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

        var currentDuration = _sampleGroundFireBehavior.Duration.Value;
        var nextDuration = _sampleGroundFireBehavior.Duration.NextLevelValue;

        var currentDurationShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentDuration + "s", "black"));
        var nextDurationShow = NoteUtils.AddColor(nextDuration + "s", upgradeColor);

        //------------------------------------------------

        var currentDamage = _sampleGroundFireBehavior.Damage.Value;
        var nextDamage = _sampleGroundFireBehavior.Damage.NextLevelValue;

        var currentDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentDamage + "dps", "black"));
        var nextDamageShow = NoteUtils.AddColor(nextDamage + "dps", upgradeColor);

        //------------------------------------------------

        var currentSlow = _sampleGroundFireBehavior.Slow.Value;
        var nextSlow = _sampleGroundFireBehavior.Slow.NextLevelValue;

        var currentSlowShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentSlow + "%", "black"));
        var nextSlowShow = NoteUtils.AddColor(nextSlow + "%", upgradeColor);

        //------------------------------------------------

        var currentCriticalChance = _sampleGroundFireBehavior.CriticalChance.Value;
        var nextCriticalChance = _sampleGroundFireBehavior.CriticalChance.NextLevelValue;

        var currentCriticalChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalChance + "%", "black"));
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", upgradeColor);

        //------------------------------------------------

        var currentCriticalDamage = _sampleGroundFireBehavior.CriticalDamage.Value;
        var nextCriticalDamage = _sampleGroundFireBehavior.CriticalDamage.NextLevelValue;

        var currentCriticalDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalDamage + "%", "black"));
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"It has a chance to spawn a ground fire on bow hit.\n\n" +
            $" Chance: {currentChanceShow} (next: {nextChanceShow})\n" +
            $" Duration: {currentDurationShow} (next: {nextDurationShow})\n" +
            $" Damage: {currentDamageShow} (next: {nextDamageShow})\n" +
            $" Slow: {currentSlowShow} (next: {nextSlowShow})\n" +
            $" Critical chance: {currentCriticalChanceShow} (next: {nextCriticalChanceShow})\n" +
            $" Critical damage: {currentCriticalDamageShow} (next: {nextCriticalDamageShow})";
        return description;
    }
}
