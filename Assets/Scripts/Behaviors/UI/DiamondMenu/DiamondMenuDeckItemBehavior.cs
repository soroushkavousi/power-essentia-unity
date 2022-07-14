using Assets.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondMenuDeckItemBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private DiamondDeckRingMenuBehavior _diamondMenuDeckBehavior = default;
    [SerializeField] private RingName _ringName = default;
    [SerializeField] private int _index = default;
    [SerializeField] private Image _diamondImage = default;
    [SerializeField] private TextMeshProUGUI _diamondNameText = default;
    [SerializeField] private Image _glow = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private Observable<DiamondName> _diamondName = default;
    [SerializeField] private bool _isGlowing = default;
    private DiamondBehavior _diamondBehavior = default;
    private Observable<DiamondName> _selectedDiamondName = default;

    public DiamondBehavior DiamondBehavior => _diamondBehavior;
    public DiamondName DiamondName => _diamondName.Value;
    public bool IsGlowing => _isGlowing;

    private void Awake()
    {
        _diamondName = PlayerBehavior.MainPlayer.DynamicData.SelectedItems.RingDiamondNamesMap[_ringName][_index];
        _diamondName.Attach(this);
        _selectedDiamondName = PlayerBehavior.MainPlayer.DynamicData.SelectedItems.
            MenuDiamondName[_ringName];
        ShowDiamondDetails();
    }

    private void ShowDiamondDetails()
    {
        if (_diamondName.Value == DiamondName.NONE)
        {
            _diamondImage.sprite = GameManagerBehavior.Instance.Defaults.DiamondImage;
            _diamondNameText.text = GameManagerBehavior.Instance.Defaults.DiamondName;
            return;
        }
        _diamondBehavior = DiamondOwnerBehavior.MainDiamondOwner.AllDiamondBehaviors[_diamondName.Value];
        _diamondImage.sprite = _diamondBehavior.Icon;
        _diamondNameText.text = _diamondBehavior.ShowName;
    }

    public void HandleClickEvent()
    {
        var selectedDiamondName = _selectedDiamondName.Value;
        if (_isGlowing)
        {
            if (selectedDiamondName != _diamondName.Value)
                _diamondName.Value = selectedDiamondName;
            _diamondMenuDeckBehavior.FinishUseInDeckOperation();
        }
        else
        {
            if (_diamondName.Value == DiamondName.NONE)
                return;
            if (selectedDiamondName == _diamondName.Value)
                return;
            _selectedDiamondName.Value = _diamondName.Value;
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

    public void OnNotify(ISubject subject)
    {
        if (subject == _diamondName)
        {
            ShowDiamondDetails();
        }
    }
}
