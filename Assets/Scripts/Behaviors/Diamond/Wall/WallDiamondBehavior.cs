using UnityEngine;

public class WallDiamondBehavior : DiamondBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(WallDiamondBehavior) + Constants.HeaderEnd)]

    [SerializeField] private WallDiamondStaticData _staticData = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    private WallBehavior _wallBehavior = default;

    public WallBehavior WallBehavior => _wallBehavior;

    public override void Initialize(Observable<DiamondKnowledgeState> state, Observable<int> level,
        DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.FeedData(_staticData);
        base.Initialize(state, level, diamondOwnerBehavior);
    }

    protected override void DoActivationWork()
    {
        Transform wallLocation;
        if (SceneManagerBehavior.Instance.CurrentSceneName == SceneName.MISSION)
            wallLocation = MissionManagerBehavior.Instance.WallLocation;
        else
            wallLocation = OutBoxBehavior.Instance.Location3;

        _wallBehavior = Instantiate(_staticData.WallBehaviorPrefab, wallLocation);
        _wallBehavior.name = "Wall";
        _wallBehavior.Initialize(_level);
    }

    protected override void DoDeactivationWork()
    {
        Destroy(_wallBehavior);
    }

    protected override string GetDescription()
    {
        var description = $"" +
            $"On Activation it creates a magical wall.\n" +
            $"Wall diamond is a base diamond that is active permanently.";
        return description;
    }

    protected override string GetStatsDescription()
    {
        //------------------------------------------------

        var currentHealth = _wallBehavior.HealthBehavior.Health.Value.ToLong();
        var nextHealth = _wallBehavior.HealthBehavior.Health.NextLevelValue.ToLong();

        var currentHealthShow = NoteUtils.AddColor(currentHealth, "black");
        currentHealthShow = NoteUtils.ChangeSize($"Health: {currentHealthShow}", NoteUtils.NumberSizeRatio);
        var nextHealthShow = NoteUtils.AddColor(nextHealth, NoteUtils.UpgradeColor);
        nextHealthShow = NoteUtils.ChangeSize($"({nextHealthShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentPhysicalResistance = _wallBehavior.HealthBehavior.Health.PhysicalResistance.Value.ToLong();
        var nextPhysicalResistance = _wallBehavior.HealthBehavior.Health.PhysicalResistance.NextLevelValue.ToLong();

        var currentPhysicalResistanceShow = NoteUtils.AddColor(currentPhysicalResistance + "%", "black");
        currentPhysicalResistanceShow = NoteUtils.ChangeSize($"Physical Resistance: {currentPhysicalResistanceShow}", NoteUtils.NumberSizeRatio);
        var nextPhysicalResistanceShow = NoteUtils.AddColor(nextPhysicalResistance + "%", NoteUtils.UpgradeColor);
        nextPhysicalResistanceShow = NoteUtils.ChangeSize($"({nextPhysicalResistanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var currentMagicResistance = _wallBehavior.HealthBehavior.Health.MagicResistance.Value.ToLong();
        var nextMagicResistance = _wallBehavior.HealthBehavior.Health.MagicResistance.NextLevelValue.ToLong();

        var currentMagicResistanceShow = NoteUtils.AddColor(currentMagicResistance + "%", "black");
        currentMagicResistanceShow = NoteUtils.ChangeSize($"Magic Resistance: {currentMagicResistanceShow}", NoteUtils.NumberSizeRatio);
        var nextMagicResistanceShow = NoteUtils.AddColor(nextMagicResistance + "%", NoteUtils.UpgradeColor);
        nextMagicResistanceShow = NoteUtils.ChangeSize($"({nextMagicResistanceShow})", NoteUtils.NextNumberSizeRatio);

        //------------------------------------------------

        var statsDescription = $"" +
            $"{currentHealthShow}    {nextHealthShow}\n" +
            $"{currentPhysicalResistanceShow}    {nextPhysicalResistanceShow}\n" +
            $"{currentMagicResistanceShow}    {nextMagicResistanceShow}\n";
        return statsDescription;
    }
}
