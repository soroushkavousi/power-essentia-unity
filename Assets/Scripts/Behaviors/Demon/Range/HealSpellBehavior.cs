using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(SpellBehavior))]
public class HealSpellBehavior : MonoBehaviour
{
    [SerializeField] private CircleCollider2D _healAreaCircle = default;
    [SerializeField] private float _maxHealAreaRadius = default;
    [SerializeField] private float _healAreaGrowTime = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    private BodyBehavior _bodyBehavior = default;
    private SpellBehavior _spellBehavior = default;
    private ParticleSystem _particleSystem = default;

    public void FeedData()
    {
        _bodyBehavior = GetComponent<BodyBehavior>();
        _spellBehavior = GetComponent<SpellBehavior>();
        _particleSystem = GetComponent<ParticleSystem>();
        _healAreaCircle.radius = 0;

        _bodyBehavior.FeedData();
        _bodyBehavior.OnEnterActions.Add(HealIfDemon);
        _spellBehavior.FeedData(HealAllies);
    }


    private void Update()
    {

    }

    private void HealAllies()
    {
        _particleSystem.Play(true);
        StartCoroutine(GrowHealAreaSmouthly());
    }

    private IEnumerator GrowHealAreaSmouthly()
    {
        var growDelayRatio = 0.05f;
        var stepsCount = _healAreaGrowTime / growDelayRatio;
        var growRaito = _maxHealAreaRadius / stepsCount;
        //var growRatio = new Vector3(0.05f, 0.05f, 0.05f);
        while (_healAreaCircle.radius < _maxHealAreaRadius)
        {
            _healAreaCircle.radius +=  growRaito;
            //transform.localScale += growRatio;
            yield return new WaitForSeconds(growDelayRatio);
        }
        yield return new WaitForSeconds(growDelayRatio);
        _healAreaCircle.radius = 0;
        //transform.localScale = Vector3.zero;
    }

    private void HealIfDemon(Collider2D otherCollider)
    {
        var bodyAreaBehavior = otherCollider.GetComponent<BodyAreaBehavior>();
        if (bodyAreaBehavior == null)
            return;

        var invaderBehavior = bodyAreaBehavior.BodyBehavior.GetComponent<DemonBehavior>();
        if (invaderBehavior == null)
            return;

        StartCoroutine(HealDemon(invaderBehavior));
    }

    private IEnumerator HealDemon(DemonBehavior invaderBehavior)
    {
        if (invaderBehavior == null)
            yield break;
        var invaderHealthBehavior = invaderBehavior.GetComponent<HealthBehavior>();
        if (invaderHealthBehavior == null)
            yield break;
        invaderHealthBehavior.Health.Current.Change(+500, name,
            HealthChangeType.HEAL);
    }
}