using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeaponData",
    menuName = "StaticData/Core/MeleeWeaponData", order = 1)]
public class MeleeWeaponStaticData : WeaponStaticData
{
    public AudioClip HitSound = default;
}
