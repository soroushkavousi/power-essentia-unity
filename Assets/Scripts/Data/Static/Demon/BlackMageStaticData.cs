using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackMageStaticData",
    menuName = "StaticData/Demons/BlackMageStaticData", order = 1)]
public class BlackMageStaticData : ScriptableObject
{
    [SerializeField] private DemonStaticData _demonStaticData = default;
    [SerializeField] private MovementStaticData _movementStaticData = default;
    [SerializeField] private RangeAttackerStaticData _rangeAttackerStaticData = default;
    [SerializeField] private AIAttackerStaticData _aiAttackerStaticData = default;

    public DemonStaticData DemonStaticData => _demonStaticData;
    public MovementStaticData MovementStaticData => _movementStaticData;
    public RangeAttackerStaticData RangeAttackerStaticData => _rangeAttackerStaticData;
    public AIAttackerStaticData AIAttackerStaticData => _aiAttackerStaticData;
}