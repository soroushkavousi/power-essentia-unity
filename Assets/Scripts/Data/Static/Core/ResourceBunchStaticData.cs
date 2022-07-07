using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class ResourceBunchStaticData
    {
        public ResourceType Type = default;
        public long Amount = default;
    }

    [Serializable]
    public class ResourceBunchWithLevelStaticData : ResourceBunchStaticData
    {
        public long LevelPercentage = default;
    }
}
