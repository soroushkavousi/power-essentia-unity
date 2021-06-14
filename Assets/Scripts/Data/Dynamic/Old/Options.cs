using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class Options
{
    private static readonly string _relativeDataPath = @"options.json";

    public static Options Instance { get; private set; }
    public static Options Default => new Options
    {
        MasterVolume = 0.5f,
        Difficulty = 2,
    };

    [SerializeField]
    private float _masterVolume;
    [SerializeField]
    private int _difficulty;

    public float MasterVolume 
    {
        get => _masterVolume;
        set
        {
            _masterVolume = value;
            OnMasterVolumeChangeActions.CallActionsSafely();
        }
    }
    public int Difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            OnDifficultyChangedActions.CallActionsSafely();
        }
    }

    public OrderedList<Action> OnMasterVolumeChangeActions { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnDifficultyChangedActions { get; } = new OrderedList<Action>();

    static Options()
    {
        Instance = Default;
        Instance.ReadFromStorage();
    }

    private Options() { }

    public void ReadFromStorage()
    {
        var dataExists = StorageGateway.DoesDataExist(_relativeDataPath);
        if(dataExists == false)
        {
            WriteToStorage();
            return;
        }
        Instance = StorageGateway.ReadData<Options>(_relativeDataPath);
    }

    public void WriteToStorage()
    {
        Debug.Log($"Writing to storage.");
        Task.Run(() => StartWritingToStorage());
    }

    private void StartWritingToStorage()
    {
        StorageGateway.WriteData(_relativeDataPath, Instance);
    }

    public void ReturnToDefault()
    {
        Instance.MasterVolume = Default.MasterVolume;
        Instance.Difficulty = Default.Difficulty;
    }
}
