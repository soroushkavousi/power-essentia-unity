﻿using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class PlayerDynamicDataTO : IObserver
{
    private static readonly string _relativeDataPath = @"player.json";
    private static readonly int _automaticSavePeriod = 500;
    private PlayerDynamicData _playerDynamicData = default;
    private bool _shouldSave = default;
    public static PlayerDynamicDataTO Instance { get; private set; }

    public long PatchNumber;
    public string PlayerSetName;
    public int DemonLevel;
    public SelectedItemsDynamicDataTO SelectedItems;
    public List<DiamondDynamicDataTO> Diamonds;
    public List<ResourceBunchDynamicDataTO> ResourceBunches;
    public PlayerDynamicData PlayerDynamicData => _playerDynamicData;

    static PlayerDynamicDataTO()
    {
        Load();
        //#For_Test
        TestPlayer.ApplyData(Instance);
        Instance.Save();
        Task.Run(() => Instance.StartAutomaticSaving());
        Instance._playerDynamicData = Instance.GetPlayerDynamicData();
        Debug.Log(Application.persistentDataPath + "/data");
    }

    public PlayerDynamicDataTO() { }

    public static void Load()
    {
        var dataExists = StorageGateway.DoesDataExist(_relativeDataPath);
        if (dataExists == false)
        {
            Instance = PlayerStart.Data;
            Instance.Save();
            return;
        }
        Instance = StorageGateway.ReadData<PlayerDynamicDataTO>(_relativeDataPath);
        if (Instance.PatchNumber < PlayerStart.Data.PatchNumber)
        {
            StorageGateway.DeleteFile(_relativeDataPath);
            Load();
        }
    }

    public void Save()
    {
        _shouldSave = true;
    }

    private async Task StartAutomaticSaving()
    {
        while (true)
        {
            await Task.Delay(_automaticSavePeriod);
            if (_shouldSave == false)
                continue;
            _shouldSave = false;
            StorageGateway.WriteData(_relativeDataPath, Instance);
        }
    }

    public void ReturnToDefault()
    {
        //Instance.MasterVolume = Default.MasterVolume;
        //Instance.Difficulty = Default.Difficulty;
    }

    private PlayerDynamicData GetPlayerDynamicData()
    {
        _playerDynamicData = new PlayerDynamicData(
            PatchNumber, PlayerSetName.ToEnum<PlayerSetName>(), DemonLevel,
            SelectedItems.GetSelectedItemsDynamicData(),
            Diamonds.Select(cd => cd.GetDiamondDynamicData()).ToList(),
            ResourceBunches.Select(rb => rb.GetResourceBunch()).ToList()
            );

        _playerDynamicData.PlayerSetName.Attach(this);
        _playerDynamicData.DemonLevel.Attach(this);
        return _playerDynamicData;
    }

    private void OnPlayerSetNameChanged()
    {
        PlayerSetName = _playerDynamicData.PlayerSetName.Value.ToString();
        Save();
    }

    private void OnDemonLevelChanged()
    {
        DemonLevel = _playerDynamicData.DemonLevel.Value;
        Save();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _playerDynamicData.PlayerSetName)
            OnPlayerSetNameChanged();
        else if (subject == _playerDynamicData.DemonLevel)
            OnDemonLevelChanged();
    }
}
