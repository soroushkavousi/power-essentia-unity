using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BodyBehavior))]
public class HealSpellBehavior : SpellBehavior, IObserver<CollideData>
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(HealSpellBehavior) + Constants.HeaderEnd)]

    [SerializeField] private HealSpellStaticData _healSpellStaticData = default;
    [SerializeField] private CircleCollider2D _healAreaCircle = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private Number _healAmount = default;
    private BodyBehavior _bodyBehavior = default;
    private ParticleSystem _particleSystem = default;

    public void Initialize(Level level)
    {
        base.Initialize(_healSpellStaticData, level);
        _bodyBehavior = GetComponent<BodyBehavior>();
        _particleSystem = GetComponent<ParticleSystem>();
        _healAreaCircle.radius = 0;
        _bodyBehavior.FeedData();
        _bodyBehavior.Attach(this);
        _healAmount = new(_level, _healSpellStaticData.HealAmountLevelInfo);
        StartCoroutine(GoOnCooldown());
    }

    protected override void CastAction()
    {
        HealAllies();
    }

    private void HealAllies()
    {
        _particleSystem.Play(true);
        StartCoroutine(GrowHealAreaSmouthly());
    }

    private IEnumerator GrowHealAreaSmouthly()
    {
        var growDelayRatio = 0.05f;
        var stepsCount = _healSpellStaticData.HealAreaGrowTime / growDelayRatio;
        var growRaito = _healSpellStaticData.MaxHealAreaRadius / stepsCount;
        while (_healAreaCircle.radius < _healSpellStaticData.MaxHealAreaRadius)
        {
            _healAreaCircle.radius += growRaito;
            yield return new WaitForSeconds(growDelayRatio);
        }
        yield return new WaitForSeconds(growDelayRatio);
        _healAreaCircle.radius = 0;
    }

    private void HealIfDemon(Collider2D otherCollider)
    {
        var bodyAreaBehavior = otherCollider.GetComponent<BodyAreaBehavior>();
        if (bodyAreaBehavior == null)
            return;

        var invaderBehavior = bodyAreaBehavior.BodyBehavior.GetComponent<DemonBehavior>();
        if (invaderBehavior == null)
            return;

        if (!isActiveAndEnabled)
            return;

        StartCoroutine(HealDemon(invaderBehavior));
    }

    private IEnumerator HealDemon(DemonBehavior invaderBehavior)
    {
        if (invaderBehavior == null)
            yield break;
        var invaderHealthBehavior = invaderBehavior.GetComponent<HealthBehavior>();
        if (invaderHealthBehavior == null)
            yield break;
        invaderHealthBehavior.Health.Heal(_healAmount.Value);
    }

    public void OnNotify(ISubject<CollideData> subject, CollideData data)
    {
        if (data.IsCollidingDisabled)
            return;

        if (ReferenceEquals(subject, _bodyBehavior))
        {
            if (data.Type == CollideType.ENTER)
                HealIfDemon(data.TargetCollider2D);
        }
    }
}