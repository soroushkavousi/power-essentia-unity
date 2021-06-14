using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusOwnerBehavior : MonoBehaviour
{
    [SerializeField] private BurnStatusBehavior _burnStatusBehavior = default;
    [SerializeField] private StunStatusBehavior _stunStatusBehavior = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    public BurnStatusBehavior BurnStatusBehavior => _burnStatusBehavior;
    public StunStatusBehavior StunStatusBehavior => _stunStatusBehavior;


    public void FeedData()
    {
        
    }
}
