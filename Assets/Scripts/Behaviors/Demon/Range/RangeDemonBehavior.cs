using UnityEngine;

[RequireComponent(typeof(RangeAttackerBehavior))]
[RequireComponent(typeof(AIAttackerBehavior))]
public abstract class RangeDemonBehavior : DemonBehavior
{
    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]
    private RangeDemonStaticData _staticData = default;



    public override void Initialize(int level)
    {
        base.Initialize(level);
    }

    public void FeedData(RangeDemonStaticData staticData)
    {
        _staticData = staticData;
        base.FeedData(_staticData);

    }
}
