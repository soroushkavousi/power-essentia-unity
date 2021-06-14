using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondMenuToolsBehavior : MonoBehaviour
{
    private static DiamondMenuToolsBehavior _instance = default;
    [SerializeField] private DiamondUpgradeMenuBehavior _diamondUpgradeMenuBehavior = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    public static DiamondMenuToolsBehavior Instance => Utils.GetInstance(ref _instance);
    public DiamondUpgradeMenuBehavior DiamondUpgradeMenuBehavior => _diamondUpgradeMenuBehavior;

    private void Awake()
    {
        _diamondUpgradeMenuBehavior.FeedData(PlayerBehavior.Main.DynamicData.SelectedItems.
                MenuDiamondName[RingName.TOOLS]);
    }
}
