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
        var description = $"" +
            $"It has a chance to spawn a ground fire when an enemy is hit." +
            $" The ground fire will burn any enemies who walk on it.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        //------------------------------------------------

        var currentChacne = _chance.Value;
        var nextChance = _chance.NextLevelValue;

        var currentChanceShow = NoteUtils.AddColor(currentChacne + "%", "black");
        currentChanceShow = NoteUtils.ChangeSize($"Chance: {currentChanceShow}", NoteUtils.NumberSizeRatio);
        var nextChanceShow = NoteUtils.AddColor(nextChance + "%", NoteUtils.UpgradeColor);
        nextChanceShow = NoteUtils.ChangeSize($"({nextChanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentDuration = _sampleGroundFireBehavior.Duration.Value;
        var nextDuration = _sampleGroundFireBehavior.Duration.NextLevelValue;

        var currentDurationShow = NoteUtils.AddColor(currentDuration + "s", "black");
        currentDurationShow = NoteUtils.ChangeSize($"Duration: {currentDurationShow}", NoteUtils.NumberSizeRatio);
        var nextDurationShow = NoteUtils.AddColor(nextDuration + "s", NoteUtils.UpgradeColor);
        nextDurationShow = NoteUtils.ChangeSize($"({nextDurationShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentDamage = _sampleGroundFireBehavior.Damage.Value;
        var nextDamage = _sampleGroundFireBehavior.Damage.NextLevelValue;

        var currentDamageShow = NoteUtils.AddColor(currentDamage + "dps", "black");
        currentDamageShow = NoteUtils.ChangeSize($"Damage: {currentDamageShow}", NoteUtils.NumberSizeRatio);
        var nextDamageShow = NoteUtils.AddColor(nextDamage + "dps", NoteUtils.UpgradeColor);
        nextDamageShow = NoteUtils.ChangeSize($"({nextDamageShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentSlow = _sampleGroundFireBehavior.Slow.Value;
        var nextSlow = _sampleGroundFireBehavior.Slow.NextLevelValue;

        var currentSlowShow = NoteUtils.AddColor(currentSlow + "%", "black");
        currentSlowShow = NoteUtils.ChangeSize($"Slow: {currentSlowShow}", NoteUtils.NumberSizeRatio);
        var nextSlowShow = NoteUtils.AddColor(nextSlow + "%", NoteUtils.UpgradeColor);
        nextSlowShow = NoteUtils.ChangeSize($"({nextSlowShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCriticalChance = _sampleGroundFireBehavior.CriticalChance.Value;
        var nextCriticalChance = _sampleGroundFireBehavior.CriticalChance.NextLevelValue;

        var currentCriticalChanceShow = NoteUtils.AddColor(currentCriticalChance + "%", "black");
        currentCriticalChanceShow = NoteUtils.ChangeSize($"Critical Chance: {currentCriticalChanceShow}", NoteUtils.NumberSizeRatio);
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", NoteUtils.UpgradeColor);
        nextCriticalChanceShow = NoteUtils.ChangeSize($"({nextCriticalChanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCriticalDamage = _sampleGroundFireBehavior.CriticalDamage.Value;
        var nextCriticalDamage = _sampleGroundFireBehavior.CriticalDamage.NextLevelValue;

        var currentCriticalDamageShow = NoteUtils.AddColor(currentCriticalDamage + "%", "black");
        currentCriticalDamageShow = NoteUtils.ChangeSize($"Critical Damage: {currentCriticalDamageShow}", NoteUtils.NumberSizeRatio);
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", NoteUtils.UpgradeColor);
        nextCriticalDamageShow = NoteUtils.ChangeSize($"({nextCriticalDamageShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var statsDescription = $"" +
            $"{currentChanceShow}    {nextChanceShow}\n" +
            $"{currentDurationShow}    {nextDurationShow}\n" +
            $"{currentDamageShow}    {nextDamageShow}\n" +
            $"{currentSlowShow}    {nextSlowShow}\n" +
            $"{currentCriticalChanceShow}    {nextCriticalChanceShow}\n" +
            $"{currentCriticalDamageShow}    {nextCriticalDamageShow}";
        return statsDescription;
    }
}
