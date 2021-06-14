using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellBehavior : MonoBehaviour
{
    [SerializeField] private string _name = default;
    [SerializeField] private float _cooldown = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private SpellState _state = default;

    private Action _castAction = default;

    public string Name => _name;
    public float Cooldown => _cooldown;
    public SpellState State => _state;

    public void FeedData(Action castAction)
    {
        _state = SpellState.UNDER_COOLDOWN;
        _castAction = castAction;
        StartCoroutine(GoOnCooldown());
    }

    private void Update()
    {

    }

    public void Cast()
    {
        if (_state != SpellState.READY)
            return;
        _state = SpellState.CASTING;
        _castAction();
        StartCoroutine(GoOnCooldown());
    }

    private IEnumerator GoOnCooldown()
    {
        _state = SpellState.UNDER_COOLDOWN;
        yield return new WaitForSeconds(_cooldown);
        _state = SpellState.READY;
    }
}