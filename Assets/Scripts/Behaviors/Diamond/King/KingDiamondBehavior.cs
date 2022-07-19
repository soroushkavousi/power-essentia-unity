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
            $"The owner of king diamond can get more money for killing monsters." +
            $"\n\nGold can be used to upgrade other diamonds.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        //------------------------------------------------

        var currentGoldBoost = _goldBoost.Value.ToLong();
        var currentGoldBosstShow = NoteUtils.AddColor(currentGoldBoost + "%", "black");
        currentGoldBosstShow = NoteUtils.ChangeSize($"Gold Boost: {currentGoldBosstShow}", NoteUtils.NumberSizeRatio);

        var nextGoldBoostShow = "";
        if (!_level.IsMax)
        {
            var nextGoldBoost = _goldBoost.NextLevelValue.ToLong();
            nextGoldBoostShow = NoteUtils.AddColor(nextGoldBoost + "%", NoteUtils.UpgradeColor);
            nextGoldBoostShow = NoteUtils.ChangeSize($"({nextGoldBoostShow})", NoteUtils.NextNumberSizeRatio);
        }

        //-----------------------------------------------

        var statsDescription = $"" +
            $"{currentGoldBosstShow}    {nextGoldBoostShow}\n";
        return statsDescription;
    }
}
