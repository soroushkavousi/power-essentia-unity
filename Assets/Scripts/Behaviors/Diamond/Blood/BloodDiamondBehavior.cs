using UnityEngine;

public class BloodDiamondBehavior : DiamondBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(BloodDiamondBehavior) + Constants.HeaderEnd)]

    [SerializeField] private BloodDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Number _bloodRatio;

    public Number BloodRatio => _bloodRatio;

    public override void Initialize(Observable<DiamondKnowledgeState> knowledgeState,
        Observable<int> level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(knowledgeState, level, diamondOwnerBehavior);

        _bloodRatio = new Number(_staticData.BloodRatio, level, _staticData.BloodRatioLevelPercentage);
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

        var currentBloodPerDemonLevel = _bloodRatio.Value;
        var nextBloodPerDemonLevel = _bloodRatio.NextLevelValue;

        var currentBloodPerDemonLevelShow = NoteUtils.AddColor(currentBloodPerDemonLevel + "%", "black");
        currentBloodPerDemonLevelShow = NoteUtils.ChangeSize($"Blood Taken Ratio: {currentBloodPerDemonLevelShow}", NoteUtils.NumberSizeRatio);
        var nextBloodPerDemonLevelShow = NoteUtils.AddColor(nextBloodPerDemonLevel + "%", NoteUtils.UpgradeColor);
        nextBloodPerDemonLevelShow = NoteUtils.ChangeSize($"({nextBloodPerDemonLevelShow})", NoteUtils.NextNumberSizeRatio);

        //-----------------------------------------------

        var statsDescription = $"" +
            $"{currentBloodPerDemonLevelShow}    {nextBloodPerDemonLevelShow}\n";
        return statsDescription;
    }
}
