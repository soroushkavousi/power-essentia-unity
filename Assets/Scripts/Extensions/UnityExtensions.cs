using Assets.Scripts.Enums;
using System;
using UnityEngine;

public static class UnityExtensions
{
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
            if (targetBodyAreaBehavior.BodyBehavior.IsColliderDisabled)
                return true;
        }

        var targetVisionAreaBehavior = target.GetComponent<VisionAreaBehavior>();
        if (targetVisionAreaBehavior != null)
        {
            if (targetVisionAreaBehavior.VisionBehavior.IsColliderDisabled)
                return true;
        }
        return false;
    }
}

