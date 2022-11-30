using Assets.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondGameDeckItemBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private RingName _ringName = default;
    [SerializeField] private int _index = default;
    [SerializeField] private KeyCode _keyCode = default;
    [SerializeField] private Image _backgroundImage = default;
    [SerializeField] private Image _diamondImage = default;
    [SerializeField] private TextMeshProUGUI _diamondNameText = default;
    [SerializeField] private Image _shadowImage = default;
    [SerializeField] private Color _usingColor = default;
    [SerializeField] private Color _cooldownColor = default;
    [SerializeField] private Color _readyColor = default;
    [SerializeField] private Color _diamondColor = default;
    [SerializeField] private Color _diamondCooldownColor = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private PeriodicDiamondBehavior _periodicDiamondBehavior = default;

    public PeriodicDiamondBehavior PeriodicDiamondBehavior => _periodicDiamondBehavior;

    private void Awake()
    {
        GetDiamondBehavior();
        ShowDiamondDetails();
        _shadowImage.transform.localScale = new Vector3(1, 0, 1);
    }

    private void Start()
    {
        _backgroundImage.color = _readyColor;
        _periodicDiamondBehavior.State.Attach(this);
    }

    private void GetDiamondBehavior()
    {
        _periodicDiamondBehavior = DiamondOwnerBehavior.MainDiamondOwner
            .RingDiamondBehaviorsMap[_ringName][_index].To<PeriodicDiamondBehavior>();
        if (_periodicDiamondBehavior == default)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    private void ShowDiamondDetails()
    {
        if (_periodicDiamondBehavior == null)
        {
            _diamondImage.sprite = GameManagerBehavior.Instance.Defaults.DiamondImage;
            _diamondNameText.text = GameManagerBehavior.Instance.Defaults.DiamondName;
            return;
        }
        _diamondImage.sprite = _periodicDiamondBehavior.Icon;
        _diamondNameText.text = _periodicDiamondBehavior.ShowName;
    }

    public void HandleClickEvent()
    {
        if (_periodicDiamondBehavior.State.Value == DiamondState.READY)
        {
            _periodicDiamondBehavior.Activate();
        }
    }

    private void Update()
    {
        if (_periodicDiamondBehavior.State.Value != DiamondState.READY)
            UpdateShadowState();

        HandleKeyboardEvent();
    }

    private void HandleKeyboardEvent()
    {
        if (Input.GetKeyDown(_keyCode))
            HandleClickEvent();
    }

    private void UpdateShadowState()
    {
        float shadowYScale;
        if (_periodicDiamondBehavior.State.Value == DiamondState.USING)
            shadowYScale = (100 - _periodicDiamondBehavior.RamainingPercentage) / 100;
        else if (_periodicDiamondBehavior.State.Value == DiamondState.COOLDOWN)
            shadowYScale = _periodicDiamondBehavior.RamainingPercentage / 100;
        else
            shadowYScale = 0;

        _shadowImage.transform.localScale = new Vector3(1, shadowYScale, 1);
    }

    private void OnDiamondStateChanged()
    {
        switch (_periodicDiamondBehavior.State.Value)
        {
            case DiamondState.READY:
                _backgroundImage.color = _readyColor;
                _diamondImage.color = _diamondColor;
                break;
            case DiamondState.USING:
                _backgroundImage.color = _usingColor;
                _diamondImage.color = _diamondColor;
                break;
            case DiamondState.COOLDOWN:
                _backgroundImage.color = _cooldownColor;
                _diamondImage.color = _diamondCooldownColor;
                break;
        }
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _periodicDiamondBehavior.State)
        {
            OnDiamondStateChanged();
        }
    }
}
