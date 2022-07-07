using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;

[Serializable]
public class DiamondDynamicDataTO : IObserver
{
    private DiamondDynamicData _diamondDynamicData = default;

    public string Name;
    public string KnowledgeState;
    public int Level;

    public DiamondDynamicDataTO(string name, string knowledgeState, int level)
    {
        Name = name;
        KnowledgeState = knowledgeState;
        Level = level;
    }

    public DiamondDynamicData GetDiamondDynamicData()
    {
        var diamondName = Name.ToEnum<DiamondName>();
        var knowledgeState = KnowledgeState.ToEnum<DiamondKnowledgeState>();
        _diamondDynamicData = new DiamondDynamicData(diamondName,
            knowledgeState, Level);
        _diamondDynamicData.KnowledgeState.Attach(this);
        _diamondDynamicData.Level.Attach(this);
        return _diamondDynamicData;
    }

    private void OnKnowledgeStateChanged()
    {
        KnowledgeState = _diamondDynamicData.KnowledgeState.Value.ToString();
        PlayerDynamicDataTO.Instance.Save();
    }

    private void OnLevelChanged()
    {
        Level = _diamondDynamicData.Level.Value;
        PlayerDynamicDataTO.Instance.Save();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _diamondDynamicData.Level)
            OnLevelChanged();
        else if (subject == _diamondDynamicData.KnowledgeState)
            OnKnowledgeStateChanged();
    }
}
