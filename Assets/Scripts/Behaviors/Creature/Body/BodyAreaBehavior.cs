using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BodyAreaBehavior : MonoBehaviour
{
    [SerializeField] private BodyBehavior _bodyBehavior = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private Action<Collider2D> _onEnterAction = default;
    private Action<Collider2D> _onExitAction = default;

    public BodyBehavior BodyBehavior => _bodyBehavior;

    public void FeedData(Action<Collider2D> onEnterAction,
        Action<Collider2D> onExitAction)
    {
        _onEnterAction = onEnterAction;
        _onExitAction = onExitAction;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (_onEnterAction == default)
            Debug.Log($"_onEnterAction == default | {name} -> {transform.parent.name} -> {transform.parent.parent.name}");
        _onEnterAction?.Invoke(otherCollider);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (_onExitAction == default)
            Debug.Log($"_onExitAction == default | {name} -> {transform.parent.name} -> {transform.parent.parent.name}");
        _onExitAction?.Invoke(otherCollider);
    }
}
