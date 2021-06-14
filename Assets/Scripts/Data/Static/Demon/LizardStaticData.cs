using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LizardStaticData",
    menuName = "StaticData/Demons/LizardStaticData", order = 1)]
public class LizardStaticData : ScriptableObject
{
    [SerializeField] private DemonStaticData _demonStaticData = default;
    [SerializeField] private MovementStaticData _movementStaticData = default;
    [SerializeField] private MeleeAttackerStaticData _meleeAttackerStaticData = default;
    [SerializeField] private AIAttackerStaticData _aiAttackerStaticData = default;

    public DemonStaticData DemonStaticData => _demonStaticData;
    public MovementStaticData MovementStaticData => _movementStaticData;
    public MeleeAttackerStaticData MeleeAttackerStaticData => _meleeAttackerStaticData;
    public AIAttackerStaticData AIAttackerStaticData => _aiAttackerStaticData;
}