using UnityEngine;

[RequireComponent(typeof(ResourceDisplayBehavior))]
public class GameResourceDisplayBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private ResourceType _resourceType = default;

    private ResourceDisplayBehavior _resourceDisplayBehavior = default;
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Observable<long> _resourceAmount = default;

    void Awake()
    {
        _resourceDisplayBehavior = GetComponent<ResourceDisplayBehavior>();
        _resourceDisplayBehavior.AmountText.text = "0";
        _resourceAmount = PlayerBehavior.Main.DynamicData.ResourceBunches
            .Find(rb => rb.Type == _resourceType).Amount;
        _resourceAmount.Attach(this);
        _resourceDisplayBehavior.AmountText.text = _resourceAmount.Value.ToString();
    }

    private void ShowResourceChange()
    {
        _resourceDisplayBehavior.AmountText.text = _resourceAmount.Value.ToString();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _resourceAmount)
        {
            ShowResourceChange();
        }
    }
}
