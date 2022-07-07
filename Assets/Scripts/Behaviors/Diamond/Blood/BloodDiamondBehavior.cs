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

    private void Awake()
    {
        base.FeedData(_staticData);
    }

    public override void Initialize(Observable<DiamondKnowledgeState> knowledgeState,
        Observable<int> level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
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
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentBloodPerDemonLevel = _bloodRatio.Value;
        var nextBloodPerDemonLevel = _bloodRatio.NextLevelValue;

        var currentBloodPerDemonLevelShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentBloodPerDemonLevel + " per demon level", "black"));
        var nextBloodPerDemonLevelShow = NoteUtils.AddColor(nextBloodPerDemonLevel + " per demon level", upgradeColor);

        //-----------------------------------------------

        var description = $"" +
            $"Blood Diamond can be in activation mode permanently.\n\n" +
            $" + It can pulls demon bloods inside of itself and store them.\n" +
            $" + Stored blood will be share with other diamonds to activate them.\n" +
            $" + Stats:\n" +
            $"   + Blood: {currentBloodPerDemonLevelShow} (next: {nextBloodPerDemonLevelShow})";
        return description;
    }
}
