using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(DiamondBehavior))]
public class FireDiamondBehavior : MonoBehaviour
{
    [SerializeField] private FireDiamondStaticData _staticData = default;
    [SerializeField] private GroundFireBehavior _sampleGroundFireBehavior = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField, TextArea] private string _description = default;
    [SerializeField] private ThreePartAdvancedNumber _groundFireChance = new ThreePartAdvancedNumber();
    private DiamondBehavior _diamondBehavior = default;

    public string Description => _description;

    public void Initialize()
    {
        _diamondBehavior = GetComponent<DiamondBehavior>();
        _diamondBehavior.FeedData(_staticData.DiamondStaticData, Activate, Deactivate);
        _diamondBehavior.Level.OnNewValueActions.Add(50, OnDiamondUpgraded);
        _diamondBehavior.GetDescription = GetDescription;
        _groundFireChance.FeedData(_staticData.StartGroundFireChance);
        ApplyLevel();
        _sampleGroundFireBehavior = CreateGroundFire(OutBoxBehavior.Instance.Location1.gameObject);
        _sampleGroundFireBehavior.name = "SampleGroundFire";
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
        if (UnityEngine.Random.value * 100 > _groundFireChance.Value)
            return;
        CreateGroundFire(hitParameters.Destination);
    }

    private GroundFireBehavior CreateGroundFire(GameObject targetEnemy)
    {
        var groundFireBehavior = Instantiate(_staticData.GroundFireBehavior, 
            _diamondBehavior.DiamondEffectsParent);
        InitializeGroundFire(groundFireBehavior, targetEnemy);

        return groundFireBehavior;
    }

    private void InitializeGroundFire(GroundFireBehavior groundFireBehavior, GameObject targetEnemy)
    {
        groundFireBehavior.Initialize(targetEnemy, _diamondBehavior.IsTargetEnemyFunction);
        ApplyLevelToGroundFire(groundFireBehavior);
    }

    private void ApplyLevelToGroundFire(GroundFireBehavior groundFireBehavior)
    {
        var diamondLevel = _diamondBehavior.Level.IntValue;

        var groundFireDurationIncrement = diamondLevel * _staticData.GroundFireDurationPerLevel;
        groundFireBehavior.Duration.Base.Change(groundFireDurationIncrement, name, "LEVEL");

        var groundFireDamageIncrement = diamondLevel * _staticData.GroundFireDamagePerLevel;
        groundFireBehavior.Damage.Base.Change(groundFireDamageIncrement, name, "LEVEL");

        var groundFireSlowIncrement = diamondLevel * _staticData.GroundFireSlowPerLevel;
        groundFireBehavior.Slow.Base.Change(groundFireSlowIncrement, name, "LEVEL");

        var groundFireCriticalChanceIncrement = diamondLevel * _staticData.GroundFireCriticalChancePerLevel;
        groundFireBehavior.CriticalChance.Base.Change(groundFireCriticalChanceIncrement, name, "LEVEL");

        var groundFireCriticalDamageIncrement = diamondLevel * _staticData.GroundFireCriticalDamagePerLevel;
        groundFireBehavior.CriticalDamage.Base.Change(groundFireCriticalDamageIncrement, name, "LEVEL");
    }

    private void OnDiamondUpgraded(NumberChangeCommand changeCommand)
    {
        if (changeCommand.Amount.IntValue != 1)
        {
            Debug.LogError($"Too much upgrades!!!!!!");
            return;
        }

        _groundFireChance.Base.Change(_staticData.GroundFireChancePerLevel, name, "UPGRADE_LEVEL");
        _sampleGroundFireBehavior.Duration.Base.Change(_staticData.GroundFireDurationPerLevel, name, "UPGRADE_LEVEL");
        _sampleGroundFireBehavior.Damage.Base.Change(_staticData.GroundFireDamagePerLevel, name, "UPGRADE_LEVEL");
        _sampleGroundFireBehavior.Slow.Base.Change(_staticData.GroundFireSlowPerLevel, name, "UPGRADE_LEVEL");
        _sampleGroundFireBehavior.CriticalChance.Base.Change(_staticData.GroundFireCriticalChancePerLevel, name, "UPGRADE_LEVEL");
        _sampleGroundFireBehavior.CriticalDamage.Base.Change(_staticData.GroundFireCriticalDamagePerLevel, name, "UPGRADE_LEVEL");
    }

    private void ApplyLevel()
    {
        var diamondLevel = _diamondBehavior.Level.IntValue;

        var groundFireChanceIncrement = diamondLevel * _staticData.GroundFireChancePerLevel;
        _groundFireChance.Base.Change(groundFireChanceIncrement, name, "LEVEL");
    }

    public string GetDescription()
    {
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentGroundFireChance = _groundFireChance.Value;
        var nextGroundFireChance = currentGroundFireChance + _staticData.GroundFireChancePerLevel;

        var currentGroundFireChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentGroundFireChance + "%", "black"));
        var nextGroundFireChanceShow = NoteUtils.AddColor(nextGroundFireChance + "%", upgradeColor);

        //------------------------------------------------

        var currentDuration = _sampleGroundFireBehavior.Duration.Value;
        var nextDuration = _sampleGroundFireBehavior.Duration.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.GroundFireDurationPerLevel);

        var currentDurationShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentDuration + "s", "black"));
        var nextDurationShow = NoteUtils.AddColor(nextDuration + "s", upgradeColor);

        //------------------------------------------------

        var currentDamage = _sampleGroundFireBehavior.Damage.Value;
        var nextDamage = _sampleGroundFireBehavior.Damage.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.GroundFireDamagePerLevel);

        var currentDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentDamage + "dps", "black"));
        var nextDamageShow = NoteUtils.AddColor(nextDamage + "dps", upgradeColor);

        //------------------------------------------------

        var currentSlow = _sampleGroundFireBehavior.Slow.Value;
        var nextSlow = _sampleGroundFireBehavior.Slow.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.GroundFireSlowPerLevel);

        var currentSlowShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentSlow + "%", "black"));
        var nextSlowShow = NoteUtils.AddColor(nextSlow + "%", upgradeColor);

        //------------------------------------------------

        var currentCriticalChance = _sampleGroundFireBehavior.CriticalChance.Value;
        var nextCriticalChance = _sampleGroundFireBehavior.CriticalChance.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.GroundFireCriticalChancePerLevel);

        var currentCriticalChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalChance + "%", "black"));
        var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", upgradeColor);

        //------------------------------------------------

        var currentCriticalDamage = _sampleGroundFireBehavior.CriticalDamage.Value;
        var nextCriticalDamage = _sampleGroundFireBehavior.CriticalDamage.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.GroundFireCriticalDamagePerLevel);

        var currentCriticalDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalDamage + "%", "black"));
        var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"It has a chance to spawn a ground fire on bow hit.\n\n" +
            $" Chance: {currentGroundFireChanceShow} (next: {nextGroundFireChanceShow})\n" +
            $" Duration: {currentDurationShow} (next: {nextDurationShow})\n" +
            $" Damage: {currentDamageShow} (next: {nextDamageShow})\n" +
            $" Slow: {currentSlowShow} (next: {nextSlowShow})\n" +
            $" Critical chance: {currentCriticalChanceShow} (next: {nextCriticalChanceShow})\n" +
            $" Critical damage: {currentCriticalDamageShow} (next: {nextCriticalDamageShow})";
        return description;
    }
}
