using UnityEngine;

public class KingDiamondBehavior : PermanentDiamondBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(KingDiamondBehavior) + Constants.HeaderEnd)]

    [SerializeField] private KingDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Number _goldBoost;

    public Number GoldBoost => _goldBoost;

    public override void Initialize(Observable<DiamondKnowledgeState> knowledgeState,
        Level level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(knowledgeState, level, diamondOwnerBehavior);

        _goldBoost = new Number(_level, _staticData.GoldBoost);
        foreach (var upgradeResource in _upgradeResourceBunches)
            upgradeResource.Amount.Decrease(80);
    }

    protected override void DoActivationWork()
    {
    }

    protected override void DoDeactivationWork()
    {

    }

    protected override string GetDescription()
    {
        var description = $"" +
            $"Owner of king diamonds can get much more money for killing monsters." +
            $" Gold can be used to upgrade other diamonds.\n" +
            $"King diamond is a base diamond that is active permanently.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        //------------------------------------------------

        var currentGoldBoost = _goldBoost.Value.ToLong();
        var nextGoldBoost = _goldBoost.NextLevelValue.ToLong();

        var currentGoldBosstShow = NoteUtils.AddColor(currentGoldBoost + "%", "black");
        currentGoldBosstShow = NoteUtils.ChangeSize($"Gold Boost: {currentGoldBosstShow}", NoteUtils.NumberSizeRatio);
        var nextGoldBoostShow = NoteUtils.AddColor(nextGoldBoost + "%", NoteUtils.UpgradeColor);
        nextGoldBoostShow = NoteUtils.ChangeSize($"({nextGoldBoostShow})", NoteUtils.NextNumberSizeRatio);

        //-----------------------------------------------

        var statsDescription = $"" +
            $"{currentGoldBosstShow}    {nextGoldBoostShow}\n";
        return statsDescription;
    }
}
