using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiamondUpgradeMenuBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _diamondTitle = default;
    [SerializeField] private TextMeshProUGUI _diamondDetails = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeCoinDisplay = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeDemonBloodDisplay = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeDarkDemonBloodDisplay = default;
    [SerializeField] private ButtonBehavior _upgradeButtonBehavior = default;
    [SerializeField] private ButtonBehavior _buyButtonBehavior = default;
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
    }

    public void UpgradeDiamond()
    {
        var resouceBunches = _selectedDiamondBehavior.UpgradeResourceBunches.Select(urb => urb.ToResourceBunch()).ToList();
        ConsumeResourceBunches(resouceBunches);
        _selectedDiamondBehavior.Level.Value += 1;
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

        _diamondDetails.text = _selectedDiamondBehavior.Description;
        UpdateUpgradeDetails();
        _upgradeButtonBehavior.Owner.SetActive(_selectedDiamondBehavior.KnowledgeState.Value == DiamondKnowledgeState.OWNED);
        _buyButtonBehavior.Owner.SetActive(_selectedDiamondBehavior.KnowledgeState.Value == DiamondKnowledgeState.DISCOVERED);
        OnShowSelectedDiamondDetailsActions.CallActionsSafely();
    }

    private void UpdateUpgradeDetails()
    {
        List<ResourceBunch> resourceBunches;
        if (_selectedDiamondBehavior.KnowledgeState.Value == DiamondKnowledgeState.DISCOVERED)
            resourceBunches = _selectedDiamondBehavior.BuyResourceBunches;
        else
            resourceBunches = _selectedDiamondBehavior.UpgradeResourceBunches.Select(urb => urb.ToResourceBunch()).ToList();

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
