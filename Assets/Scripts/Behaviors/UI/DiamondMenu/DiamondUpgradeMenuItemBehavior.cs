using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondUpgradeMenuItemBehavior : MonoBehaviour
{
    [SerializeField] private DiamondUpgradeMenuBehavior _diamondUpgradeMenuBehavior = default;
    [SerializeField] private DiamondName _diamondName = default;
    [SerializeField] private Image _diamondImage = default;
    [SerializeField] private TextMeshProUGUI _diamondNameText = default;
    [SerializeField] private Image _glow = default;
    private DiamondOwnerBehavior _diamondOwnerBehavior = default;
    private DiamondBehavior _diamondBehavior = default;
    private AdvancedString _selectedDiamondName = default;

    public DiamondBehavior DiamondBehavior => _diamondBehavior;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private bool _isGlowing = default;

    public void FeedData()
    {
        if (_diamondName == DiamondName.NONE)
        {
            gameObject.SetActive(false);
            return;
        }

        _selectedDiamondName = _diamondUpgradeMenuBehavior.SelectedDiamondName;

        _diamondOwnerBehavior = PlayerBehavior.Main.GetComponent<DiamondOwnerBehavior>();
        _selectedDiamondName.OnNewValueActions.Add(HandleSelectedDiamondChange);
        HandleSelectedDiamondChange(null);
        _diamondBehavior = _diamondOwnerBehavior.AllDiamondBehaviors[_diamondName];
        _diamondImage.sprite = _diamondBehavior.Icon;
        _diamondNameText.text = _diamondBehavior.ShowName;
    }

    public void Select()
    {
        var diamondName = _selectedDiamondName.EnumValue.To<DiamondName>();
        if (_diamondName == diamondName)
            return;
        _selectedDiamondName.Change(_diamondName, name);
    }

    private void HandleSelectedDiamondChange(StringChangeCommand changeCommand)
    {
        var diamondName = _selectedDiamondName.EnumValue.To<DiamondName>();
        if (_diamondName == diamondName)
            Glow();
        else
            UnGlow();
    }

    public void Glow()
    {
        _glow.gameObject.SetActive(true);
        _isGlowing = true;
    }

    public void UnGlow()
    {
        _glow.gameObject.SetActive(false);
        _isGlowing = false;
    }
}
