using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackerBehavior))]
public class MeleeAttackerBehavior : MonoBehaviour
{
    private AttackerBehavior _attackerBehavior = default;

    public void FeedData(MeleeAttackerStaticData data, 
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _attackerBehavior = GetComponent<AttackerBehavior>();
      
        _attackerBehavior.FeedData(data.AttackerData, isTargetEnemyFunction);
        _attackerBehavior.OnAttackingActions.Add(StrikeTheEnemy);
    }

    private void StrikeTheEnemy()
    {
        if (_attackerBehavior.CurrentEnemy == null)
            return;
        var enemyHealthBehavior = _attackerBehavior.CurrentEnemy.GetComponent<HealthBehavior>();
        var criticalEffect = new CriticalEffect(-_attackerBehavior.AttackDamage.Value,
                    _attackerBehavior.CriticalChance.Value, _attackerBehavior.CriticalDamage.Value);
        var damage = criticalEffect.Result;
        var healthChangeType = criticalEffect.IsApplied ?
            HealthChangeType.CRITICAL_PHYSICAL_DAMAGE :
            HealthChangeType.PHYSICAL_DAMAGE;
        enemyHealthBehavior.Health.Current.Change(damage,
            name, healthChangeType);
    }
}
