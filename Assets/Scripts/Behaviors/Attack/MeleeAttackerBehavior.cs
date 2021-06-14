using Assets.Scripts.Enums;
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
        enemyHealthBehavior.Health.Current.Change(_attackerBehavior.AttackDamage.Value * -1,
            name, HealthChangeType.PHYSICAL_DAMAGE, "Melee attaker striked.");
    }
}
