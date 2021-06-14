using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(DiamondBehavior))]
public class ArcherDiamondBehavior : MonoBehaviour
{
    [SerializeField] private ArcherDiamondStaticData _staticData = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField, TextArea] private string _description = default;
    private DiamondBehavior _diamondBehavior = default;
    [SerializeField] private ThreePartAdvancedNumber _interval = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private ThreePartAdvancedNumber _diamondCount = new ThreePartAdvancedNumber(currentDummyMin: 0f);
    [SerializeField] private ThreePartAdvancedNumber _cooldownReduction = new ThreePartAdvancedNumber(currentDummyMin: 0f);

    public string Description => _description;
    public ThreePartAdvancedNumber Interval => _interval;
    public ThreePartAdvancedNumber DiamondCount => _diamondCount;
    public ThreePartAdvancedNumber CooldownReduction => _cooldownReduction;

    public void Initialize()
    {
        _diamondBehavior = GetComponent<DiamondBehavior>();

        var diamondLevel = _diamondBehavior.Level.IntValue;

        _interval.FeedData(_staticData.StartInterval);
        var intervalNegativeBaseIncrement = diamondLevel * _staticData.IntervalNegativeBasePerLevel;
        _diamondCount.Base.Change(-intervalNegativeBaseIncrement, name, "ARCHER_DIAMOND");

        _diamondCount.FeedData(_staticData.StartDiamondCount);
        var diamondCountBaseIncrement = diamondLevel * _staticData.DiamondCountBasePerlevel;
        _diamondCount.Base.Change(diamondCountBaseIncrement, name, "ARCHER_DIAMOND");

        _cooldownReduction.FeedData(_staticData.StartCooldownReduction);
        var cooldownReductionBaseIncrement = diamondLevel * _staticData.CooldownReductionBasePerLevel;
        _cooldownReduction.Base.Change(cooldownReductionBaseIncrement, name, "ARCHER_DIAMOND");

        _diamondBehavior.FeedData(_staticData.DiamondStaticData, Activate, Deactivate);
        _diamondBehavior.Level.OnNewValueActions.Add(50, OnDiamondUpgraded);
        _diamondBehavior.GetDescription = GetDescription;
    }

    private void Activate()
    {
        /* Todo
         * Every (30 -> 10) seconds it refreshes a random deck diamond which is on cooldown by x seconds.
         * Decrease the remaining time of the diamond
         */
    }

    private void Deactivate()
    {

    }

    private void DoInEveryPeriod()
    {
        if (UnityEngine.Random.value * 100 > _cooldownReduction.Value)
            return;
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

        _interval.Base.Change(-_staticData.IntervalNegativeBasePerLevel, name, "UPGRADE_LEVEL");
        _diamondCount.Base.Change(_staticData.DiamondCountBasePerlevel, name, "UPGRADE_LEVEL");
        _cooldownReduction.Base.Change(_staticData.CooldownReductionBasePerLevel, name, "UPGRADE_LEVEL");
    }

    public string GetDescription()
    {
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentInterval = _interval.Value;
        var nextInterval = _interval.Current
            .CalculateValueWithAdditions(baseAddition: -_staticData.IntervalNegativeBasePerLevel);

        var currentIntervalShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentInterval + "s", "black"));
        var nextIntervalShow = NoteUtils.AddColor(nextInterval + "s", upgradeColor);

        //------------------------------------------------

        var currentDiamondCount = _diamondCount.IntValue;
        var nextDiamondCount = Mathf.FloorToInt(_diamondCount.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.DiamondCountBasePerlevel));

        var currentDiamondCountShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentDiamondCount + " diamond", "black"));
        var nextDiamondCountShow = NoteUtils.AddColor(nextDiamondCount + " diamond", upgradeColor);

        //------------------------------------------------

        var currentCooldownReduction = _cooldownReduction.Value;
        var nextCooldownReduction = _cooldownReduction.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.CooldownReductionBasePerLevel);

        var currentCooldownReductionShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentCooldownReduction + "s", "black"));
        var nextCooldownReductionShow = NoteUtils.AddColor(nextCooldownReduction + "s", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"Archer Diamond exists in archer blood and it's permanently active.\n" +
            $"It do some things for the archer:\n\n" +
            $" + It gives the archer an ability to control the other diamonds.\n" +
            $" + It intervally decrease the cooldown of the used diamonds:\n" +
            $"   + Interval: {currentIntervalShow} (next: {nextIntervalShow})\n" +
            $"   + Effected Diamonds Count: {currentDiamondCountShow} (next: {nextDiamondCount})\n" +
            $"   + Cooldown Reduction: {currentCooldownReductionShow} (next: {nextCooldownReductionShow})";

        return description;
    }
}
