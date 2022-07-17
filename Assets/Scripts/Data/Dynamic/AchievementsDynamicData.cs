using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class AchievementsDynamicData
    {
        public Level DemonLevel;

        private AchievementsDynamicData() { }

        public AchievementsDynamicData(int demonLevel)
        {
            DemonLevel = new(demonLevel, GameManagerBehavior.Instance.Settings.DemonMaxLevel);
        }
    }
}
