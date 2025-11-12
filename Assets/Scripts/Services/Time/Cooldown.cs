using UnityEngine;

namespace Services.Time
{
    [System.Serializable]
    public struct Cooldown
    {
        [SerializeField] private float duration;
        private double _readyAt;

        public Cooldown(float duration)
        {
            this.duration = duration;
            _readyAt = 0;
        }
        
        public void Consume(IClock clock) => _readyAt = clock.Time + duration;
        
        public bool Ready(IClock clock) => clock.Time >= _readyAt;
        
        public float Remaining(IClock clock) => Mathf.Max(0f, (float)(_readyAt - clock.Time));
        public float RemainingRatio(IClock clock) => duration <= 0 ? 0f : Mathf.Clamp01(Remaining(clock) / duration);
    }
}