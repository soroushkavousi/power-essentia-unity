using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(HealthBehavior))]
public class WallBehavior : MonoBehaviour
{
    private static WallBehavior _instance = default;
    [SerializeField] private WallStaticData _data = default;

    //[Space(Constants.DebugSectionSpace, order = -1001)]
    //[Header(Constants.DebugSectionHeader, order = -1000)]

    private BodyBehavior _bodyBehavior = default;
    private HealthBehavior _healthBehavior = default;

    public static WallBehavior Instance => Utils.GetInstance(ref _instance);

    public void Initialize()
    {
        _bodyBehavior = GetComponent<BodyBehavior>();
        _healthBehavior = GetComponent<HealthBehavior>();

        _bodyBehavior.FeedData();
        _bodyBehavior.OnEnterActions.Add(OnBodyEnter);
        _healthBehavior.FeedData(_data.HealthData, true);
    }

    private void OnBodyEnter(Collider2D otherCollider)
    {
        var target = otherCollider.gameObject;
    }

    private void Start()
    {
        if (SceneManagerBehavior.Instance.CurrentSceneName != SceneName.MISSION)
            gameObject.SetActive(false);
    }
}
