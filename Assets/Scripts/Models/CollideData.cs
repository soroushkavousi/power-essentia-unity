using UnityEngine;

public class CollideData
{
    public CollideType Type { get; }
    public Collider2D TargetCollider2D { get; }
    public bool IsCollidingDisabled { get; }

    public CollideData(CollideType type, Collider2D targetCollider2D, bool isCollidingDisabled)
    {
        Type = type;
        TargetCollider2D = targetCollider2D;
        IsCollidingDisabled = isCollidingDisabled;
    }
}

public enum CollideType
{
    ENTER,
    EXIT
}
