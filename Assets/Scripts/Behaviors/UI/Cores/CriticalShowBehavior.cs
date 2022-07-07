using TMPro;
using UnityEngine;

public class CriticalShowBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numberText = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private float _number = default;

    public void FeedData(float number)
    {
        _number = Mathf.Abs(number);
        _numberText.text = _number.ToString();
    }

    public void GetDestroyed()
    {
        Destroy(gameObject);
    }
}
