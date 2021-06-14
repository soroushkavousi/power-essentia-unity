using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class DiamondStaticData
{
    [Header("Base")]
    [SerializeField] private DiamondName _name = default;
    [SerializeField] private bool _isPermanent = default;
    [SerializeField] private float _startActiveTime = default;
    [SerializeField] private float _startCooldownTime = default;
    [SerializeField] private Sprite _icon = default;
    [SerializeField, TextArea] private string _description = default;
    [SerializeField] private List<ResourceBoxStaticData> _upgradeResourceBoxes = default;

    public DiamondName Name => _name;
    public bool IsPermanent => _isPermanent;
    public float StartActiveTime => _startActiveTime;
    public float StartCooldownTime => _startCooldownTime;
    public string ShowName => _name.ToString().ToLower().CapitalizeFirstCharacter();
    public Sprite Icon => _icon;
    public string Description => _description;
    public List<ResourceBoxStaticData> UpgradeResourceBoxes => _upgradeResourceBoxes;
}
