using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ResourceBoxStaticData
{
    [SerializeField] private List<ResourceBunchStaticData> _resourceBunches = default;

    //public List<ResourceBunchStaticData> ResourceBunches => _resourceBunches;

    public long GetResourceAmount(ResourceType type)
    {
        var _resourceBunch = _resourceBunches.Find(rb => rb.Type == type);
        if (_resourceBunch != null)
            return _resourceBunch.Amount;

        return 0;
    }
}
