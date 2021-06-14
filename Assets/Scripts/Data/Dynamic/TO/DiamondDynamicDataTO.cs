using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class DiamondDynamicDataTO
{
    private DiamondDynamicData _diamondDynamicData = default;

    public string Name;
    public bool IsDiscovered;
    public bool IsOwned;
    public int Level;

    public DiamondDynamicDataTO(string name, bool isDiscovered,
        bool isOwned, int level)
    {
        Name = name;
        IsDiscovered = isDiscovered;
        IsOwned = isOwned;
        Level = level;
    }

    public DiamondDynamicData GetDiamondDynamicData()
    {
        var diamondName = Name.ToEnum<DiamondName>();
        _diamondDynamicData = new DiamondDynamicData(diamondName,
            IsDiscovered, IsOwned, Level);
        _diamondDynamicData.IsDiscovered.OnNewValueActions.Add(OnIsDiscoveredChanged);
        _diamondDynamicData.IsOwned.OnNewValueActions.Add(OnIsOwnedChanged);
        _diamondDynamicData.Level.OnNewValueActions.Add(OnLevelChanged);
        return _diamondDynamicData;
    }

    private void OnIsDiscoveredChanged(BooleanChangeCommand changeCommand)
    {
        IsDiscovered = _diamondDynamicData.IsDiscovered.Value;
        PlayerDynamicDataTO.Instance.Save();
    }

    private void OnIsOwnedChanged(BooleanChangeCommand changeCommand)
    {
        IsOwned = _diamondDynamicData.IsOwned.Value;
        PlayerDynamicDataTO.Instance.Save();
    }

    private void OnLevelChanged(NumberChangeCommand changeCommand)
    {
        Level = _diamondDynamicData.Level.IntValue;
        PlayerDynamicDataTO.Instance.Save();
    }
}
