using System.Collections;
using UnityEngine;

public abstract class SpellBehavior : MonoBehaviour
{
    [Header(Constants.HeaderStart + nameof(SpellBehavior) + Constants.HeaderEnd)]

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] protected SpellState _state = default;
    [SerializeField] private Number _cooldown = default;
    [SerializeField] protected Observable<int> _level = default;
    private SpellStaticData _spellStaticData = default;

    public string Name => _spellStaticData.Name;
    public Number Cooldown => _cooldown;
    public SpellState State => _state;

    protected void Initialize(SpellStaticData spellStaticData, Observable<int> level)
    {
        _spellStaticData = spellStaticData;
        _level = level;
        _state = SpellState.UNDER_COOLDOWN;
        _cooldown = new(_spellStaticData.Cooldown.Randomize(), _level, _spellStaticData.CooldownLevelPercentage);
    }

    protected abstract void CastAction();

    public void Cast()
    {
        if (_state != SpellState.READY)
            return;
        _state = SpellState.CASTING;
        CastAction();
        StartCoroutine(GoOnCooldown());
    }

    protected IEnumerator GoOnCooldown()
    {
        _state = SpellState.UNDER_COOLDOWN;
        yield return new WaitForSeconds(_cooldown.Value);
        _state = SpellState.READY;
    }
}