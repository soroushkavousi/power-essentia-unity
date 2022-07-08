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
    public List<string> BaseRingDiamondNames;
    public List<string> LeftRingDiamondNames;
    public List<string> RightRingDiamondNames;
    public string MenuDeckDiamondName;
    public string MenuBaseDiamondName;

    public SelectedItemsDynamicDataTO(int demonLevel, List<string> baseRingDiamondNames,
        List<string> leftRingDiamondNames, List<string> rightRingDiamondNames,
        string menuDeckDiamondName, string menuBaseDiamondName)
    {
        DemonLevel = demonLevel;
        BaseRingDiamondNames = baseRingDiamondNames;
        LeftRingDiamondNames = leftRingDiamondNames;
        RightRingDiamondNames = rightRingDiamondNames;
        MenuDeckDiamondName = menuDeckDiamondName;
        MenuBaseDiamondName = menuBaseDiamondName;
    }

    public SelectedItemsDynamicData GetSelectedItemsDynamicData()
    {
        var baseRingDiamondNames = BaseRingDiamondNames.Select(diamondName => diamondName.ToEnum<DiamondName>()).ToList();
        var leftRingDiamondNames = LeftRingDiamondNames.Select(diamondName => diamondName.ToEnum<DiamondName>()).ToList();
        var rightRingDiamondNames = RightRingDiamondNames.Select(diamondName => diamondName.ToEnum<DiamondName>()).ToList();
        var menuDeckDiamondName = MenuDeckDiamondName.ToEnum<DiamondName>();
        var menuBaseDiamondName = MenuBaseDiamondName.ToEnum<DiamondName>();

        _selectedItemsDynamicData = new SelectedItemsDynamicData(DemonLevel,
            baseRingDiamondNames, leftRingDiamondNames, rightRingDiamondNames,
            menuDeckDiamondName, menuBaseDiamondName);

        _selectedItemsDynamicData.DemonLevel.Attach(this);

        foreach (var currentRingName in new List<RingName> { RingName.BASE, RingName.LEFT, RingName.RIGHT })
        {
            for (int i = 0; i < _selectedItemsDynamicData.RingDiamondNamesMap[currentRingName].Count; i++)
            {
                _selectedItemsDynamicData.RingDiamondNamesMap[currentRingName][i].Attach(this);
            }
        }

        _selectedItemsDynamicData.MenuDiamondName[RingName.DECK].Attach(this);
        _selectedItemsDynamicData.MenuDiamondName[RingName.BASE].Attach(this);
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
            case RingName.BASE:
                BaseRingDiamondNames[index] = newDiamondName.ToString();
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

    private void HandleMenuBaseDiamondNameChange()
    {
        MenuBaseDiamondName = _selectedItemsDynamicData.MenuDiamondName[RingName.BASE].Value.ToString();
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
            else if (subject == _selectedItemsDynamicData.MenuDiamondName[RingName.BASE])
                HandleMenuBaseDiamondNameChange();
            else
            {
                foreach (var currentRingName in new List<RingName> { RingName.BASE, RingName.LEFT, RingName.RIGHT })
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