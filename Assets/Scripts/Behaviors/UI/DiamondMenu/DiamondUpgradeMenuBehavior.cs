using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondUpgradeMenuBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _diamondTitle = default;
    [SerializeField] private TextMeshProUGUI _diamondDescription = default;
    [SerializeField] private TextMeshProUGUI _diamondStatsDescription = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeCoinDisplay = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeDemonBloodDisplay = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeDarkDemonBloodDisplay = default;
    [SerializeField] private ButtonBehavior _upgradeButtonBehavior = default;
    [SerializeField] private ButtonBehavior _buyButtonBehavior = default;
    [SerializeField] private Image _masteredImage = default;
    [SerializeField] private List<DiamondUpgradeMenuItemBehavior> _items = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Observable<DiamondName> _selectedDiamondName = default;
    [SerializeField] private DiamondBehavior _selectedDiamondBehavior = default;

    public Observable<DiamondName> SelectedDiamondName => _selectedDiamondName;
    public DiamondBehavior SelectedDiamondBehavior => _selectedDiamondBehavior;
    public OrderedList<Action> OnShowSelectedDiamondDetailsActions { get; } = new OrderedList<Action>();

    public void FeedData(Observable<DiamondName> selectedDiamondName)
    {
        _selectedDiamondName = selectedDiamondName;
        _selectedDiamondName.Attach(this);
        ChangeSelectedDiamond();
        _items.ForEach(item => item.FeedData());
    }

    private void ChangeSelectedDiamond()
    {
        if (_selectedDiamondName.Value == DiamondName.NONE)
            return;

        _selectedDiamondBehavior = DiamondOwnerBehavior.MainDiamondOwner
            .AllDiamondBehaviors[_selectedDiamondName.Value];
        ShowSelectedDiamondDetails();
    }

    public void BuyDiamond()
    {
        ConsumeResourceBunches(_selectedDiamondBehavior.BuyResourceBunches);
        _selectedDiamondBehavior.KnowledgeState.Value = DiamondKnowledgeState.OWNED;
        ShowSelectedDiamondDetails();
        SetNewDiamondInDeck();
    }

    private void SetNewDiamondInDeck()
    {
        var deck = PlayerBehavior.MainPlayer.DynamicData.SelectedItems.RingDiamondNamesMap;
        foreach (var ringName in new List<RingName> { RingName.RIGHT, RingName.LEFT })
        {
            for (int i = 0; i < 4; i++)
            {
                if (deck[ringName][i].Value == DiamondName.NONE)
                {
                    deck[ringName][i].Value = _selectedDiamondBehavior.Name;
                    return;
                }
            }
        }
    }

    public void UpgradeDiamond()
    {
        var resouceBunches = _selectedDiamondBehavior.UpgradeResourceBunches.Select(urb => urb.ToResourceBunch()).ToList();
        ConsumeResourceBunches(resouceBunches);
        _selectedDiamondBehavior.Level.Value += 1;
        if (_selectedDiamondBehavior.Level.Value == GameManagerBehavior.Instance.Settings.DiamondMaxLevel)
            _selectedDiamondBehavior.KnowledgeState.Value = DiamondKnowledgeState.MASTERED;
        ShowSelectedDiamondDetails();
    }

    private void ConsumeResourceBunches(List<ResourceBunch> resourceBunches)
    {
        resourceBunches.ForEach(rb =>
            PlayerBehavior.MainPlayer.DynamicData.ResourceBunches.Find(prb => prb.Type == rb.Type).Amount.Value -= rb.Amount.Value
        );
    }

    private void ShowSelectedDiamondDetails()
    {
        var level = _selectedDiamondBehavior.Level.Value;
        var title = $"{_selectedDiamondBehavior.ShowName} Diamond (Level {level})";
        _diamondTitle.text = title;
        _upgradeButtonBehavior.Text.text = $"Upgrade to level {level + 1}";
        _diamondDescription.text = _selectedDiamondBehavior.Description;
        _diamondStatsDescription.text = _selectedDiamondBehavior.StatsDescription;
        UpdateUpgradeDetails();
        _masteredImage.gameObject.SetActive(_selectedDiamondBehavior.KnowledgeState.Value == DiamondKnowledgeState.MASTERED);
        _upgradeButtonBehavior.Owner.SetActive(_selectedDiamondBehavior.KnowledgeState.Value == DiamondKnowledgeState.OWNED);
        _buyButtonBehavior.Owner.SetActive(_selectedDiamondBehavior.KnowledgeState.Value == DiamondKnowledgeState.DISCOVERED);
        OnShowSelectedDiamondDetailsActions.CallActionsSafely();
    }

    private void UpdateUpgradeDetails()
    {
        if (_selectedDiamondBehavior == null)
            return;
        List<ResourceBunch> resourceBunches;
        switch (_selectedDiamondBehavior.KnowledgeState.Value)
        {
            case DiamondKnowledgeState.DISCOVERED:
                resourceBunches = _selectedDiamondBehavior.BuyResourceBunches;
                break;
            case DiamondKnowledgeState.OWNED:
                resourceBunches = _selectedDiamondBehavior.UpgradeResourceBunches.Select(urb => urb.ToResourceBunch()).ToList();
                break;
            case DiamondKnowledgeState.MASTERED:
                _upgradeCoinDisplay.gameObject.SetActive(false);
                _upgradeDemonBloodDisplay.gameObject.SetActive(false);
                _upgradeDarkDemonBloodDisplay.gameObject.SetActive(false);
                return;
            default:
                return;
        }

        var upgradeCoinAmount = resourceBunches.Find(rb => rb.Type == ResourceType.COIN)?.Amount?.Value ?? 0;
        _upgradeCoinDisplay.AmountText.text = upgradeCoinAmount.ToString();
        _upgradeCoinDisplay.gameObject.SetActive(upgradeCoinAmount != 0);

        var upgradeDemonBloodAmount = resourceBunches.Find(rb => rb.Type == ResourceType.DEMON_BLOOD)?.Amount?.Value ?? 0;
        _upgradeDemonBloodDisplay.AmountText.text = upgradeDemonBloodAmount.ToString();
        _upgradeDemonBloodDisplay.gameObject.SetActive(upgradeDemonBloodAmount != 0);

        var upgradeDarkDemonBloodAmount = resourceBunches.Find(rb => rb.Type == ResourceType.DARK_DEMON_BLOOD)?.Amount?.Value ?? 0;
        _upgradeDarkDemonBloodDisplay.AmountText.text = upgradeDarkDemonBloodAmount.ToString();
        _upgradeDarkDemonBloodDisplay.gameObject.SetActive(upgradeDarkDemonBloodAmount != 0);

        var playerResourceBunches = PlayerBehavior.MainPlayer.DynamicData.ResourceBunches;
        var playerCoinAmount = playerResourceBunches.Find(prb => prb.Type == ResourceType.COIN).Amount.Value;
        var playerDemonBloodAmount = playerResourceBunches.Find(prb => prb.Type == ResourceType.DEMON_BLOOD).Amount.Value;
        var playerDarkDemonBloodAmount = playerResourceBunches.Find(prb => prb.Type == ResourceType.DARK_DEMON_BLOOD).Amount.Value;

        if (upgradeCoinAmount > playerCoinAmount
            || upgradeDemonBloodAmount > playerDemonBloodAmount
            || upgradeDarkDemonBloodAmount > playerDarkDemonBloodAmount)
        {
            _upgradeButtonBehavior.Lock();
            _buyButtonBehavior.Lock();
        }
        else
        {
            _upgradeButtonBehavior.Unlock();
            _buyButtonBehavior.Unlock();
        }
    }

    private void OnEnable()
    {
        UpdateUpgradeDetails();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _selectedDiamondName)
        {
            ChangeSelectedDiamond();
        }
    }
}
