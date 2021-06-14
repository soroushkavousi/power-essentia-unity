using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BodyAreaBehavior : MonoBehaviour
{
    [SerializeField] private BodyBehavior _bodyBehavior = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private Action<Collider2D> _onEnterAction = default;
    private Action<Collider2D> _onExitAction = default;

    public BodyBehavior BodyBehavior => _bodyBehavior;

    public void FeedStaticData(Action<Collider2D> onEnterAction, 
        Action<Collider2D> onExitAction)
    {
        _onEnterAction = onEnterAction;
        _onExitAction = onExitAction;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(_onEnterAction == default)
            Debug.Log($"_onEnterAction == default | {name} -> {transform.parent.name} -> {transform.parent.parent.name}");
        _onEnterAction(otherCollider);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        _onExitAction(otherCollider);
    }
}
