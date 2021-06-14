using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondGameDeckItemBehavior : MonoBehaviour
{
    [SerializeField] private RingName _ringName = default;
    [SerializeField] private int _index = default;
    [SerializeField] private KeyCode _keyCode = default;
    [SerializeField] private Image _diamondImage = default;
    [SerializeField] private TextMeshProUGUI _diamondNameText = default;
    [SerializeField] private Image _shadowImage = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private DiamondBehavior _diamondBehavior = default;

    public DiamondBehavior DiamondBehavior => _diamondBehavior;

    private void Awake()
    {
        GetDiamondBehavior();
        ShowDiamondDetails();
        _shadowImage.transform.localScale = new Vector3(1, 0, 1);
    }

    private void GetDiamondBehavior()
    {
        _diamondBehavior = PlayerBehavior.Main.GetComponent<DiamondOwnerBehavior>().RingDiamondBehaviorsMap[_ringName][_index];
        if (_diamondBehavior == default)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    private void ShowDiamondDetails()
    {
        if (_diamondBehavior == null)
        {
            _diamondImage.sprite = DefaultContainerBehavior.Instance.DiamondImage;
            _diamondNameText.text = DefaultContainerBehavior.Instance.DiamondName;
            return;
        }
        _diamondImage.sprite = _diamondBehavior.Icon;
        _diamondNameText.text = _diamondBehavior.ShowName;
    }

    public void HandleClickEvent()
    {
        if(_diamondBehavior.IsReady)
            _diamondBehavior.Activate();
    }

    private void Update()
    {
        if(!_diamondBehavior.IsReady)
            UpdateShadowState();

        HandleKeyboardEvent();
    }

    private void HandleKeyboardEvent()
    {
        if(Input.GetKeyDown(_keyCode))
            HandleClickEvent();
    }

    private void UpdateShadowState()
    {
        float shadowYScale;
        if (_diamondBehavior.OnUsing)
            shadowYScale = (100 - _diamondBehavior.RamainingPercentage) / 100;
        else if (_diamondBehavior.OnCooldown)
            shadowYScale = _diamondBehavior.RamainingPercentage / 100;
        else
            shadowYScale = 0;

        _shadowImage.transform.localScale = new Vector3(1, shadowYScale, 1);
    }
}
