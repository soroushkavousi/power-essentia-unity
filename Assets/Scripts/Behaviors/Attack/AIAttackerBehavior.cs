﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(VisionBehavior))]
[RequireComponent(typeof(AttackerBehavior))]
public class AIAttackerBehavior : MonoBehaviour
{

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private List<GameObject> _enemiesInVision = new List<GameObject>();
    [SerializeField] public GameObject _closestEnemy = default;
    [SerializeField] private AIAttackerStaticData _staticData = default;

    private VisionBehavior _visionBehavior = default;
    private AttackerBehavior _attackerBehavior = default;
    public List<GameObject> EnemiesInVision => _enemiesInVision;
    public bool EnemiesAreInVision => _enemiesInVision.Count != 0;
    public OrderedList<Action<GameObject>> OnNewEnemyIsInMyVisionActions { get; } = new OrderedList<Action<GameObject>>();
    public OrderedList<Action> OnCurrentTargetEnemyDiedActions { get; } = new OrderedList<Action>();
    public OrderedList<Action<GameObject>> OnCurrentTargetEnemyEscapedActions { get; } = new OrderedList<Action<GameObject>>();
    public OrderedList<Action<GameObject>> OnEnemyIsOutOfMyVisionActions { get; } = new OrderedList<Action<GameObject>>();

    public void FeedData(AIAttackerStaticData staticData)
    {
        _staticData = staticData;

        _visionBehavior = GetComponent<VisionBehavior>();
        _visionBehavior.FeedData(staticData.VisionStaticData);
        _visionBehavior.OnEnterActions.Add(TrackIfTargetEnemyIsInVision);
        _visionBehavior.OnExitActions.Add(UnTrackIfTargetEnemyIsOutOfVision);

        _attackerBehavior = GetComponent<AttackerBehavior>();
    }

    private void Start()
    {
        StartCoroutine(FindClosestEnemy());
    }

    private void Update()
    {
        RemoveDeadEnemiesFromVision();
    }

    private void TrackIfTargetEnemyIsInVision(Collider2D otherCollider)
    {
        var enemy = _attackerBehavior.IsTargetEnemyFunction(otherCollider.gameObject);
        if (enemy != null)
            OnNewEnemyIsInMyVision(enemy);
    }

    private void UnTrackIfTargetEnemyIsOutOfVision(Collider2D otherCollider)
    {
        var enemy = _attackerBehavior.IsTargetEnemyFunction(otherCollider.gameObject);
        if (enemy != null)
            OnEnemyIsOutOfMyVision(enemy);
    }

    private void OnNewEnemyIsInMyVision(GameObject enemy)
    {
        _enemiesInVision.Add(enemy);
        OnNewEnemyIsInMyVisionActions.CallActionsSafely(enemy);
    }

    private void OnEnemyIsOutOfMyVision(GameObject enemy)
    {
        _enemiesInVision.Remove(enemy);
        //if (_currentTargetEnemy == enemy)
        //{
        //    if (enemy == null)
        //        OnCurrentTargetEnemyDied();

        //    var enemyHealthBehavior = enemy.GetComponent<HealthBehavior>();
        //    if (enemyHealthBehavior.IsDead)
        //        OnCurrentTargetEnemyDied();
        //    else
        //        OnCurrentTargetEnemyEscaped(enemy);
        //}
        OnEnemyIsOutOfMyVisionActions.CallActionsSafely(enemy);
    }

    private void RemoveDeadEnemiesFromVision()
    {
        if (_enemiesInVision.Count != 0)
        {
            foreach (var enemy in _enemiesInVision)
            {
                if (enemy == null)
                {
                    _enemiesInVision.Remove(enemy);
                    Debug.Log($"RemoveDeadEnemiesFromVision {enemy.name} removed from list.");
                }
            }
        }
    }

    public IEnumerator FindClosestEnemy()
    {
        while (true)
        {
            if (_enemiesInVision.Count != 0)
            {
                _closestEnemy = _enemiesInVision.FindCLosestGameObject(gameObject);
                _attackerBehavior.CurrentEnemy = _closestEnemy;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
