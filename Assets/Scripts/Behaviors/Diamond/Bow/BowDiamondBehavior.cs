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
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentDamage = _bowWeaponBehavior.AttackDamage.Value.ToLong();
        var nextDamage = _bowWeaponBehavior.AttackDamage.NextLevelValue.ToLong();

        var currentDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentDamage, "black"));
        var nextDamageShow = NoteUtils.AddColor(nextDamage, upgradeColor);

        //------------------------------------------------

        var currentAttackSpeed = _bowWeaponBehavior.AttackSpeed.Value;
        var nextAttackSpeed = _bowWeaponBehavior.AttackSpeed.NextLevelValue;

        var currentFireRateShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentAttackSpeed + "fps", "black"));
        var nextFireRateShow = NoteUtils.AddColor(nextAttackSpeed + "fps", upgradeColor);

        //------------------------------------------------

        var currentCriticalChance = _bowWeaponBehavior.CriticalChance.Value;
        var nextCriticalChance = _bowWeaponBehavior.CriticalChance.NextLevelValue;

        var currentCriticalChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalChance + "%", "black"));
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", upgradeColor);

        //------------------------------------------------

        var currentCriticalDamage = _bowWeaponBehavior.CriticalDamage.Value;
        var nextCriticalDamage = _bowWeaponBehavior.CriticalDamage.NextLevelValue;

        var currentCriticalDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalDamage + "%", "black"));
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"Bow Diamond can be in activation mode permanently. It do a lot of things for the archer:\n\n" +
            $" + It creates a magical bow\n" +
            $" + It creates a magical arrow as the archer commands\n" +
            $" + Magical arrows can carry other diamond effects\n" +
            $" + Magical bow has the following stats:\n" +
            $"   + Damage: {currentDamageShow} (next: {nextDamageShow})\n" +
            $"   + Fire Rate: {currentFireRateShow} (next: {nextFireRateShow})\n" +
            $"   + Critical Chance: {currentCriticalChanceShow} (next: {nextCriticalChanceShow})\n" +
            $"   + Critical Damage: {currentCriticalDamageShow} (next: {nextCriticalDamageShow})";
        return description;
    }
}
