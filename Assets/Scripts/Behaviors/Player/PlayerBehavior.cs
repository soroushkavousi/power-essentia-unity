using Assets.Scripts.Models;
using UnityEngine;

[RequireComponent(typeof(StateManagerBehavior))]
[RequireComponent(typeof(BodyBehavior))]
public abstract class PlayerBehavior : MonoBehaviour
{
    private static PlayerBehavior _main = default;
    [Header(Constants.HeaderStart + nameof(PlayerBehavior) + Constants.HeaderEnd)]
    [SerializeField] protected bool _isMainPlayer = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] protected bool _mouseIsDown = default;
    [SerializeField] protected PlayerDynamicData _dynamicData = default;
    private PlayerStaticData _staticData = default;
    protected StateManagerBehavior _stateManagerBehavior = default;
    protected BodyBehavior _bodyBehavior = default;

    public PlayerDynamicData DynamicData => _dynamicData;
    public bool IsMainPlayer => _isMainPlayer;
    public bool MouseIsDown { get => _mouseIsDown; set => _mouseIsDown = value; }
    public static PlayerBehavior Main => _main;
    public bool IsInitialized { get; }

    protected void Initialize(PlayerStaticData staticData)
    {
        if (_isMainPlayer)
            _main = this;

        _stateManagerBehavior = GetComponent<StateManagerBehavior>();
        _bodyBehavior = GetComponent<BodyBehavior>();
        _bodyBehavior.FeedData();
        _staticData = staticData;
        GetDynamicData();
    }

    protected virtual void Start()
    {
        if (SceneManagerBehavior.Instance.CurrentSceneName != SceneName.MISSION)
            gameObject.SetActive(false);
    }

    private void GetDynamicData()
    {
        if (IsMainPlayer)
            _dynamicData = PlayerDynamicDataTO.Instance.PlayerDynamicData;
    }

    public GameObject IsTargetEnemy(GameObject target)
    {
        var bodyAreaBehavior = target.GetComponent<BodyAreaBehavior>();
        if (bodyAreaBehavior != null)
        {
            target = bodyAreaBehavior.BodyBehavior.gameObject;
        }

        var invaderBehavior = target.GetComponent<DemonBehavior>();
        if (invaderBehavior != null)
            return target;

        return null;
    }
}
