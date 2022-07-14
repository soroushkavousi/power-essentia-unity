using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarBehavior : MonoBehaviour, IObserver<Damage>, IObserver
{
    [SerializeField] private Image _fill = default;
    [SerializeField] private HealthBehavior _healthBehavior = default;
    [SerializeField] private Gradient _gradient = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private Slider _slider = default;

    public Slider Slider => _slider;
    public HealthBehavior HealthBehavior => _healthBehavior;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void Start()
    {
        _healthBehavior.Health.Attach((IObserver)this);
        _healthBehavior.Health.Attach((IObserver<Damage>)this);
        ShowHealthChange();
    }

    private void ShowCriticalDamage(Damage damage)
    {
        if (damage.IsCritical == true)
        {
            var position = transform.position + new Vector3(0, 20, 0);
            var criticalShowBehavior = Instantiate(
                GameManagerBehavior.Instance.Prefabs.CriticalShowBehavior,
                position, Quaternion.identity, MissionManagerBehavior.Instance.TempBox);
            criticalShowBehavior.FeedData(damage.Value.ToLong());
        }
    }

    private void ShowHealthChange()
    {
        _slider.maxValue = _healthBehavior.Health.Max.Value;
        _slider.value = _healthBehavior.Health.Value;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    public void OnNotify(ISubject<Damage> subject, Damage damage)
    {
        ShowCriticalDamage(damage);
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _healthBehavior.Health)
            ShowHealthChange();
    }
}
