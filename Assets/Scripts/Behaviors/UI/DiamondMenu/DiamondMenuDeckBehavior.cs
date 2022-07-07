using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

public class DiamondMenuDeckBehavior : MonoBehaviour
{
    private static DiamondMenuDeckBehavior _instance = default;
    [SerializeField] private DiamondUpgradeMenuBehavior _diamondUpgradeMenuBehavior = default;
    [SerializeField] private ButtonBehavior _useInDeckButton = default;
    [SerializeField] private List<DiamondMenuDeckItemBehavior> _deckItemBehaviors = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

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
        if (_diamondUpgradeMenuBehavior.SelectedDiamondBehavior.KnowledgeState.Value == DiamondKnowledgeState.OWNED)
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
