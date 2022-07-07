using UnityEngine;

public class StatusOwnerBehavior : MonoBehaviour
{
    [SerializeField] private BurnStatusBehavior _burnStatusBehavior = default;
    [SerializeField] private StunStatusBehavior _stunStatusBehavior = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public BurnStatusBehavior BurnStatusBehavior => _burnStatusBehavior;
    public StunStatusBehavior StunStatusBehavior => _stunStatusBehavior;


    public void FeedData()
    {

    }
}
