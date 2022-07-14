using UnityEngine;

public class BloodDiamondBehavior : PermanentDiamondBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(BloodDiamondBehavior) + Constants.HeaderEnd)]

    [SerializeField] private BloodDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Number _bloodRatio;

    public Number BloodRatio => _bloodRatio;

    public override void Initialize(Observable<DiamondKnowledgeState> knowledgeState,
        Level level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(knowledgeState, level, diamondOwnerBehavior);

        _bloodRatio = new Number(_level, _staticData.BloodRatioLevelInfo);
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
            $"It will take the blood of dead demons in the battlefield." +
            $" Stored blood can be used to upgrade other diamonds.\n" +
            $"Blood diamond is a base diamond that is active permanently.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        //------------------------------------------------

        var currentBloodRatio = _bloodRatio.Value.ToLong();
        var nextBloodPerDemonLevel = _bloodRatio.NextLevelValue.ToLong();

        var currentBloodRatioShow = NoteUtils.AddColor(currentBloodRatio + "%", "black");
        currentBloodRatioShow = NoteUtils.ChangeSize($"Blood Taken Ratio: {currentBloodRatioShow}", NoteUtils.NumberSizeRatio);
        var nextBloodRatioShow = NoteUtils.AddColor(nextBloodPerDemonLevel + "%", NoteUtils.UpgradeColor);
        nextBloodRatioShow = NoteUtils.ChangeSize($"({nextBloodRatioShow})", NoteUtils.NextNumberSizeRatio);

        //-----------------------------------------------

        var statsDescription = $"" +
            $"{currentBloodRatioShow}    {nextBloodRatioShow}\n";
        return statsDescription;
    }
}
