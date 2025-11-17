using System;

namespace TimeKit.Core
{
    public sealed class CooldownObserver : IDisposable
    {
        private readonly IReadOnlyCooldown _cooldown;
        private float _lastNotified;

        public event Action<float> RemainingChanged;
        public event Action<float> RatioChanged;
        
        public CooldownObserver(IReadOnlyCooldown cooldown)
        {
            _cooldown = cooldown ?? throw new ArgumentNullException(nameof(cooldown));
            TimeManager.Ticked += OnTick;
        }

        private void OnTick()
        {
            float curRemaining = _cooldown.Remaining;
            float ratio = _cooldown.RemainingRatio;

            if (_lastNotified - curRemaining > float.Epsilon)
            {
                _lastNotified = curRemaining;
                RemainingChanged?.Invoke(curRemaining);
            }
            RatioChanged?.Invoke(ratio);
        }
        
        public void Dispose()
        {
            TimeManager.Ticked -= OnTick;
            RemainingChanged = null;
            RatioChanged = null;
        }
    }
}