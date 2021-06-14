using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(DiamondBehavior))]
public class StoneDiamondBehavior : MonoBehaviour
{
    [SerializeField] private StoneDiamondStaticData _staticData = default;
    [SerializeField] private FallingStoneBehavior _sampleFallingStoneBehavior = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField, TextArea] private string _description = default;
    [SerializeField] private ThreePartAdvancedNumber _fallingStoneChance = new ThreePartAdvancedNumber();
    private DiamondBehavior _diamondBehavior = default;

    public void Initialize()
    {
        _diamondBehavior = GetComponent<DiamondBehavior>();
        _diamondBehavior.FeedData(_staticData.DiamondStaticData, Activate, Deactivate);
        _diamondBehavior.Level.OnNewValueActions.Add(50, OnDiamondUpgraded);
        _diamondBehavior.GetDescription = GetDescription;
        _fallingStoneChance.FeedData(_staticData.StartFallingStoneChance);
        ApplyLevel();
        _sampleFallingStoneBehavior = CreateFallingStone(OutBoxBehavior.Instance.Location1.gameObject);
        _sampleFallingStoneBehavior.name = "SampleFallingStone";
    }

    private void Activate()
    {
        _diamondBehavior.OwnerAttackerBehavior.OnHitActions.Add(HandleHitEvent);
    }

    private void Deactivate()
    {
        _diamondBehavior.OwnerAttackerBehavior.OnHitActions.Remove(HandleHitEvent);
    }

    private void HandleHitEvent(HitParameters hitParameters)
    {
        if (UnityEngine.Random.value * 100 > _fallingStoneChance.Value)
            return;
        CreateFallingStone(hitParameters.Destination);
    }

    private FallingStoneBehavior CreateFallingStone(GameObject targetEnemy)
    {
        var fallingStoneBehavior = Instantiate(_staticData.FallingStoneBehavior, 
            _diamondBehavior.DiamondEffectsParent);
        InitializeFallingStone(fallingStoneBehavior, targetEnemy);

        return fallingStoneBehavior;
    }

    private void InitializeFallingStone(FallingStoneBehavior fallingStoneBehavior, GameObject targetEnemy)
    {
        fallingStoneBehavior.Initialize(targetEnemy, _diamondBehavior.IsTargetEnemyFunction);
        ApplyLevelToFallingStone(fallingStoneBehavior);
    }

    private void ApplyLevelToFallingStone(FallingStoneBehavior fallingStoneBehavior)
    {
        var diamondLevel = _diamondBehavior.Level.IntValue;

        var fallingStoneDurationIncrement = diamondLevel * _staticData.FallingStoneImpactDamagePerLevel;
        fallingStoneBehavior.ImpactDamage.Base.Change(fallingStoneDurationIncrement, name, "LEVEL");

        var fallingStoneDamageIncrement = diamondLevel * _staticData.FallingStoneStunDurationPerLevel;
        fallingStoneBehavior.StunDuration.Base.Change(fallingStoneDamageIncrement, name, "LEVEL");

        var fallingStoneCriticalChanceIncrement = diamondLevel * _staticData.FallingStoneCriticalChancePerLevel;
        fallingStoneBehavior.CriticalChance.Base.Change(fallingStoneCriticalChanceIncrement, name, "LEVEL");

        var fallingStoneCriticalDamageIncrement = diamondLevel * _staticData.FallingStoneCriticalDamagePerLevel;
        fallingStoneBehavior.CriticalDamage.Base.Change(fallingStoneCriticalDamageIncrement, name, "LEVEL");
    }

    private void OnDiamondUpgraded(NumberChangeCommand changeCommand)
    {
        if (changeCommand.Amount.IntValue != 1)
        {
            Debug.LogError($"Too much upgrades!!!!!!");
            return;
        }

        _fallingStoneChance.Base.Change(_staticData.FallingStoneChancePerLevel, name, "UPGRADE_LEVEL");
        _sampleFallingStoneBehavior.ImpactDamage.Base.Change(_staticData.FallingStoneImpactDamagePerLevel, name, "UPGRADE_LEVEL");
        _sampleFallingStoneBehavior.StunDuration.Base.Change(_staticData.FallingStoneStunDurationPerLevel, name, "UPGRADE_LEVEL");
        _sampleFallingStoneBehavior.CriticalChance.Base.Change(_staticData.FallingStoneCriticalChancePerLevel, name, "UPGRADE_LEVEL");
        _sampleFallingStoneBehavior.CriticalDamage.Base.Change(_staticData.FallingStoneCriticalDamagePerLevel, name, "UPGRADE_LEVEL");
    }

    private void ApplyLevel()
    {
        var diamondLevel = _diamondBehavior.Level.IntValue;

        var fallingStoneChanceIncrement = diamondLevel * _staticData.FallingStoneChancePerLevel;
        _fallingStoneChance.Base.Change(fallingStoneChanceIncrement, name, "LEVEL");
    }

    public string GetDescription()
    {
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentFallingStoneChance = _fallingStoneChance.IntValue;
        var nextFallingStoneChance = currentFallingStoneChance + _staticData.FallingStoneChancePerLevel;

        var currentFallingStoneChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentFallingStoneChance + "%", "black"));
        var nextFallingStoneChanceShow = NoteUtils.AddColor(nextFallingStoneChance + "%", upgradeColor);

        //------------------------------------------------

        var currentImpactDamage = _sampleFallingStoneBehavior.ImpactDamage.Value;
        var nextImpactDamage = _sampleFallingStoneBehavior.ImpactDamage.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.FallingStoneImpactDamagePerLevel);

        var currentImpactDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentImpactDamage, "black"));
        var nextImpactDamageShow = NoteUtils.AddColor(nextImpactDamage, upgradeColor);

        //------------------------------------------------

        var currentStunDuration = _sampleFallingStoneBehavior.StunDuration.Value;
        var nextStunDuration = _sampleFallingStoneBehavior.StunDuration.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.FallingStoneStunDurationPerLevel);

        var currentStunDurationShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentStunDuration + "s", "black"));
        var nextStunDurationShow = NoteUtils.AddColor(nextStunDuration + "s", upgradeColor);

        //------------------------------------------------

        var currentCriticalChance = _sampleFallingStoneBehavior.CriticalChance.Value;
        var nextCriticalChance = _sampleFallingStoneBehavior.CriticalChance.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.FallingStoneCriticalChancePerLevel);

        var currentCriticalChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalChance + "%", "black"));
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", upgradeColor);

        //------------------------------------------------

        var currentCriticalDamage = _sampleFallingStoneBehavior.CriticalDamage.Value;
        var nextCriticalDamage = _sampleFallingStoneBehavior.CriticalDamage.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.FallingStoneCriticalDamagePerLevel);

        var currentCriticalDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalDamage + "%", "black"));
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"It has a chance to fall a stone on the enemy target.\n\n" +
            $" Chance: {currentFallingStoneChanceShow} (next: {nextFallingStoneChanceShow})\n" +
            $" Impact Damage: {currentImpactDamageShow} (next: {nextImpactDamageShow})\n" +
            $" Duration: {currentStunDurationShow} (next: {nextStunDurationShow})\n" +
            $" Critical chance: {currentCriticalChanceShow} (next: {nextCriticalChanceShow})\n" +
            $" Critical damage: {currentCriticalDamageShow} (next: {nextCriticalDamageShow})";
        return description;
    }
}
