using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(DiamondBehavior))]
public class WallDiamondBehavior : MonoBehaviour
{
    [SerializeField] private WallDiamondStaticData _staticData = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField, TextArea] private string _description = default;
    private WallBehavior _wallBehavior = default;
    private HealthBehavior _wallHealthBehavior = default;
    private DiamondBehavior _diamondBehavior = default;

    public string Description => _description;

    public void Initialize()
    {
        _diamondBehavior = GetComponent<DiamondBehavior>();

        _diamondBehavior.FeedData(_staticData.DiamondStaticData, Activate, Deactivate);
        _diamondBehavior.Level.OnNewValueActions.Add(50, OnDiamondUpgraded);
        _diamondBehavior.GetDescription = GetDescription;
    }

    private void InitializeWall()
    {
        Transform parentGameObject;
        if(SceneManagerBehavior.Instance.CurrentSceneName == SceneName.MISSION)
            parentGameObject = MissionManagerBehavior.Instance.BattleField;
        else
            parentGameObject = OutBoxBehavior.Instance.Location1;

        _wallBehavior = Instantiate(_staticData.WallBehaviorPrefab, parentGameObject);
        _wallBehavior.GetComponent<RectTransform>().anchoredPosition = _staticData.LocationInBattleField;
        _wallBehavior.name = "Wall";
        _wallBehavior.Initialize();
        ApplyLevelToWall();
    }

    private void ApplyLevelToWall()
    {
        var diamondLevel = _diamondBehavior.Level.IntValue;

        _wallHealthBehavior = _wallBehavior.GetComponent<HealthBehavior>();

        var healthBaseIncrement = diamondLevel * _staticData.HealthBasePerLevel;
        _wallHealthBehavior.Health.Base.Change(healthBaseIncrement, name, "LEVEL");

        var physicalResistanceBaseIncrement = diamondLevel * _staticData.PhysicalResistanceBasePerLevel;
        _wallHealthBehavior.ResistanceBehavior.PhysicalResistance.Base.Change(physicalResistanceBaseIncrement, name, "LEVEL");
    }

    private void Activate()
    {
        InitializeWall();
    }

    private void Deactivate()
    {
        Destroy(_wallBehavior);
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

        _wallHealthBehavior.Health.Base.Change(_staticData.HealthBasePerLevel, name, "WALL_DIAMOND_UPGRADE");
        _wallHealthBehavior.ResistanceBehavior.PhysicalResistance.Base.Change(_staticData.PhysicalResistanceBasePerLevel, name, "WALL_DIAMOND_UPGRADE");
    }

    public string GetDescription()
    {
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentHealth = _wallHealthBehavior.Health.IntValue;
        var nextHealth = (int)_wallHealthBehavior.Health.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.HealthBasePerLevel);

        var currentHealthShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentHealth, "black"));
        var nextHealthShow = NoteUtils.AddColor(nextHealth, upgradeColor);

        //------------------------------------------------

        var currentPhysicalResistance = _wallHealthBehavior.ResistanceBehavior.PhysicalResistance.IntValue;
        var nextPhysicalResistance = (int)_wallHealthBehavior.ResistanceBehavior.PhysicalResistance.Current
            .CalculateValueWithAdditions(baseAddition: _staticData.PhysicalResistanceBasePerLevel);

        var currentPhysicalResistanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentPhysicalResistance + "%", "black"));
        var nextPhysicalResistanceShow = NoteUtils.AddColor(nextPhysicalResistance + "%", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"Wall Diamond can be in activation mode permanently.\n\n" +
            $" + On Activation it creates a magical wall\n" +
            $" + Magical wall has the following stats:\n" +
            $"   + Health: {currentHealthShow} (next: {nextHealthShow})\n" +
            $"   + Physical Resistance: {currentPhysicalResistanceShow} (next: {nextPhysicalResistanceShow})";
        return description;
    }
}
