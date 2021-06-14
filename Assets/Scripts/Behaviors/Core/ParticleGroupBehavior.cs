using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleGroupBehavior : MonoBehaviour
{
    [SerializeField] private List<ParticleMemberBehavior> _particleMemberBehaviors = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private bool _isColliderDisabled = default;

    private ParticleSystem _particleSystem = default;

    public bool IsColliderDisabled { get => _isColliderDisabled; set => _isColliderDisabled = value; }
    public ParticleSystem ParticleSystem => _particleSystem;
    public OrderedList<Action<GameObject, GameObject>> OnParticleCollisionActions { get; } = new OrderedList<Action<GameObject, GameObject>>();

    public void FeedData()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        _particleMemberBehaviors.ForEach(particleMemberBehavior 
            => particleMemberBehavior.FeedStaticData(HandleParticleCollision));
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
