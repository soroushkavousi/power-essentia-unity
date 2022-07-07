using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionLayerBehavior : MonoBehaviour
{
    [SerializeField] private List<InteractionLayerPageBehavior> _pages = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private Vector3 _originalPosition = default;

    public Vector3 OriginalPosition => _originalPosition;

    private void Awake()
    {
        _originalPosition = transform.position;
        StartCoroutine(EnableAllThePages());
    }

    private IEnumerator EnableAllThePages()
    {
        if (_pages == default || _pages.Count == 0)
            yield break;

        transform.position = OutBoxBehavior.Instance.Location1.position;
        _pages.ForEach(c =>
        {
            c.gameObject.SetActive(true);
        });
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _pages.All(c => c.gameObject.activeSelf));
        yield return new WaitForFixedUpdate();
        _pages.ForEach(c =>
        {
            c.gameObject.SetActive(false);
        });
        yield return new WaitUntil(() => _pages.All(c => !c.gameObject.activeSelf));
        transform.position = _originalPosition;
        gameObject.SetActive(false);
    }
}
