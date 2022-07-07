using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ParticleMemberBehavior : MonoBehaviour
{
    [SerializeField] private ParticleGroupBehavior _particleGroupBehavior = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private Action<GameObject, GameObject> _onParticleCollisionAction = default;

    public ParticleGroupBehavior ParticleGroupBehavior => _particleGroupBehavior;

    public void FeedData(Action<GameObject, GameObject> onParticleCollisionAction)
    {
        _onParticleCollisionAction = onParticleCollisionAction;
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log($"OnParticleCollision: other {other.name}");
        _onParticleCollisionAction?.Invoke(gameObject, other);
    }
}
