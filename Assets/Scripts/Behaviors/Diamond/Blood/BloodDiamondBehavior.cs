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
            $"It will take the blood of dead demons in the battlefield." +
            $"\n\nStored blood can be used to upgrade other diamonds.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        //------------------------------------------------

        var currentBloodRatio = _bloodRatio.Value.ToLong();
        var nextBloodRatio = _bloodRatio.NextLevelValue.ToLong();

        var currentBloodRatioShow = NoteUtils.AddColor(currentBloodRatio + "%", "black");
        currentBloodRatioShow = NoteUtils.ChangeSize($"Blood Taken Ratio: {currentBloodRatioShow}", NoteUtils.NumberSizeRatio);
        var nextBloodRatioShow = NoteUtils.AddColor(nextBloodRatio + "%", NoteUtils.UpgradeColor);
        nextBloodRatioShow = NoteUtils.ChangeSize($"({nextBloodRatioShow})", NoteUtils.NextNumberSizeRatio);

        //-----------------------------------------------

        var statsDescription = $"" +
            $"{currentBloodRatioShow}    {nextBloodRatioShow}\n";
        return statsDescription;
    }
}
