using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class SelectedItemsDynamicDataTO
{
    private SelectedItemsDynamicData _selectedItemsDynamicData = default;

    public int DemonLevel;
    public List<string> ToolsRingDiamondNames;
    public List<string> LeftRingDiamondNames;
    public List<string> RightRingDiamondNames;
    public string MenuDeckDiamondName;
    public string MenuToolsDiamondName;

    public SelectedItemsDynamicDataTO(int demonLevel, List<string> toolsRingDiamondNames,
        List<string> leftRingDiamondNames, List<string> rightRingDiamondNames, 
        string menuDeckDiamondName, string menuToolsDiamondName)
    {
        DemonLevel = demonLevel;
        ToolsRingDiamondNames = toolsRingDiamondNames;
        LeftRingDiamondNames = leftRingDiamondNames;
        RightRingDiamondNames = rightRingDiamondNames;
        MenuDeckDiamondName = menuDeckDiamondName;
        MenuToolsDiamondName = menuToolsDiamondName;
    }

    public SelectedItemsDynamicData GetSelectedItemsDynamicData()
    {
        var toolsRingDiamondNames = ToolsRingDiamondNames.Select(didn => didn.ToEnum<DiamondName>()).ToList();
        var leftRingDiamondNames = LeftRingDiamondNames.Select(didn => didn.ToEnum<DiamondName>()).ToList();
        var rightRingDiamondNames = RightRingDiamondNames.Select(didn => didn.ToEnum<DiamondName>()).ToList();
        var menuDeckDiamondName = MenuDeckDiamondName.ToEnum<DiamondName>();
        var menuToolsDiamondName = MenuToolsDiamondName.ToEnum<DiamondName>();
        
        _selectedItemsDynamicData = new SelectedItemsDynamicData(DemonLevel,
            toolsRingDiamondNames, leftRingDiamondNames, rightRingDiamondNames, 
            menuDeckDiamondName, menuToolsDiamondName);

        _selectedItemsDynamicData.DemonLevel.OnNewValueActions.Add(HandleDemonLevelChange);

        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.TOOLS][0].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.TOOLS, 0, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.TOOLS][1].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.TOOLS, 1, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.TOOLS][2].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.TOOLS, 2, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.TOOLS][3].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.TOOLS, 3, cc));

        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.LEFT][0].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.LEFT, 0, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.LEFT][1].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.LEFT, 1, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.LEFT][2].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.LEFT, 2, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.LEFT][3].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.LEFT, 3, cc));

        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.RIGHT][0].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.RIGHT, 0, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.RIGHT][1].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.RIGHT, 1, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.RIGHT][2].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.RIGHT, 2, cc));
        _selectedItemsDynamicData.RingDiamondNamesMap[RingName.RIGHT][3].OnNewValueActions.Add(
                (cc) => HandleDiamondNameChange(RingName.RIGHT, 3, cc));

        _selectedItemsDynamicData.MenuDiamondName[RingName.DECK].OnNewValueActions.Add(HandleMenuDeckDiamondNameChange);
        _selectedItemsDynamicData.MenuDiamondName[RingName.TOOLS].OnNewValueActions.Add(HandleMenuToolsDiamondNameChange);

        //for (int i = 0; i < _selectedItemsDynamicData.DeckItemDiamondNames.Count; i++)
        //{
        //    Debug.Log($"GetSelectedItemsDynamicData {i}");
        //    _selectedItemsDynamicData.DeckItemDiamondNames[i].OnNewValueActions.Add(
        //        (cc) => HandleDeckItemDiamondNameChange(i, cc));
        //}
        //_selectedItemsDynamicData.DeckItemDiamondNames[0].OnNewValueActions[0](null);
        return _selectedItemsDynamicData;
    }

    private void HandleDemonLevelChange(NumberChangeCommand changeCommand)
    {
        DemonLevel = _selectedItemsDynamicData.DemonLevel.IntValue;
        PlayerDynamicDataTO.Instance.Save();
    }

    private void HandleDiamondNameChange(RingName ringName, int index, StringChangeCommand changeCommand)
    {
        var newDiamondName = _selectedItemsDynamicData.RingDiamondNamesMap[ringName][index].Value;
        switch (ringName)
        {
            case RingName.TOOLS:
                ToolsRingDiamondNames[index] = newDiamondName;
                break;
            case RingName.LEFT:
                LeftRingDiamondNames[index] = newDiamondName;
                break;
            case RingName.RIGHT:
                RightRingDiamondNames[index] = newDiamondName;
                break;
        }
        PlayerDynamicDataTO.Instance.Save();
    }

    private void HandleMenuDeckDiamondNameChange(StringChangeCommand changeCommand)
    {
        MenuDeckDiamondName = _selectedItemsDynamicData.MenuDiamondName[RingName.DECK].Value;
        PlayerDynamicDataTO.Instance.Save();
    }

    private void HandleMenuToolsDiamondNameChange(StringChangeCommand changeCommand)
    {
        MenuToolsDiamondName = _selectedItemsDynamicData.MenuDiamondName[RingName.TOOLS].Value;
        PlayerDynamicDataTO.Instance.Save();
    }
}