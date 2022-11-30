using Assets.Scripts.Enums;

public abstract class PermanentDiamondBehavior : DiamondBehavior
{
    private PermanentDiamondStaticData _permanentDiamondStaticData = default;

    public override void Activate()
    {
        _state.Value = DiamondState.USING;
        DoActivationWork();
    }

    public override void Deactivate()
    {
        _state.Value = DiamondState.DEACTIVED;
        DoDeactivationWork();
    }

    public void FeedData(PermanentDiamondStaticData permanentDiamondStaticData)
    {
        _permanentDiamondStaticData = permanentDiamondStaticData;
        FeedData(permanentDiamondStaticData, DiamondType.PERMANENT);
    }

    public override void Initialize(Observable<DiamondKnowledgeState> knowledgeState,
        Level level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.Initialize(knowledgeState, level, diamondOwnerBehavior);
    }
}
