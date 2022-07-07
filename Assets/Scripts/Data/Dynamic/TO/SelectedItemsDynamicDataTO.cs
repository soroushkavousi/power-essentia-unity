using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class SelectedItemsDynamicDataTO : IObserver
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

        _selectedItemsDynamicData.DemonLevel.Attach(this);

        foreach (var currentRingName in new List<RingName> { RingName.TOOLS, RingName.LEFT, RingName.RIGHT })
        {
            for (int i = 0; i < _selectedItemsDynamicData.RingDiamondNamesMap[currentRingName].Count; i++)
            {
                _selectedItemsDynamicData.RingDiamondNamesMap[currentRingName][i].Attach(this);
            }
        }

        _selectedItemsDynamicData.MenuDiamondName[RingName.DECK].Attach(this);
        _selectedItemsDynamicData.MenuDiamondName[RingName.TOOLS].Attach(this);
        return _selectedItemsDynamicData;
    }

    private void HandleDemonLevelChange()
    {
        DemonLevel = _selectedItemsDynamicData.DemonLevel.Value;
        PlayerDynamicDataTO.Instance.Save();
    }

    private void HandleDiamondNameChange(RingName ringName, int index)
    {
        var newDiamondName = _selectedItemsDynamicData.RingDiamondNamesMap[ringName][index].Value;
        switch (ringName)
        {
            case RingName.TOOLS:
                ToolsRingDiamondNames[index] = newDiamondName.ToString();
                break;
            case RingName.LEFT:
                LeftRingDiamondNames[index] = newDiamondName.ToString();
                break;
            case RingName.RIGHT:
                RightRingDiamondNames[index] = newDiamondName.ToString();
                break;
        }
        PlayerDynamicDataTO.Instance.Save();
    }

    private void HandleMenuDeckDiamondNameChange()
    {
        MenuDeckDiamondName = _selectedItemsDynamicData.MenuDiamondName[RingName.DECK].Value.ToString();
        PlayerDynamicDataTO.Instance.Save();
    }

    private void HandleMenuToolsDiamondNameChange()
    {
        MenuToolsDiamondName = _selectedItemsDynamicData.MenuDiamondName[RingName.TOOLS].Value.ToString();
        PlayerDynamicDataTO.Instance.Save();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _selectedItemsDynamicData.DemonLevel)
        {
            HandleDemonLevelChange();
        }
        else if (subject is Observable<DiamondName>)
        {
            if (subject == _selectedItemsDynamicData.MenuDiamondName[RingName.DECK])
                HandleMenuDeckDiamondNameChange();
            else if (subject == _selectedItemsDynamicData.MenuDiamondName[RingName.TOOLS])
                HandleMenuToolsDiamondNameChange();
            else
            {
                foreach (var currentRingName in new List<RingName> { RingName.TOOLS, RingName.LEFT, RingName.RIGHT })
                {
                    for (int i = 0; i < _selectedItemsDynamicData.RingDiamondNamesMap[currentRingName].Count; i++)
                    {
                        if (subject == _selectedItemsDynamicData.RingDiamondNamesMap[currentRingName][i])
                        {
                            HandleDiamondNameChange(currentRingName, i);
                            return;
                        }
                    }
                }
            }
        }
    }
}