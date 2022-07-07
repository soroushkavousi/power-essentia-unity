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

    private void Awake()
    {
        base.FeedData(_staticData);
    }

    public override void Initialize(Observable<DiamondKnowledgeState> state, Observable<int> level,
        DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.Initialize(state, level, diamondOwnerBehavior);

    }

    protected override void DoActivationWork()
    {
        if (SceneManagerBehavior.Instance.CurrentSceneName != SceneName.MISSION)
            return;

        _wallBehavior = Instantiate(_staticData.WallBehaviorPrefab, MissionManagerBehavior.Instance.WallLocation);
        _wallBehavior.name = "Wall";
        _wallBehavior.Initialize(_level);
    }

    protected override void DoDeactivationWork()
    {
        Destroy(_wallBehavior);
    }

    protected override string GetDescription()
    {
        var upgradeColor = "#55540E";
        //------------------------------------------------

        var currentHealth = _wallBehavior.HealthBehavior.Health.Value.ToLong();
        var nextHealth = _wallBehavior.HealthBehavior.Health.NextLevelValue.ToLong();

        var currentHealthShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentHealth, "black"));
        var nextHealthShow = NoteUtils.AddColor(nextHealth, upgradeColor);

        //------------------------------------------------

        var currentPhysicalResistance = _wallBehavior.HealthBehavior.Health.PhysicalResistance.Value;
        var nextPhysicalResistance = _wallBehavior.HealthBehavior.Health.PhysicalResistance.NextLevelValue;

        var currentPhysicalResistanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentPhysicalResistance + "%", "black"));
        var nextPhysicalResistanceShow = NoteUtils.AddColor(nextPhysicalResistance + "%", upgradeColor);

        //------------------------------------------------

        var currentMagicResistance = _wallBehavior.HealthBehavior.Health.MagicResistance.Value;
        var nextMagicResistance = _wallBehavior.HealthBehavior.Health.MagicResistance.NextLevelValue;

        var currentMagicResistanceShow = NoteUtils.MakeBold(NoteUtils.AddColor(currentMagicResistance + "%", "black"));
        var nextMagicResistanceShow = NoteUtils.AddColor(nextMagicResistance + "%", upgradeColor);

        //------------------------------------------------

        var description = $"" +
            $"Wall Diamond can be in activation mode permanently.\n\n" +
            $" + On Activation it creates a magical wall\n" +
            $" + Magical wall has the following stats:\n" +
            $"   + Health: {currentHealthShow} (next: {nextHealthShow})\n" +
            $"   + Physical Resistance: {currentPhysicalResistanceShow} (next: {nextPhysicalResistanceShow})" +
            $"   + Magic Resistance: {currentMagicResistanceShow} (next: {nextMagicResistanceShow})";
        return description;
    }
}
