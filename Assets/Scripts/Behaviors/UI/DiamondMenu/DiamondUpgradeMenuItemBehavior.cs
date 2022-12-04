using Assets.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondUpgradeMenuItemBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private DiamondUpgradeMenuBehavior _diamondUpgradeMenuBehavior = default;
    [SerializeField] private DiamondName _diamondName = default;
    [SerializeField] private Image _diamondImage = default;
    [SerializeField] private TextMeshProUGUI _diamondNameText = default;
    [SerializeField] private TextMeshProUGUI _diamondLevelText = default;
    [SerializeField] private Image _glow = default;
    private DiamondOwnerBehavior _diamondOwnerBehavior = default;
    private DiamondBehavior _diamondBehavior = default;
    private Observable<DiamondName> _selectedDiamondName = default;

    public DiamondBehavior DiamondBehavior => _diamondBehavior;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private bool _isGlowing = default;

    public void FeedData()
    {
        if (_diamondName == DiamondName.NONE)
        {
            gameObject.SetActive(false);
            return;
        }

        _selectedDiamondName = _diamondUpgradeMenuBehavior.SelectedDiamondName;

        _diamondOwnerBehavior = PlayerBehavior.MainPlayer.To<DiamondOwnerBehavior>();
        _selectedDiamondName.Attach(this);
        HandleSelectedDiamondChange();
        _diamondBehavior = _diamondOwnerBehavior.AllDiamondBehaviors[_diamondName];
        _diamondImage.sprite = _diamondBehavior.Icon;
        _diamondNameText.text = _diamondBehavior.ShowName;
        _diamondBehavior.Level.Attach(this);
        SetDiamondLevel();
    }

    public void Select()
    {
        var diamondName = _selectedDiamondName.Value;
        if (_diamondName == diamondName)
            return;
        _selectedDiamondName.Value = _diamondName;
    }

    private void HandleSelectedDiamondChange()
    {
        var diamondName = _selectedDiamondName.Value;
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

    private void SetDiamondLevel()
    {
        _diamondLevelText.text = $"Level {_diamondBehavior.Level.Value}";
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _selectedDiamondName)
        {
            HandleSelectedDiamondChange();
        }
        else if (subject == _diamondBehavior.Level)
        {
            SetDiamondLevel();
        }
    }
}
