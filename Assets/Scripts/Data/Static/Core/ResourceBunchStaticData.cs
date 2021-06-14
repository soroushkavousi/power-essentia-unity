using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ResourceBunchStaticData
{
    [SerializeField] public ResourceType _type = default;
    [SerializeField] public long _amount = default;

    public ResourceType Type => _type;
    public long Amount => _amount;
}
