namespace Assets.Scripts.Models
{
    public class CriticalEffect
    {
        private readonly float _inputDamage = default;
        private readonly float _criticalChance = default;
        private readonly float _criticalDamage = default;
        private bool _isApplied = default;
        private float _resultDamage = default;

        public bool IsApplied => _isApplied;
        public float Result => _resultDamage;

        public CriticalEffect(float inputDamage, float criticalChance, float criticalDamage)
        {
            _inputDamage = inputDamage;
            _criticalChance = criticalChance;
            _criticalDamage = criticalDamage;
            Apply();
        }

        private void Apply()
        {
            _resultDamage = _inputDamage;
            if (_criticalChance == 0)
                return;
            if (_criticalDamage == 0)
                return;
            var randomValue = UnityEngine.Random.Range(0f, 100f);
            if (randomValue > _criticalChance)
                return;
            _isApplied = true;
            _resultDamage = _resultDamage.AddPercentage(_criticalDamage);
        }
    }
}
