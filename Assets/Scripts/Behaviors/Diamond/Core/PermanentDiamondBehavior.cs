using Assets.Scripts.Enums;

public abstract class PermanentDiamondBehavior : DiamondBehavior
{
    private PermanentDiamondStaticData _permanentDiamondStaticData = default;

    public override void Activate()
    {
        _isReady = false;
        DoActivationWork();
        _onUsing = true;
    }

    public override void Deactivate()
    {
        _onUsing = false;
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
