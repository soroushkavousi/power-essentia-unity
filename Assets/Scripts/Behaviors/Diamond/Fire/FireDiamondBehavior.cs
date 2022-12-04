using Assets.Scripts.Models;
using UnityEngine;

public class FireDiamondBehavior : PeriodicDiamondBehavior, IObserver<HitParameters>
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(FireDiamondBehavior) + Constants.HeaderEnd)]

    [SerializeField] private FireDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private GroundFireBehavior _sampleGroundFireBehavior = default;
    [SerializeField] private Number _chance;

    public override void Initialize(Observable<DiamondKnowledgeState> state,
        Level level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(state, level, diamondOwnerBehavior);
        _chance = new(_level, _staticData.ChanceLevelInfo,
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
            $"\n\nThe ground fire will burn any enemies who walk on it." +
            $"\n\nYou can activate this diamond by clicking on its icon in your deck when fighting.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        var damageType = NoteUtils.AddColor(DamageType.MAGIC.ToString(), "#9f00e3");
        var damageTypeShow = NoteUtils.ChangeSize($"Damage Type: {damageType}", NoteUtils.NumberSizeRatio);

        //------------------------------------------------

        var currentActiveTime = _activeTime.Value.ToLong();
        var currentActiveTimeShow = NoteUtils.AddColor(currentActiveTime + "s", "black");
        currentActiveTimeShow = NoteUtils.ChangeSize($"Active Time: {currentActiveTimeShow}", NoteUtils.NumberSizeRatio);

        var nextActiveTimeShow = "";
        if (!_level.IsMax)
        {
            var nextActiveTime = _activeTime.NextLevelValue.ToLong();
            nextActiveTimeShow = NoteUtils.AddColor(nextActiveTime + "s", NoteUtils.UpgradeColor);
            nextActiveTimeShow = NoteUtils.ChangeSize($"({nextActiveTimeShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentCooldownTime = _cooldownTime.Value.Round();
        var currentCooldownTimeShow = NoteUtils.AddColor(currentCooldownTime + "s", "black");
        currentCooldownTimeShow = NoteUtils.ChangeSize($"Cooldown: {currentCooldownTimeShow}", NoteUtils.NumberSizeRatio);

        var nextCooldownTimeShow = "";
        if (!_level.IsMax)
        {
            var nextCooldownTime = _cooldownTime.NextLevelValue.Round();
            nextCooldownTimeShow = NoteUtils.AddColor(nextCooldownTime + "s", NoteUtils.UpgradeColor);
            nextCooldownTimeShow = NoteUtils.ChangeSize($"({nextCooldownTimeShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentChance = _chance.Value.Round();
        var currentChanceShow = NoteUtils.AddColor(currentChance + "%", "black");
        currentChanceShow = NoteUtils.ChangeSize($"Chance: {currentChanceShow}", NoteUtils.NumberSizeRatio);

        var nextChanceShow = "";
        if (!_level.IsMax)
        {
            var nextChance = _chance.NextLevelValue.Round();
            nextChanceShow = NoteUtils.AddColor(nextChance + "%", NoteUtils.UpgradeColor);
            nextChanceShow = NoteUtils.ChangeSize($"({nextChanceShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentDuration = _sampleGroundFireBehavior.Duration.Value.Round();
        var currentDurationShow = NoteUtils.AddColor(currentDuration + "s", "black");
        currentDurationShow = NoteUtils.ChangeSize($"Duration: {currentDurationShow}", NoteUtils.NumberSizeRatio);

        var nextDurationShow = "";
        if (!_level.IsMax)
        {
            var nextDuration = _sampleGroundFireBehavior.Duration.NextLevelValue.Round();
            nextDurationShow = NoteUtils.AddColor(nextDuration + "s", NoteUtils.UpgradeColor);
            nextDurationShow = NoteUtils.ChangeSize($"({nextDurationShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentDamage = _sampleGroundFireBehavior.Damage.Value.ToLong();
        var currentDamageShow = NoteUtils.AddColor(currentDamage + "dps", "black");
        currentDamageShow = NoteUtils.ChangeSize($"Damage: {currentDamageShow}", NoteUtils.NumberSizeRatio);

        var nextDamageShow = "";
        if (!_level.IsMax)
        {
            var nextDamage = _sampleGroundFireBehavior.Damage.NextLevelValue.ToLong();
            nextDamageShow = NoteUtils.AddColor(nextDamage + "dps", NoteUtils.UpgradeColor);
            nextDamageShow = NoteUtils.ChangeSize($"({nextDamageShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentSlow = _sampleGroundFireBehavior.Slow.Value.ToLong();
        var currentSlowShow = NoteUtils.AddColor(currentSlow + "%", "black");
        currentSlowShow = NoteUtils.ChangeSize($"Slow: {currentSlowShow}", NoteUtils.NumberSizeRatio);

        var nextSlowShow = "";
        if (!_level.IsMax)
        {
            var nextSlow = _sampleGroundFireBehavior.Slow.NextLevelValue.ToLong();
            nextSlowShow = NoteUtils.AddColor(nextSlow + "%", NoteUtils.UpgradeColor);
            nextSlowShow = NoteUtils.ChangeSize($"({nextSlowShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentCriticalChance = _sampleGroundFireBehavior.CriticalChance.Value.ToLong();
        var currentCriticalChanceShow = NoteUtils.AddColor(currentCriticalChance + "%", "black");
        currentCriticalChanceShow = NoteUtils.ChangeSize($"Critical Chance: {currentCriticalChanceShow}", NoteUtils.NumberSizeRatio);

        var nextCriticalChanceShow = "";
        if (!_level.IsMax)
        {
            var nextCriticalChance = _sampleGroundFireBehavior.CriticalChance.NextLevelValue.ToLong();
            nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", NoteUtils.UpgradeColor);
            nextCriticalChanceShow = NoteUtils.ChangeSize($"({nextCriticalChanceShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentCriticalDamage = _sampleGroundFireBehavior.CriticalDamage.Value.ToLong();
        var currentCriticalDamageShow = NoteUtils.ConvertCriticalDamageToXFormat(currentCriticalDamage);
        currentCriticalDamageShow = NoteUtils.AddColor(currentCriticalDamageShow, "black");
        currentCriticalDamageShow = NoteUtils.ChangeSize($"Critical Damage: {currentCriticalDamageShow}", NoteUtils.NumberSizeRatio);

        var nextCriticalDamageShow = "";
        if (!_level.IsMax)
        {
            var nextCriticalDamage = _sampleGroundFireBehavior.CriticalDamage.NextLevelValue.ToLong();
            nextCriticalDamageShow = NoteUtils.ConvertCriticalDamageToXFormat(nextCriticalDamage);
            nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamageShow, NoteUtils.UpgradeColor);
            nextCriticalDamageShow = NoteUtils.ChangeSize($"({nextCriticalDamageShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var statsDescription = $"" +
            $"{damageTypeShow}\n" +
            $"{currentActiveTimeShow}    {nextActiveTimeShow}\n" +
            $"{currentCooldownTimeShow}    {nextCooldownTimeShow}\n" +
            $"{currentChanceShow}    {nextChanceShow}\n" +
            $"{currentDurationShow}    {nextDurationShow}\n" +
            $"{currentDamageShow}    {nextDamageShow}\n" +
            $"{currentSlowShow}    {nextSlowShow}\n" +
            $"{currentCriticalChanceShow}    {nextCriticalChanceShow}\n" +
            $"{currentCriticalDamageShow}    {nextCriticalDamageShow}";
        return statsDescription;
    }
}
