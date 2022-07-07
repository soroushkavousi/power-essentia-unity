using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpellOwnerBehavior : MonoBehaviour
{
    [SerializeField] private List<SpellBehavior> _spellBehaviors = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public void FeedStaticData()
    {

    }


    private void Update()
    {

    }
}