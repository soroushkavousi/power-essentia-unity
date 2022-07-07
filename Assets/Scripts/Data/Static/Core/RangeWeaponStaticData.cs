using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeaponData",
    menuName = "StaticData/Core/RangeWeaponData", order = 1)]
public class RangeWeaponStaticData : WeaponStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Range Weapon Data")]
    public AudioClip FireSound = default;
    public ProjectileBehavior ProjectileBehavior = default;
}
