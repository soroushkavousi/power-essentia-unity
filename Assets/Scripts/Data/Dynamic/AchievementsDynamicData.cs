using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class AchievementsDynamicData
    {
        public Observable<int> DemonLevel = new();

        private AchievementsDynamicData() { }

        public AchievementsDynamicData(int demonLevel)
        {
            DemonLevel.Value = demonLevel;
        }
    }
}
