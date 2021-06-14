using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ParticleMemberBehavior : MonoBehaviour
{
    [SerializeField] private ParticleGroupBehavior _particleGroupBehavior = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private Action<GameObject, GameObject> _onParticleCollisionAction = default;

    public ParticleGroupBehavior ParticleGroupBehavior => _particleGroupBehavior;

    public void FeedStaticData(Action<GameObject, GameObject> onParticleCollisionAction)
    {
        _onParticleCollisionAction = onParticleCollisionAction;
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log($"OnParticleCollision: other {other.name}");
        _onParticleCollisionAction(gameObject, other);
    }
}
