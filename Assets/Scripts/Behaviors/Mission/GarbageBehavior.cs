using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BodyBehavior))]
public class GarbageBehavior : MonoBehaviour
{
    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private BodyBehavior _bodyBehavior = default;

    void Awake()
    {
        _bodyBehavior = GetComponent<BodyBehavior>();
        _bodyBehavior.FeedData();
        _bodyBehavior.OnUnconstrainedEnterActions.Add(OnBodyEnter);
        //_bodyBehavior.OnExitActions.Add(OnBodyExit);
    }

    private void OnBodyEnter(Collider2D otherCollider)
    {
        var target = otherCollider.gameObject;

        var bodyAreaBehavior = target.GetComponent<BodyAreaBehavior>();
        if (bodyAreaBehavior == null)
            return;

        var owner = target.transform.parent.gameObject;
        var projectileBehavior = owner.GetComponent<ProjectileBehavior>();
        if (projectileBehavior == null)
            return;

        //projectileBehavior.Damage.Fix(0);
        projectileBehavior.GetComponent<BodyBehavior>().IsColliderDisabled = true;
        Destroy(projectileBehavior.gameObject, 1);
    }

    //private void OnBodyExit(Collider2D otherCollider)
    //{
    //    var target = otherCollider.gameObject;

    //    var bodyAreaBehavior = target.GetComponent<BodyAreaBehavior>();
    //    if (bodyAreaBehavior == null)
    //        return;

    //    var owner = target.transform.parent.gameObject;
    //    var projectileBehavior = owner.GetComponent<ProjectileBehavior>();
    //    if (projectileBehavior == null)
    //        return;

    //    Destroy(owner);
    //}
}
