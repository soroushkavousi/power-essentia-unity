using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGroupBehavior : MonoBehaviour
{
    [SerializeField] private List<ParticleMemberBehavior> _particleMemberBehaviors = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private bool _isColliderDisabled = default;

    private ParticleSystem _particleSystem = default;

    public bool IsColliderDisabled { get => _isColliderDisabled; set => _isColliderDisabled = value; }
    public ParticleSystem ParticleSystem => _particleSystem;
    public OrderedList<Action<GameObject, GameObject>> OnParticleCollisionActions { get; } = new OrderedList<Action<GameObject, GameObject>>();

    public void FeedData()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        _particleMemberBehaviors.ForEach(particleMemberBehavior
            => particleMemberBehavior.FeedData(HandleParticleCollision));
    }

    private void HandleParticleCollision(GameObject sender, GameObject other)
    {
        Debug.Log($"HandleParticleCollision: other {other.name}");

        if (_isColliderDisabled)
            return;

        OnParticleCollisionActions.CallActionsSafely(sender, other);
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Teeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeest");
    }
}
