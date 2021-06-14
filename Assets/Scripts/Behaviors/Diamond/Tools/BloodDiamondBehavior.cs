using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(DiamondBehavior))]
public class BloodDiamondBehavior : MonoBehaviour
{
    [SerializeField] private BloodDiamondStaticData _staticData = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField, TextArea] private string _description = default;
    [SerializeField] private ThreePartAdvancedNumber _bloodPerDemonLevel = new ThreePartAdvancedNumber();
    private DiamondBehavior _diamondBehavior = default;
    private ResourceBox _gameResourceBox = default;

    public string Description => _description;

    public void Initialize()
    {
        _diamondBehavior = GetComponent<DiamondBehavior>();

        _diamondBehavior.FeedData(_staticData.DiamondStaticData, Activate, Deactivate);
        _diamondBehavior.Level.OnNewValueActions.Add(50, OnDiamondUpgraded);
        _diamondBehavior.GetDescription = GetDescription;
        _bloodPerDemonLevel.FeedData(_staticData.StartBloodPerDemonLevel);

        var diamondLevel = _diamondBehavior.Level.IntValue;
        var bloodPerDemonLevelBaseIncrement = diamondLevel * _staticData.BloodPerDemonLevelBasePerLevel;
        _bloodPerDemonLevel.Base.Change(bloodPerDemonLevelBaseIncrement, name, "LEVEL");

        _gameResourceBox = PlayerBehavior.Main.DynamicData.ResourceBox;
    }

    private void Activate()
    {
        if (SceneManagerBehavior.Instance.CurrentSceneName != SceneName.MISSION)
            return;

        WaveManagerBehavior.Instance.OnNewDeadInvaderActions.Add(AddBlood);
    }

    private void AddBlood(DemonBehavior demonBehavior)
    {
        var bloodIncrement = (int)(demonBehavior.Level.IntValue * _bloodPerDemonLevel.Value);
        _gameResourceBox.ResourceBunches[ResourceType.DEMON_BLOOD].Change(bloodIncrement, name, $"INVADER_DEAD");
        // Todo
        //if (Random.value * 100 <= 30)
        //{
            //_gameResourceBox.ResourceBunches[ResourceType.DARK_DEMON_BLOOD].Change(1, name, $"INVADER_DEAD");
        //}
    }

    private void Deactivate()
    {
        if (SceneManagerBehavior.Instance.CurrentSceneName != SceneName.MISSION)
            return;

        WaveManagerBehavior.Instance.OnNewDeadInvaderActions.Remove(AddBlood);
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

        _bloodPerDemonLevel.Base.Change(_staticData.BloodPerDemonLevelBasePerLevel, name, "BLOOD_DIAMOND_UPGRADE");
    }

    public string GetDescription()
    {
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentBloodPerDemonLevel = _bloodPerDemonLevel.IntValue;
        var nextBloodPerDemonLevel = (int)_bloodPerDemonLevel.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.BloodPerDemonLevelBasePerLevel);

        var currentBloodPerDemonLevelShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentBloodPerDemonLevel + " per demon level", "black"));
        var nextBloodPerDemonLevelShow = NoteUtils.AddColor(nextBloodPerDemonLevel + " per demon level", upgradeColor);

        //-----------------------------------------------

        //------------------------------------------------

        var description = $"" +
            $"Blood Diamond can be in activation mode permanently.\n\n" +
            $" + It can pulls demon bloods inside of itself and store them.\n" +
            $" + Stored blood will be share with other diamonds to activate them.\n" +
            $" + Stats:\n" +
            $"   + Blood: {currentBloodPerDemonLevelShow} (next: {nextBloodPerDemonLevelShow})";
        return description;
    }
}
