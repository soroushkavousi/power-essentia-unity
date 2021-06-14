using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class NumberChangeCommand
{
    //[SerializeField] private string _id = default;
    [SerializeField] private NumberChangeCommandPart _part = default;
    [SerializeField] private float _amountValue = default;
    [SerializeField] private string _gameObjectName = default;
    [SerializeField] private string _type = default;
    [SerializeField] private string _description = default;
    [SerializeField] private string _scriptName = default;
    private ThreePartAdvancedNumber _amount = new ThreePartAdvancedNumber();
    private string _sourceFilePath = default;

    //public string Id => _id;
    public NumberChangeCommandPart Part => _part;
    public ThreePartAdvancedNumber Amount => _amount;
    public string Type => _type;
    public string SourceFilePath => _sourceFilePath;
    public string ScriptName => _scriptName;
    public string GameObjectName => _gameObjectName;
    public string Description => _description;
    public float AmountValue => _amountValue;

    public NumberChangeCommand(NumberChangeCommandPart part,
        float amount, string gameObjectName,
        string type = "", string description = "",
        [CallerFilePath] string sourceFilePath = "")
    {
        //_id = Guid.NewGuid().ToString("N");
        _part = part;
        if(_part != NumberChangeCommandPart.FEED_DATA)
            _amount.FeedData(amount.Round());
        _gameObjectName = gameObjectName;
        _type = type;
        _description = description;
        _sourceFilePath = sourceFilePath;
        _scriptName = Path.GetFileName(_sourceFilePath);
        _amount.OnNewValueActions.Add(UpdateAmountValue);
        UpdateAmountValue(null);
    }

    private void UpdateAmountValue(NumberChangeCommand changeCommand)
    {
        _amountValue = _amount.Value;
    }

    public NumberChangeCommand Copy() => new NumberChangeCommand(_part,
        _amount.Value, _gameObjectName, _type, _description, _sourceFilePath);
}