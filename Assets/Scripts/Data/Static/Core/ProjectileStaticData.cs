using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData",
    menuName = "StaticData/Core/ProjectileData", order = 1)]
public class ProjectileStaticData : ScriptableObject
{
    public MovementStaticData MovementStaticData = default;
    public AudioClip HitSound = default;
}
