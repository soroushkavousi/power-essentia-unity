using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

public class DiamondDeckRingMenuBehavior : MonoBehaviour
{
    private static DiamondDeckRingMenuBehavior _instance = default;
    [SerializeField] private DiamondUpgradeMenuBehavior _diamondUpgradeMenuBehavior = default;
    [SerializeField] private ButtonBehavior _useInDeckButton = default;
    [SerializeField] private List<DiamondMenuDeckItemBehavior> _deckItemBehaviors = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public static DiamondDeckRingMenuBehavior Instance => Utils.GetInstance(ref _instance);
    public DiamondUpgradeMenuBehavior DiamondUpgradeMenuBehavior => _diamondUpgradeMenuBehavior;

    private void Awake()
    {
        _diamondUpgradeMenuBehavior.FeedData(PlayerBehavior.MainPlayer.DynamicData.SelectedItems.
                MenuDiamondName[RingName.DECK]);
        _diamondUpgradeMenuBehavior.OnShowSelectedDiamondDetailsActions.Add(HandleShowSelectedDiamondDetailsAction);
        HandleShowSelectedDiamondDetailsAction();
    }

    private void HandleShowSelectedDiamondDetailsAction()
    {
        switch (_diamondUpgradeMenuBehavior.SelectedDiamondBehavior.KnowledgeState.Value)
        {
            case DiamondKnowledgeState.OWNED:
            case DiamondKnowledgeState.MASTERED:
                _useInDeckButton.Unlock();
                break;
            default:
                _useInDeckButton.Lock();
                break;
        }
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
