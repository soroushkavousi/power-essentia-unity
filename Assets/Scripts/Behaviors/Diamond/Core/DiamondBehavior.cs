using Assets.Scripts.Models;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public class DiamondBehavior : MonoBehaviour
{
    [SerializeField] private DiamondName _name = default;
    [SerializeField] private UnityEvent _InitializeEvent;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private DiamondStaticData _staticData = default;
    [SerializeField] private AdvancedBoolean _isDiscovered = default;
    [SerializeField] private AdvancedBoolean _isOwned = default;
    [SerializeField] private OnePartAdvancedNumber _level = default;
    [SerializeField] private ThreePartAdvancedNumber _activeTime = new ThreePartAdvancedNumber();
    [SerializeField] private ThreePartAdvancedNumber _cooldownTime = new ThreePartAdvancedNumber();
    [SerializeField] private bool _isReady = default;
    [SerializeField] private bool _onUsing = default;
    [SerializeField] private bool _onCooldown = default;
    [SerializeField] private float _ramainingTime = default;
    [SerializeField] private float _ramainingPercentage = default;
    [SerializeField] private bool _isPermanent = default;
    private DiamondOwnerBehavior _diamondOwnerBehavior = default;
    private AttackerBehavior _ownerAttackerBehavior = default;
    private Action _activateAction = default;
    private Action _deactivateAction = default;
    private Transform _diamondEffectsParent = default;

    public DiamondName Name => _name;
    public string ShowName => _staticData.ShowName;
    public Sprite Icon => _staticData.Icon;
    public ResourceBoxStaticData BuyOrUpgradeResourceBox => 
        _isOwned.Value ? _staticData.UpgradeResourceBoxes[_level.IntValue] : _staticData.UpgradeResourceBoxes[0];
    public AdvancedBoolean IsDiscovered => _isDiscovered;
    public AdvancedBoolean IsOwned => _isOwned;
    public OnePartAdvancedNumber Level => _level;
    public ThreePartAdvancedNumber ActiveTime => _activeTime;
    public ThreePartAdvancedNumber CooldownTime => _cooldownTime;
    public bool IsReady => _isReady;
    public bool OnUsing => _onUsing;
    public bool OnCooldown => _onCooldown;
    public float RamainingTime => _ramainingTime;
    public float RamainingPercentage => _ramainingPercentage;
    public string Description => _staticData.Description;
    public DiamondOwnerBehavior DiamondOwnerBehavior => _diamondOwnerBehavior;
    public AttackerBehavior OwnerAttackerBehavior => _ownerAttackerBehavior;
    public Func<string> GetDescription { get; set; }
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; private set; }
    public Transform DiamondEffectsParent => _diamondEffectsParent;

    public void Initialize(AdvancedBoolean isDiscovered, AdvancedBoolean isOwned,
        OnePartAdvancedNumber level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        _isReady = true;
        _isDiscovered = isDiscovered;
        _isOwned = isOwned;
        _level = level;
        _diamondOwnerBehavior = diamondOwnerBehavior;
        if (diamondOwnerBehavior != null)
            _ownerAttackerBehavior = diamondOwnerBehavior.GetComponent<AttackerBehavior>();
        IsTargetEnemyFunction = _ownerAttackerBehavior.IsTargetEnemyFunction;
        if (SceneManagerBehavior.Instance.CurrentSceneName == SceneName.MISSION)
            _diamondEffectsParent = MissionManagerBehavior.Instance.DiamondEffectsParent;
        else
            _diamondEffectsParent = OutBoxBehavior.Instance.Location1;
        _InitializeEvent.Invoke();
    }

    public void FeedData(DiamondStaticData staticData, 
        Action activateAction, Action deactivateAction)
    {
        _staticData = staticData;
        _activeTime.FeedData(_staticData.StartActiveTime);
        _cooldownTime.FeedData(_staticData.StartCooldownTime);
        _activateAction = activateAction;
        _deactivateAction = deactivateAction;
    }

    private void Update()
    {
        if (_isPermanent)
            return;

        if(_onUsing)
            UpdateRemainingTime(_activeTime.Value, Deactivate);
        else if (_onCooldown)
            UpdateRemainingTime(_cooldownTime.Value, GetReady);
    }

    public void Activate()
    {
        _isReady = false;
        _activateAction();
        _ramainingTime = _activeTime.Value;
        _onUsing = true;
    }

    public void Deactivate()
    {
        _onUsing = false;
        _deactivateAction();
        _ramainingTime = _cooldownTime.Value;
        _onCooldown = true;
    }

    public void GetReady()
    {
        _onCooldown = false;
        _isReady = true;
    }

    private void UpdateRemainingTime(float maxTime, Action finishingAction)
    {
        _ramainingTime -= Time.deltaTime;
        if (_ramainingTime < 0)
            _ramainingTime = 0;
        _ramainingPercentage = _ramainingTime / maxTime * 100;
        if (_ramainingPercentage == 0)
        {
            finishingAction();
        }
    }
}
