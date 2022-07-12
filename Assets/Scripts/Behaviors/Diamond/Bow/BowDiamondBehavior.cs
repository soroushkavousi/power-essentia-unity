using UnityEngine;

public class BowDiamondBehavior : DiamondBehavior
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
        Observable<int> level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(state, level, diamondOwnerBehavior);
        _ownerRangeAttackerBehavior = diamondOwnerBehavior.GetComponent<RangeAttackerBehavior>();
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
            $" The arrows of magical bos can carry other diamond effects.\n" +
            $"Bow diamond is a base diamond that is active permanently.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        //------------------------------------------------

        var currentDamage = _bowWeaponBehavior.AttackDamage.Value.ToLong();
        var nextDamage = _bowWeaponBehavior.AttackDamage.NextLevelValue.ToLong();

        var currentDamageShow = NoteUtils.AddColor(currentDamage, "black");
        currentDamageShow = NoteUtils.ChangeSize($"Damage: {currentDamageShow}", NoteUtils.NumberSizeRatio);
        var nextDamageShow = NoteUtils.AddColor(nextDamage, NoteUtils.UpgradeColor);
        nextDamageShow = NoteUtils.ChangeSize($"({nextDamageShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentAttackSpeed = _bowWeaponBehavior.AttackSpeed.Value.Round();
        var nextAttackSpeed = _bowWeaponBehavior.AttackSpeed.NextLevelValue.Round();

        var currentFireRateShow = NoteUtils.AddColor(currentAttackSpeed + "fps", "black");
        currentFireRateShow = NoteUtils.ChangeSize($"Fire Rate: {currentFireRateShow}", NoteUtils.NumberSizeRatio);
        var nextFireRateShow = NoteUtils.AddColor(nextAttackSpeed + "fps", NoteUtils.UpgradeColor);
        nextFireRateShow = NoteUtils.ChangeSize($"({nextFireRateShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCriticalChance = _bowWeaponBehavior.CriticalChance.Value.ToLong();
        var nextCriticalChance = _bowWeaponBehavior.CriticalChance.NextLevelValue.ToLong();

        var currentCriticalChanceShow = NoteUtils.AddColor(currentCriticalChance + "%", "black");
        currentCriticalChanceShow = NoteUtils.ChangeSize($"Critical Chance: {currentCriticalChanceShow}", NoteUtils.NumberSizeRatio);
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", NoteUtils.UpgradeColor);
        nextCriticalChanceShow = NoteUtils.ChangeSize($"({nextCriticalChanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentCriticalDamage = _bowWeaponBehavior.CriticalDamage.Value.ToLong();
        var nextCriticalDamage = _bowWeaponBehavior.CriticalDamage.NextLevelValue.ToLong();

        var currentCriticalDamageShow = NoteUtils.AddColor(currentCriticalDamage + "%", "black");
        currentCriticalDamageShow = NoteUtils.ChangeSize($"Critical Damage: {currentCriticalDamageShow}", NoteUtils.NumberSizeRatio);
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", NoteUtils.UpgradeColor);
        nextCriticalDamageShow = NoteUtils.ChangeSize($"({nextCriticalDamageShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var description = $"" +
            $"{currentDamageShow}    {nextDamageShow}\n" +
            $"{currentFireRateShow}    {nextFireRateShow}\n" +
            $"{currentCriticalChanceShow}    {nextCriticalChanceShow}\n" +
            $"{currentCriticalDamageShow}    {nextCriticalDamageShow}\n";
        return description;
    }
}
