using UnityEngine;

[CreateAssetMenu(fileName = "MeleeDemonStaticData",
    menuName = "StaticData/Demons/MeleeDemonStaticData", order = 1)]
public class MeleeDemonStaticData : DemonStaticData
{
    public MeleeAttackerStaticData MeleeAttackerStaticData = default;
    public AIAttackerStaticData AIAttackerStaticData = default;
    public MovementStaticData MovementStaticData = default;
}