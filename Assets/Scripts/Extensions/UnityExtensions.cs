using UnityEngine;

public static class UnityExtensions
{
    public static Vector2 Randomize(this Vector2 value, float percentage = 20f)
    {
        var randomizeX = value.x.Randomize(percentage);
        var randomizeY = value.y.Randomize(percentage);
        return new Vector2(randomizeX, randomizeY);
    }

    public static Vector3 Randomize(this Vector3 value, float percentage = 20f)
    {
        var randomizeX = value.x.Randomize(percentage);
        var randomizeY = value.y.Randomize(percentage);
        var randomizeZ = value.z.Randomize(percentage);
        return new Vector3(randomizeX, randomizeY, randomizeZ);
    }

    public static T GetSubComponent<T>(this ScriptableObject data)
    {
        var iComponent = (IComponent)data;
        return iComponent.GetSubComponent<T>();
    }

    public static bool IsTargetColliderDisabled(this Collider2D targetCollider)
    {
        var target = targetCollider.gameObject;

        var targetBodyAreaBehavior = target.GetComponent<BodyAreaBehavior>();
        if (targetBodyAreaBehavior != null)
        {
            if (targetBodyAreaBehavior.BodyBehavior.IsCollidingDisabled)
                return true;
        }

        var targetVisionAreaBehavior = target.GetComponent<VisionAreaBehavior>();
        if (targetVisionAreaBehavior != null)
        {
            if (targetVisionAreaBehavior.VisionBehavior.IsCollidingDisabled)
                return true;
        }
        return false;
    }
}

