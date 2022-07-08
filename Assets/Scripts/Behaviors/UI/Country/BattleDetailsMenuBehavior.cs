using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(InteractionLayerPageBehavior))]
public class BattleDetailsMenuBehavior : MonoBehaviour
{
    private static BattleDetailsMenuBehavior _instance = default;
    [SerializeField] private TextMeshProUGUI _demondsLevel = default;
    [SerializeField] private ButtonBehavior _demondsLevelMinusButton = default;
    [SerializeField] private ButtonBehavior _demondsLevelPlusButton = default;
    private InteractionLayerPageBehavior _interactionLayerComponentBehavior = default;

    public static BattleDetailsMenuBehavior Instance => Utils.GetInstance(ref _instance);

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]

    private void Awake()
    {
        _interactionLayerComponentBehavior = GetComponent<InteractionLayerPageBehavior>();
        _demondsLevelMinusButton.OnClickDownActions.Add(() => _demondsLevel.text = (_demondsLevel.text.ToInt() - 1).ToString());
        _demondsLevelPlusButton.OnClickDownActions.Add(() => _demondsLevel.text = (_demondsLevel.text.ToInt() + 1).ToString());
    }

    public void ShowDetailsForCity()
    {
        _interactionLayerComponentBehavior.Enable();
    }

    public void GoNextMission()
    {
        MusicPlayerBehavior.Instance.PlayClickSound();
        SceneManagerBehavior.Instance.LoadPreparation();
    }
}
