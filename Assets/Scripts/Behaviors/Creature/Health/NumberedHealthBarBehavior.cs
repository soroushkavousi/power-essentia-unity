using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(HealthBarBehavior))]
public class NumberedHealthBarBehavior : MonoBehaviour, IObserver
{
    [SerializeField] private TextMeshProUGUI _numberText = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private HealthBarBehavior _healthBarBehavior = default;

    private void Awake()
    {
        _healthBarBehavior = GetComponent<HealthBarBehavior>();
    }

    private void Start()
    {
        _healthBarBehavior.HealthBehavior.Health.Attach(this);
    }

    public void OnNotify(ISubject subject)
    {
        var maxValue = _healthBarBehavior.HealthBehavior.Health.Max.Value;
        var currentValue = _healthBarBehavior.HealthBehavior.Health.Value;
        _numberText.text = $"{currentValue} / {maxValue}";
    }
}
