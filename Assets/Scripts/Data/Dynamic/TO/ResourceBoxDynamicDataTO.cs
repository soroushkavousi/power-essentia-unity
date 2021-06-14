using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ResourceBoxDynamicDataTO
{
    private ResourceBox _resourceBox = default;

    public List<ResourceBunchDynamicDataTO> ResourceBunches = default;

    public long GetResourceAmount(ResourceType type)
    {
        var _resourceBunch = ResourceBunches.Find(rb => rb.Type == type.ToString());
        if (_resourceBunch != null)
            return _resourceBunch.Amount;

        return 0;
    }

    private ResourceBoxDynamicDataTO() { }

    public ResourceBoxDynamicDataTO(List<ResourceBunchDynamicDataTO> resourceBunches)
    {
        ResourceBunches = resourceBunches;
    }

    public ResourceBox GetResourceBox()
    {
        var resourceBunches = ResourceBunches.Select(rbdto => rbdto.GetResourceBunch()).ToList();
        _resourceBox = new ResourceBox(resourceBunches);
        return _resourceBox;
    }
}
