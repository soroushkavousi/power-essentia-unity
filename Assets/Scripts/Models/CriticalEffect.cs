namespace Assets.Scripts.Models
{
    public class CriticalEffect
    {
        private readonly float _number = default;
        private readonly float _chance = default;
        private readonly float _multiplier = default;
        private bool _isApplied = default;
        private float _result = default;

        public bool IsApplied => _isApplied;
        public float Result => _result;

        public CriticalEffect(float number, float chance, float multiplier)
        {
            _number = number;
            _chance = chance;
            _multiplier = multiplier;
            Apply();
        }

        private void Apply()
        {
            _result = _number;
            if (_chance == 0)
                return;
            if (_multiplier == 0)
                return;
            if (UnityEngine.Random.value * 100 > _chance)
                return;
            _isApplied = true;
            _result *= _multiplier / 100;
        }
    }
}
