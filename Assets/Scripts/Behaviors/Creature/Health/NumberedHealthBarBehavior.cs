using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(HealthBarBehavior))]
public class NumberedHealthBarBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _numberText = default;

    private HealthBarBehavior _healthBarBehavior = default;

    private void Awake()
    {
        _healthBarBehavior = GetComponent<HealthBarBehavior>();
    }

    private void Start()
    {
        _healthBarBehavior.HealthBehavior.Health.Attach(this);
        ShowHealthChange();
    }

    private void ShowHealthChange()
    {
        var maxValue = _healthBarBehavior.HealthBehavior.Health.Max.Value.ToLong();
        var currentValue = _healthBarBehavior.HealthBehavior.Health.Value.ToLong();
        _numberText.text = $"{currentValue} / {maxValue}";
    }

    public void OnNotify(ISubject subject)
    {
        ShowHealthChange();
    }
}
