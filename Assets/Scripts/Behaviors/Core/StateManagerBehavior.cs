using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StateManagerBehavior : MonoBehaviour
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private Observable<string> _state = new();
    private List<string> _states = default;
    private Action _checkStateAction = default;
    private Action _stopOldStateAction = default;
    private Action _startNewStateAction = default;
    private string _nextState = default;
    private bool _isGoingToNextState = default;
    private Animator _animator = default;

    public Observable<string> State => _state;
    public OrderedList<Action> OnStartNewStateActions { get; } = new OrderedList<Action>();

    public void FeedData<T>(Type stateEnumType, T startState,
        Action checkStateAction, Action stopOldStateAction,
        Action startNewStateAction)
    {
        _animator = GetComponent<Animator>();

        _states = Enum.GetNames(stateEnumType).ToList();
        _state.Value = startState.ToString();
        _checkStateAction = checkStateAction;
        _stopOldStateAction = stopOldStateAction;
        _startNewStateAction = startNewStateAction;
    }


    private void Update()
    {
        if (!_isGoingToNextState)
            _checkStateAction?.Invoke();
    }

    //This function will be called in the animations
    private void StartNewState(string state)
    {
        if (_state.Value == state)
            return;

        _isGoingToNextState = true;
        _state.Value = state;
        _nextState = string.Empty;
        _stopOldStateAction?.Invoke();
        _startNewStateAction?.Invoke();
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