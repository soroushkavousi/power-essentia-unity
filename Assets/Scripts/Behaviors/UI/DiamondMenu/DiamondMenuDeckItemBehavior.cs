using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondMenuDeckItemBehavior : MonoBehaviour
{
    [SerializeField] private DiamondMenuDeckBehavior _diamondMenuDeckBehavior = default;
    [SerializeField] private RingName _ringName = default;
    [SerializeField] private int _index = default;
    [SerializeField] private Image _diamondImage = default;
    [SerializeField] private TextMeshProUGUI _diamondNameText = default;
    [SerializeField] private Image _glow = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private DiamondName _diamondNameEnum = default;
    [SerializeField] private bool _isGlowing = default;
    private DiamondOwnerBehavior _diamondOwnerBehavior = default;
    private DiamondBehavior _diamondBehavior = default;
    private AdvancedString _diamondName = default;
    private AdvancedString _selectedDiamondName = default;

    public DiamondBehavior DiamondBehavior => _diamondBehavior;
    public DiamondName DiamondName => _diamondNameEnum;
    public bool IsGlowing => _isGlowing;

    private void Awake()
    {
        _diamondOwnerBehavior = PlayerBehavior.Main.GetComponent<DiamondOwnerBehavior>();
        _diamondName = PlayerBehavior.Main.DynamicData.SelectedItems.RingDiamondNamesMap[_ringName][_index];
        _diamondName.OnNewValueActions.Add(cc => ShowDiamondDetails());
        _selectedDiamondName = PlayerBehavior.Main.DynamicData.SelectedItems.
            MenuDiamondName[_ringName];
        ShowDiamondDetails();
    }

    private void ShowDiamondDetails()
    {
        _diamondNameEnum = _diamondName.EnumValue.To<DiamondName>();

        if (_diamondNameEnum == DiamondName.NONE)
        {
            _diamondImage.sprite = DefaultContainerBehavior.Instance.DiamondImage;
            _diamondNameText.text = DefaultContainerBehavior.Instance.DiamondName;
            return;
        }
        _diamondBehavior = _diamondOwnerBehavior.AllDiamondBehaviors[_diamondNameEnum];
        _diamondImage.sprite = _diamondBehavior.Icon;
        _diamondNameText.text = _diamondBehavior.ShowName;
    }

    public void HandleClickEvent()
    {
        var selectedDiamondName = _selectedDiamondName.EnumValue.To<DiamondName>();
        if (_isGlowing)
        {
            if (selectedDiamondName != _diamondNameEnum)
                _diamondName.Change(selectedDiamondName, name);
            _diamondMenuDeckBehavior.FinishUseInDeckOperation();
        }
        else
        {
            if (_diamondNameEnum == DiamondName.NONE)
                return;
            if (selectedDiamondName == _diamondNameEnum)
                return;
            _selectedDiamondName.Change(_diamondNameEnum, name);
        }
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
