using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class BooleanChangeCommand
{
    [SerializeField] private bool _value = default;
    [SerializeField] private string _gameObjectName = default;
    [SerializeField] private string _type = default;
    [SerializeField] private string _description = default;
    [SerializeField] private string _scriptName = default;

    public bool Value => _value;
    public string Type => _type;
    public string ScriptName => _scriptName;
    public string GameObjectName => _gameObjectName;
    public string Description => _description;

    public BooleanChangeCommand(bool value, string gameObjectName,
        string type = "", string description = "",
        [CallerFilePath] string sourceFilePath = "")
    {
        _value = value;
        _gameObjectName = gameObjectName;
        _type = type;
        _description = description;
        _scriptName = Path.GetFileName(sourceFilePath);
    }
}