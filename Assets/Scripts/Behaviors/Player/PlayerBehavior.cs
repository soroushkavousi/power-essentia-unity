using Assets.Scripts.Models;
using System.Linq;
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
    [SerializeField] protected PlayerDynamicData _dynamicData = null;
    protected bool _isInitialized = false;
    private PlayerStaticData _staticData = default;
    protected StateManagerBehavior _stateManagerBehavior = default;
    protected BodyBehavior _bodyBehavior = default;

    public PlayerDynamicData DynamicData => _dynamicData;
    public bool IsMainPlayer => _isMainPlayer;
    public bool MouseIsDown { get => _mouseIsDown; set => _mouseIsDown = value; }
    public static PlayerBehavior MainPlayer => FindMainPlayer();

    public virtual void Initialize()
    {
        _isInitialized = true;
    }

    protected void FeedData(PlayerStaticData staticData)
    {
        if (_isMainPlayer)
            _main = this;

        _stateManagerBehavior = GetComponent<StateManagerBehavior>();
        _bodyBehavior = GetComponent<BodyBehavior>();
        _bodyBehavior.FeedData();
        _staticData = staticData;
        GetDynamicData();
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

    private static PlayerBehavior FindMainPlayer()
    {
        if (_main == null || _main == default)
            _main = FindObjectsOfType<PlayerBehavior>(true).FirstOrDefault(p => p.IsMainPlayer == true);
        if (!_main._isInitialized)
            _main.Initialize();
        return _main;
    }
}
