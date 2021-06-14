using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondUpgradeMenuBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _diamondTitle = default;
    [SerializeField] private TextMeshProUGUI _diamondDetails = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeCoinDisplay = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeDemonBloodDisplay = default;
    [SerializeField] private ResourceDisplayBehavior _upgradeDarkDemonBloodDisplay = default;
    [SerializeField] private ButtonBehavior _upgradeButtonBehavior = default;
    [SerializeField] private ButtonBehavior _buyButtonBehavior = default;
    [SerializeField] private List<DiamondUpgradeMenuItemBehavior> _items = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private AdvancedString _selectedDiamondName = default;
    [SerializeField] private DiamondBehavior _selectedDiamondBehavior = default;

    private DiamondOwnerBehavior _diamondOwnerBehavior = default;

    public AdvancedString SelectedDiamondName => _selectedDiamondName;
    public DiamondBehavior SelectedDiamondBehavior => _selectedDiamondBehavior;
    public DiamondOwnerBehavior DiamondOwnerBehavior => _diamondOwnerBehavior;
    public OrderedList<Action> OnShowSelectedDiamondDetailsActions { get; } = new OrderedList<Action>();

    public void FeedData(AdvancedString selectedDiamondName)
    {
        _selectedDiamondName = selectedDiamondName;
        _selectedDiamondName.OnNewValueActions.Add(10, cc => ChangeSelectedDiamond());
        _diamondOwnerBehavior = PlayerBehavior.Main.GetComponent<DiamondOwnerBehavior>();
        ChangeSelectedDiamond();
        _items.ForEach(item => item.FeedData());
    }

    private void ChangeSelectedDiamond()
    {
        var diamondName = _selectedDiamondName.EnumValue.To<DiamondName>();
        if (diamondName == DiamondName.NONE)
            return;

        _selectedDiamondBehavior = _diamondOwnerBehavior.AllDiamondBehaviors[diamondName];
        ShowSelectedDiamondDetails();
    }

    public void BuyDiamond()
    {
        ConsumeResources();
        _selectedDiamondBehavior.IsOwned.Change(true, name, "Bought");
        ShowSelectedDiamondDetails();
    }

    public void UpgradeDiamond()
    {
        ConsumeResources();
        _selectedDiamondBehavior.Level.Change(1, name, "Upgraded");
        ShowSelectedDiamondDetails();
    }

    private void ConsumeResources()
    {
        var resourceBox = _selectedDiamondBehavior.BuyOrUpgradeResourceBox;
        var playerResourceBox = PlayerBehavior.Main.DynamicData.ResourceBox;

        var upgradeCoinAmount = resourceBox.GetResourceAmount(ResourceType.COIN);
        if (upgradeCoinAmount != 0)
        {
            playerResourceBox.ResourceBunches[ResourceType.COIN]
                .Change(-upgradeCoinAmount, name, "");
        }

        var upgradeDemonBloodAmount = resourceBox.GetResourceAmount(ResourceType.DEMON_BLOOD);
        if (upgradeDemonBloodAmount != 0)
        {
            playerResourceBox.ResourceBunches[ResourceType.DEMON_BLOOD]
                .Change(-upgradeDemonBloodAmount, name, "");
        }

        var upgradeDarkDemonBloodAmount = resourceBox.GetResourceAmount(ResourceType.DARK_DEMON_BLOOD);
        if (upgradeDarkDemonBloodAmount != 0)
        {
            playerResourceBox.ResourceBunches[ResourceType.DARK_DEMON_BLOOD]
                .Change(-upgradeDarkDemonBloodAmount, name, "");
        }
    }

    private void ShowSelectedDiamondDetails()
    {
        var level = _selectedDiamondBehavior.Level.IntValue;
        var title = $"{_selectedDiamondBehavior.ShowName} Diamond (Level {level})";
        _diamondTitle.text = title;

        _diamondDetails.text = _selectedDiamondBehavior.GetDescription();
        UpdateUpgradeDetails();
        _upgradeButtonBehavior.Owner.SetActive(_selectedDiamondBehavior.IsOwned.Value);
        _buyButtonBehavior.Owner.SetActive(!_selectedDiamondBehavior.IsOwned.Value);
        OnShowSelectedDiamondDetailsActions.CallActionsSafely();
    }

    private void UpdateUpgradeDetails()
    {
        var upgradeResourceBox = _selectedDiamondBehavior.BuyOrUpgradeResourceBox;

        var upgradeCoinAmount = upgradeResourceBox.GetResourceAmount(ResourceType.COIN);
        _upgradeCoinDisplay.AmountText.text = upgradeCoinAmount.ToString();
        _upgradeCoinDisplay.gameObject.SetActive(upgradeCoinAmount != 0);

        var upgradeDemonBloodAmount = upgradeResourceBox.GetResourceAmount(ResourceType.DEMON_BLOOD);
        _upgradeDemonBloodDisplay.AmountText.text = upgradeDemonBloodAmount.ToString();
        _upgradeDemonBloodDisplay.gameObject.SetActive(upgradeDemonBloodAmount != 0);

        var upgradeDarkDemonBloodAmount = upgradeResourceBox.GetResourceAmount(ResourceType.DARK_DEMON_BLOOD);
        _upgradeDarkDemonBloodDisplay.AmountText.text = upgradeDarkDemonBloodAmount.ToString();
        _upgradeDarkDemonBloodDisplay.gameObject.SetActive(upgradeDarkDemonBloodAmount != 0);

        var playerResourceBox = PlayerBehavior.Main.DynamicData.ResourceBox;
        var playerCoinAmount = playerResourceBox.ResourceBunches[ResourceType.COIN].LongValue;
        var playerDemonBloodAmount = playerResourceBox.ResourceBunches[ResourceType.DEMON_BLOOD].LongValue;
        var playerDarkDemonBloodAmount = playerResourceBox.ResourceBunches[ResourceType.DARK_DEMON_BLOOD].LongValue;

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
}
