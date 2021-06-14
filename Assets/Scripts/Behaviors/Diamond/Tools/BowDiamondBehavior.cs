using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(DiamondBehavior))]
public class BowDiamondBehavior : MonoBehaviour
{
    [SerializeField] private BowDiamondStaticData _staticData = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField, TextArea] private string _description = default;
    private DiamondBehavior _diamondBehavior = default;

    public string Description => _description;

    public void Initialize()
    {
        _diamondBehavior = GetComponent<DiamondBehavior>();

        _diamondBehavior.FeedData(_staticData.DiamondStaticData, Activate, Deactivate);
        _diamondBehavior.Level.OnNewValueActions.Add(50, OnDiamondUpgraded);
        _diamondBehavior.GetDescription = GetDescription;
    }

    private void Activate()
    {
        var diamondLevel = _diamondBehavior.Level.IntValue;

        var damageBaseIncrement = diamondLevel * _staticData.DamageBasePerLevel;
        _diamondBehavior.OwnerAttackerBehavior.AttackDamage.Base.Change(damageBaseIncrement, name, "BOW_DIAMOND_ACTIVATION");

        var fireRateBaseIncrement = diamondLevel * _staticData.FireRateBasePerLevel;
        _diamondBehavior.OwnerAttackerBehavior.AttackSpeed.Base.Change(fireRateBaseIncrement, name, "BOW_DIAMOND_ACTIVATION");

        var criticalChanceIncrement = diamondLevel * _staticData.CriticalChanceBasePerLevel;
        //Todo
        //_diamondBehavior.OwnerAttackerBehavior.CriticalChance.Base.Change(criticalChanceIncrement, name, "BOW_DIAMOND_ACTIVATION");

        var criticalDamageIncrement = diamondLevel * _staticData.CriticalDamageBasePerLevel;
        //Todo
        //_diamondBehavior.OwnerAttackerBehavior.CriticalDamage.Base.Change(criticalDamageIncrement, name, "BOW_DIAMOND_ACTIVATION");
    }

    private void Deactivate()
    {
        var diamondLevel = _diamondBehavior.Level.IntValue;

        var damageBaseIncrement = diamondLevel * _staticData.DamageBasePerLevel;
        _diamondBehavior.OwnerAttackerBehavior.AttackDamage.Base.Change(-damageBaseIncrement, name, "BOW_DIAMOND_DEACTIVATION");

        var fireRateBaseIncrement = diamondLevel * _staticData.FireRateBasePerLevel;
        _diamondBehavior.OwnerAttackerBehavior.AttackSpeed.Base.Change(-fireRateBaseIncrement, name, "BOW_DIAMOND_DEACTIVATION");

        var criticalChanceIncrement = diamondLevel * _staticData.CriticalChanceBasePerLevel;
        //Todo
        //_diamondBehavior.OwnerAttackerBehavior.CriticalChance.Base.Change(-criticalChanceIncrement, name, "BOW_DIAMOND_DEACTIVATION");

        var criticalDamageIncrement = diamondLevel * _staticData.CriticalDamageBasePerLevel;
        //Todo
        //_diamondBehavior.OwnerAttackerBehavior.CriticalDamage.Base.Change(-criticalDamageIncrement, name, "BOW_DIAMOND_DEACTIVATION");
    }

    private void OnDiamondUpgraded(NumberChangeCommand changeCommand)
    {
        if(changeCommand.Amount.IntValue != 1)
        {
            Debug.LogError($"Too much upgrades!!!!!!");
            return;
        }

        if (!_diamondBehavior.OnUsing)
            return;

        _diamondBehavior.OwnerAttackerBehavior.AttackDamage.Base.Change(_staticData.DamageBasePerLevel, name, "BOW_DIAMOND_UPGRADE");
        _diamondBehavior.OwnerAttackerBehavior.AttackSpeed.Base.Change(_staticData.FireRateBasePerLevel, name, "BOW_DIAMOND_UPGRADE");
        //Todo
        //_diamondBehavior.OwnerAttackerBehavior.CriticalChance.Base.Change(_staticData.CriticalChanceBasePerLevel, name, "BOW_DIAMOND_UPGRADE");
        //_diamondBehavior.OwnerAttackerBehavior.CriticalDamage.Base.Change(_staticData.CriticalDamageBasePerLevel, name, "BOW_DIAMOND_UPGRADE");
    }

    public string GetDescription()
    {
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentDamage = _diamondBehavior.OwnerAttackerBehavior.AttackDamage.IntValue;
        var nextDamage = (int)_diamondBehavior.OwnerAttackerBehavior.AttackDamage.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.DamageBasePerLevel);

        var currentDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentDamage, "black"));
        var nextDamageShow = NoteUtils.AddColor(nextDamage, upgradeColor);

        //------------------------------------------------

        var currentFireRate = _diamondBehavior.OwnerAttackerBehavior.AttackSpeed.Value;
        var nextFireRate = _diamondBehavior.OwnerAttackerBehavior.AttackSpeed.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.FireRateBasePerLevel);

        var currentFireRateShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentFireRate + "fps", "black"));
        var nextFireRateShow = NoteUtils.AddColor(nextFireRate + "fps", upgradeColor);

        //------------------------------------------------

        //Todo
        //var currentCriticalChance = _diamondBehavior.OwnerAttackerBehavior.CriticalChance.Value;
        //var nextCriticalChance = _diamondBehavior.OwnerAttackerBehavior.CriticalChance.Current
        //    .CalculateValueWithAdditions(baseAddition: _staticData.CriticalChanceBasePerLevel);

        //var currentCriticalChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalChance + "%", "black"));
        //var nextCriticalChanceShow = NoteUtils.AddColor(nextCriticalChance + "%", upgradeColor);

        var currentCriticalChanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(20 + "%", "black"));
        var nextCriticalChanceShow = NoteUtils.AddColor(20 + "%", upgradeColor);

        //------------------------------------------------

        //Todo
        //var currentCriticalDamage = _diamondBehavior.OwnerAttackerBehavior.CriticalDamage.Value;
        //var nextCriticalDamage = _diamondBehavior.OwnerAttackerBehavior.CriticalDamage.Current
        //    .CalculateValueWithAdditions(baseAddition: _staticData.CriticalDamageBasePerLevel);

        //var currentCriticalDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCriticalDamage + "%", "black"));
        //var nextCriticalDamageShow = NoteUtils.AddColor(nextCriticalDamage + "%", upgradeColor);

        var currentCriticalDamageShow = NoteUtils.MakeBold(NoteUtils.AddColor(200 + "%", "black"));
        var nextCriticalDamageShow = NoteUtils.AddColor(200 + "%", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"Bow Diamond can be in activation mode permanently. It do a lot of things for the archer:\n\n" +
            $" + It creates a magical bow\n" +
            $" + It creates a magical arrow as the archer commands\n" +
            $" + Magical arrows can carry other diamond effects\n" +
            $" + Magical bow has the following stats:\n" +
            $"   + Damage: {currentDamageShow} (next: {nextDamageShow})\n" +
            $"   + Fire Rate: {currentFireRateShow} (next: {nextFireRateShow})\n" +
            $"   + Critical Chance: {currentCriticalChanceShow} (next: {nextCriticalChanceShow})\n" +
            $"   + Critical Damage: {currentCriticalDamageShow} (next: {nextCriticalDamageShow})";
        return description;
    }
}
