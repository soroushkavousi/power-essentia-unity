using Assets.Scripts.Models;
using System;

[Serializable]
public class AchievementsDynamicDataTO : IObserver
{
    private AchievementsDynamicData _achievementsDynamicData = default;

    public int DemonLevel;

    public AchievementsDynamicDataTO(int demonLevel)
    {
        DemonLevel = demonLevel;
    }

    public AchievementsDynamicData GetAchievementsDynamicData()
    {
        _achievementsDynamicData = new AchievementsDynamicData(DemonLevel);
        _achievementsDynamicData.DemonLevel.Attach(this);
        return _achievementsDynamicData;
    }

    private void HandleDemonLevelChange()
    {
        DemonLevel = _achievementsDynamicData.DemonLevel.Value;
        PlayerDynamicDataTO.Instance.Save();
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _achievementsDynamicData.DemonLevel)
        {
            HandleDemonLevelChange();
        }
    }
}