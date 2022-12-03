using UnityEngine;

public class BowDiamondBehavior : PermanentDiamondBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(BowDiamondBehavior) + Constants.HeaderEnd)]
    [SerializeField] private BowDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private RangeWeaponBehavior _bowWeaponBehavior = default;
    private RangeAttackerBehavior _ownerRangeAttackerBehavior = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public RangeWeaponBehavior BowWeaponBehavior => _bowWeaponBehavior;

    public override void Initialize(Observable<DiamondKnowledgeState> state,
        Level level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(state, level, diamondOwnerBehavior);
        _ownerRangeAttackerBehavior = diamondOwnerBehavior.GetComponent<RangeAttackerBehavior>();

        foreach (var upgradeResource in _upgradeResourceBunches)
            upgradeResource.Amount.Decrease(50);
    }

    protected override void DoActivationWork()
    {
        _bowWeaponBehavior = Instantiate(_staticData.BowWeaponPrefab, _ownerRangeAttackerBehavior.WeaponLocation);
        _bowWeaponBehavior.Initialize(_level, IsTargetEnemyFunction);
    }

    protected override void DoDeactivationWork()
    {
        Destroy(_bowWeaponBehavior);
    }

    protected override string GetDescription()
    {
        var description = $"" +
            $"It creates a magical bow." +
            $"\n\nThe arrows can carry other diamond effects.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        var damageType = NoteUtils.AddColor(DamageType.PHYSICAL.ToString(), "red");
        var damageTypeShow = NoteUtils.ChangeSize($"Damage Type: {damageType}", NoteUtils.NumberSizeRatio);

        //------------------------------------------------

        var currentDamage = _bowWeaponBehavior.AttackDamage.Value.ToLong();
        var currentDamageShow = NoteUtils.AddColor(currentDamage, "black");
        currentDamageShow = NoteUtils.ChangeSize($"Damage: {currentDamageShow}", NoteUtils.NumberSizeRatio);

        var nextDamageShow = "";
        if (!_level.IsMax)
        {
            var nextDamage = _bowWeaponBehavior.AttackDamage.NextLevelValue.ToLong();
            nextDamageShow = NoteUtils.AddColor(nextDamage, NoteUtils.UpgradeColor);
            nextDamageShow = NoteUtils.ChangeSize($"({nextDamageShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentAttackSpeed = _bowWeaponBehavior.AttackSpeed.Value.Round();
        var currentFireRateShow = NoteUtils.AddColor(currentAttackSpeed + "fps", "black");
        currentFireRateShow = NoteUtils.ChangeSize($"Fire Rate: {currentFireRateShow}", NoteUtils.NumberSizeRatio);

        var nextFireRateShow = "";
        if (!_level.IsMax)
        {
            var nextAttackSpeed = _bowWeaponBehavior.AttackSpeed.NextLevelValue.Round();
            nextFireRateShow = NoteUtils.AddColor(nextAttackSpeed + "fps", NoteUtils.UpgradeColor);
            nextFireRateShow = NoteUtils.ChangeSize($"({nextFireRateShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentCriticalChance = _bowWeaponBehavior.CriticalChance.Value.ToLong();
        var currentCriticalChanceShow = NoteUtils.AddColor(currentCriticalChance + "%", "black");
        currentCriticalChanceShow = NoteUtils.ChangeSize($"Critical Chance: {currentCriticalChanceShow}", NoteUtils.NumberSizeRatio);

        var nextCriticalChanceShow = "";
        if (!_level.IsMax)
        {
            var nextCriticalChance = _bowWeaponBehavior.CriticalChance.NextLevelValue.ToLong();
            nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", NoteUtils.UpgradeColor);
            nextCriticalChanceShow = NoteUtils.ChangeSize($"({nextCriticalChanceShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var currentCriticalDamage = _bowWeaponBehavior.CriticalDamage.Value.ToLong();
        var currentCriticalDamageShow = NoteUtils.AddColor(currentCriticalDamage + "%", "black");
        currentCriticalDamageShow = NoteUtils.ChangeSize($"Critical Damage: {currentCriticalDamageShow}", NoteUtils.NumberSizeRatio);

        var nextCriticalDamageShow = "";
        if (!_level.IsMax)
        {
            var nextCriticalDamage = _bowWeaponBehavior.CriticalDamage.NextLevelValue.ToLong();
            nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", NoteUtils.UpgradeColor);
            nextCriticalDamageShow = NoteUtils.ChangeSize($"({nextCriticalDamageShow})", NoteUtils.NextNumberSizeRatio);
        }

        //------------------------------------------------

        var description = $"" +
            $"{damageTypeShow}\n" +
            $"{currentDamageShow}    {nextDamageShow}\n" +
            $"{currentFireRateShow}    {nextFireRateShow}\n" +
            $"{currentCriticalChanceShow}    {nextCriticalChanceShow}\n" +
            $"{currentCriticalDamageShow}    {nextCriticalDamageShow}\n";
        return description;
    }
}
