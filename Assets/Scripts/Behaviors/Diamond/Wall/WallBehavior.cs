using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(HealthBehavior))]
public class WallBehavior : MonoBehaviour
{
    private static WallBehavior _instance = default;
    [SerializeField] private WallStaticData _staticData = default;
    protected Observable<int> _level = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private BodyBehavior _bodyBehavior = default;
    private HealthBehavior _healthBehavior = default;

    public static WallBehavior Instance => Utils.GetInstance(ref _instance);
    public HealthBehavior HealthBehavior => _healthBehavior;

    public void Initialize(Observable<int> level)
    {
        _healthBehavior = GetComponent<HealthBehavior>();
        _bodyBehavior = GetComponent<BodyBehavior>();
        _bodyBehavior.FeedData();

        _level = level;
        _healthBehavior.FeedData(_staticData.HealthData, _level);
    }

    private void Start()
    {
        if (SceneManagerBehavior.Instance.CurrentSceneName != SceneName.MISSION)
            gameObject.SetActive(false);
    }
}
