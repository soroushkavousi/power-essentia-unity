using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ResourceBunchDynamicDataTO
{
    private ResourceBunch _resourceBunch = default;

    public string Type = default;
    public long Amount = default;

    private ResourceBunchDynamicDataTO() { }

    public ResourceBunchDynamicDataTO(string type, long amount)
    {
        Type = type;
        Amount = amount;
    }

    public ResourceBunch GetResourceBunch()
    {
        _resourceBunch = new ResourceBunch(Type.ToEnum<ResourceType>(), Amount);
        _resourceBunch.Amount.OnNewValueActions.Add(OnResourceBunchAmountChanged);
        return _resourceBunch;
    }

    private void OnResourceBunchAmountChanged(NumberChangeCommand changeCommand)
    {
        Amount = _resourceBunch.Amount.LongValue;
        PlayerDynamicDataTO.Instance.Save();
    }
}
