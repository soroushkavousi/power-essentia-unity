using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodDiamondStaticData",
    menuName = "StaticData/Diamonds/Tools/BloodDiamondStaticData", order = 1)]
public class BloodDiamondStaticData : ScriptableObject
{
    [SerializeField] private DiamondStaticData _diamondStaticData = default;
    [SerializeField] private float _startBloodPerDemonLevel = default;
    [SerializeField] private float _bloodPerDemonLevelBasePerLevel = default;

    public DiamondStaticData DiamondStaticData => _diamondStaticData;
    public float StartBloodPerDemonLevel => _startBloodPerDemonLevel;
    public float BloodPerDemonLevelBasePerLevel => _bloodPerDemonLevelBasePerLevel;
}
