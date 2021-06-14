//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//[Serializable]
//public class ChangeCommandsProcessor
//{
//    [SerializeField] private List<NumberChangeCommand> _changeCommands = default;
//    [SerializeField] private ObservableCollection<NumberChangeCommand> _changeCommandsObservableCollection = new ObservableCollection<NumberChangeCommand>();
//    [SerializeField] private float _resultAmount = 0;
//    [SerializeField] private int _lastResultIndex = -1;

//    public ObservableCollection<NumberChangeCommand> ChangeCommands => _changeCommandsObservableCollection;
//    public float ResultAmount => _resultAmount;
//    public SortedList<int, Action<NumberChangeCommand>> OnHandleAddedChangeCommandBeforeApplyActions { get; } = new SortedList<int, Action<NumberChangeCommand>>();
//    public SortedList<int, Action<NumberChangeCommand>> OnHandleAddedChangeCommandAfterApplyActions { get; } = new SortedList<int, Action<NumberChangeCommand>>();
//    public SortedList<int, Action<NumberChangeCommand>> OnHandleRemovedChangeCommandBeforeApplyActions { get; } = new SortedList<int, Action<NumberChangeCommand>>();
//    public SortedList<int, Action<NumberChangeCommand>> OnHandleRemovedChangeCommandAfterApplyActions { get; } = new SortedList<int, Action<NumberChangeCommand>>();

//    public ChangeCommandsProcessor()
//    {
//        _changeCommandsObservableCollection.CollectionChanged += HandleChangeCommandsChange;
//    }

//    private void HandleChangeCommandsChange(object sender, NotifyCollectionChangedEventArgs args)
//    {
//        _changeCommands = _changeCommandsObservableCollection.ToList();
//        if (args.Action == NotifyCollectionChangedAction.Add)
//        {
//            var addedChangeCommand = (NumberChangeCommand)args.NewItems[0];
//            HandleAddedChangeCommand(args.NewStartingIndex, addedChangeCommand);
//        }
//        else if(args.Action == NotifyCollectionChangedAction.Remove)
//        {
//            var removedChangeCommand = (NumberChangeCommand)args.OldItems[0];
//            HandleRemovedChangeCommand(args.OldStartingIndex, removedChangeCommand);
//        }
//    }

//    private void HandleAddedChangeCommand(int addedIndex, NumberChangeCommand addedChangeCommand)
//    {
//        OnHandleAddedChangeCommandBeforeApplyActions.Values.ToList().CallActionsSafely(addedChangeCommand));
//        _lastResultIndex = addedIndex - 1;
//        CalculateResultAmount();
//        OnHandleAddedChangeCommandAfterApplyActions.Values.ToList().CallActionsSafely(addedChangeCommand));
//    }

//    private void HandleRemovedChangeCommand(int removedIndex, NumberChangeCommand removedChangeCommand)
//    {
//        OnHandleRemovedChangeCommandBeforeApplyActions.Values.ToList().CallActionsSafely(removedChangeCommand));
//        _lastResultIndex = removedIndex - 1;
//        CalculateResultAmount();
//        OnHandleRemovedChangeCommandAfterApplyActions.Values.ToList().CallActionsSafely(removedChangeCommand));
//    }

//    private void CalculateResultAmount()
//    {
//        if (_lastResultIndex == -1)
//        {
//            _changeCommandsObservableCollection[0].Result = _changeCommandsObservableCollection[0].Amount.CurrentValue.Result;
//            _lastResultIndex++;
//        }

//        while (_lastResultIndex < _changeCommandsObservableCollection.Count - 1)
//        {
//            _changeCommandsObservableCollection[_lastResultIndex + 1].Result =
//                _changeCommandsObservableCollection[_lastResultIndex].Result
//                + _changeCommandsObservableCollection[_lastResultIndex + 1].Amount.CurrentValue.Result;
//            _lastResultIndex++;
//        }

//        _resultAmount = _changeCommandsObservableCollection[_lastResultIndex].Result;
//    }
//}