using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StateManagerBehavior : MonoBehaviour
{
    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private AdvancedString _state = new AdvancedString();
    private List<string> _states = default;
    private Action _checkStateAction = default;
    private Action _stopOldStateAction = default;
    private Action _startNewStateAction = default;
    private string _nextState = default;
    private bool _isGoingToNextState = default;
    private bool _isHandlingAIEvent = default;
    private Animator _animator = default;

    public AdvancedString State => _state;
    public OrderedList<Action> OnStartNewStateActions { get; } = new OrderedList<Action>();

    public void FeedData<T>(Type stateEnumType, T startState,
        Action checkStateAction, Action stopOldStateAction, 
        Action startNewStateAction)
    {
        _animator = GetComponent<Animator>();

        _states = Enum.GetNames(stateEnumType).ToList();
        _state.FeedData(startState);
        _checkStateAction = checkStateAction;
        _stopOldStateAction = stopOldStateAction;
        _startNewStateAction = startNewStateAction;
    }


    private void Update()
    {
        if(!_isGoingToNextState)
            _checkStateAction();
    }

    private void StartNewState(string state)
    {
        if (_state.Value == state)
            return;

        _isGoingToNextState = true;
        _state.Change(state, name);
        _nextState = string.Empty;
        _stopOldStateAction();
        _startNewStateAction();
        _isGoingToNextState = false;
    }

    public void GoToTheNextState(Enum nextState)
    {
        if (_nextState == nextState.ToString())
            return;
        _nextState = nextState.ToString();
        foreach (var state in _states)
        {
            if (state == "NOT_DEFINED")
                continue;
            _animator.ResetTrigger(state);
        }
        _animator.SetTrigger(_nextState);
    }
}