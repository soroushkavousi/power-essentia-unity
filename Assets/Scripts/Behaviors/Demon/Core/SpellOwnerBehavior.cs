using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpellOwnerBehavior : MonoBehaviour
{
    [SerializeField] private List<SpellBehavior> _spellBehaviors = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    public void FeedStaticData()
    {

    }


    private void Update()
    {

    }
}