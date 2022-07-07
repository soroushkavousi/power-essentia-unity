using Assets.Scripts.Models;
using System;

[Serializable]
public class ResourceBunchDynamicDataTO : IObserver
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
        _resourceBunch.Amount.Attach(this);
        return _resourceBunch;
    }

    private void OnResourceBunchAmountChanged()
    {
        Amount = _resourceBunch.Amount.Value;
        PlayerDynamicDataTO.Instance.Save();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _resourceBunch.Amount)
        {
            OnResourceBunchAmountChanged();
        }
    }
}
