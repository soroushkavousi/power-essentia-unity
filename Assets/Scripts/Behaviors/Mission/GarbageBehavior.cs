using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
public class GarbageBehavior : MonoBehaviour, IObserver<CollideData>
{
    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private BodyBehavior _bodyBehavior = default;

    void Awake()
    {
        _bodyBehavior = GetComponent<BodyBehavior>();
        _bodyBehavior.FeedData();
        _bodyBehavior.Attach(this);
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
        projectileBehavior.GetComponent<BodyBehavior>().IsCollidingDisabled = true;
        Destroy(projectileBehavior.gameObject, 1);
    }

    public void OnNotify(ISubject<CollideData> subject, CollideData data)
    {
        if (ReferenceEquals(subject, _bodyBehavior))
        {
            switch (data.Type)
            {
                case CollideType.ENTER:
                    OnBodyEnter(data.TargetCollider2D);
                    break;
            }
        }
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
