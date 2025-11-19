using System;

namespace TimeKit
{
    public sealed class CooldownObserver : IDisposable
    {
        public readonly IReadOnlyCooldown Cooldown;
        private float _lastNotified;

        public event Action<float> RemainingChanged;
        public event Action<float> RatioChanged;
        
        internal CooldownObserver(IReadOnlyCooldown cooldown)
        {
            this.Cooldown = cooldown ?? throw new ArgumentNullException(nameof(cooldown));
            TimeManager.Ticked += OnTick;
        }

        private void OnTick()
        {
            float curRemaining = Cooldown.Remaining;
            float ratio = Cooldown.RemainingRatio;

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