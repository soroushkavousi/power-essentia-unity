﻿using Assets.Scripts.Models;
using System;
using UnityEngine;

public class MeleeWeaponBehavior : WeaponBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(MeleeWeaponBehavior) + Constants.HeaderEnd)]

    [SerializeField] private MeleeWeaponStaticData _staticData = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    public override void Initialize(Level level,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        base.FeedData(_staticData);
        base.Initialize(level, isTargetEnemyFunction);
    }

    public void StrikeTheEnemy(GameObject enemy)
    {
        if (enemy == null)
            return;
        var enemyHealthBehavior = enemy.GetComponent<HealthBehavior>();
        var criticalEffect = new CriticalEffect(AttackDamage.Value,
                    CriticalChance.Value,
                    CriticalDamage.Value);
        var damage = new Damage(DamageType.PHYSICAL, criticalEffect.Result,
            criticalEffect.IsApplied);
        enemyHealthBehavior.Health.Damage(damage);
        if (_staticData.HitSound != null)
            MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_staticData.HitSound, 0.4f);
        var hitParameters = new HitParameters(gameObject, default, enemy);
        Notify(hitParameters);
    }
}
