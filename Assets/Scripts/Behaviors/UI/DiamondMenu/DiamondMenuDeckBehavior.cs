using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondMenuDeckBehavior : MonoBehaviour
{
    private static DiamondMenuDeckBehavior _instance = default;
    [SerializeField] private DiamondUpgradeMenuBehavior _diamondUpgradeMenuBehavior = default;
    [SerializeField] private ButtonBehavior _useInDeckButton = default;
    [SerializeField] private List<DiamondMenuDeckItemBehavior> _deckItemBehaviors = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    public static DiamondMenuDeckBehavior Instance => Utils.GetInstance(ref _instance);
    public DiamondUpgradeMenuBehavior DiamondUpgradeMenuBehavior => _diamondUpgradeMenuBehavior;

    private void Awake()
    {
        _diamondUpgradeMenuBehavior.FeedData(PlayerBehavior.Main.DynamicData.SelectedItems.
                MenuDiamondName[RingName.DECK]);
        _diamondUpgradeMenuBehavior.OnShowSelectedDiamondDetailsActions.Add(HandleShowSelectedDiamondDetailsAction);
        HandleShowSelectedDiamondDetailsAction();
    }

    private void HandleShowSelectedDiamondDetailsAction()
    {
        if (_diamondUpgradeMenuBehavior.SelectedDiamondBehavior.IsOwned.Value)
            _useInDeckButton.Unlock();
        else
            _useInDeckButton.Lock();
    }

    public void StartUseInDeckOperation()
    {
        _deckItemBehaviors.ForEach(dib => dib.Glow());
    }

    public void FinishUseInDeckOperation()
    {
        _deckItemBehaviors.ForEach(dib => dib.UnGlow());
    }
}
