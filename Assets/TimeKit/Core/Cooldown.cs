using System;
using System.Diagnostics;

namespace TimeKit.Core
{
    [Serializable]
    public sealed class Cooldown : IReadOnlyCooldown
    {
        private readonly IClock _clock;
        private float _duration;
        private double _readyAt;

        internal Cooldown(IClock clock, float duration)
        {
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            if (duration <= 0)
                throw new InvalidOperationException("Cooldown duration must be greater than zero.");
            
            _duration = duration;
            _readyAt = 0;
        }

        public void Consume()
        {
            Validate();
            _readyAt = _clock.Time + _duration;
        }
        
        public float Duration => _duration;

        public bool IsReady
        {
            get
            {
                Validate();
                return _readyAt <= _clock.Time;
            }
        }

        public float Remaining
        {
            get
            {
                Validate();
                return Math.Max(0f, (float)(_readyAt - _clock.Time));
            }
        }

        public float RemainingRatio
        {
            get
            {
                Validate();
                return _duration <= 0 ? 0f : Math.Clamp(Remaining / _duration, 0f, 1f);
            }
        }

        public void Reset()
        {
            _readyAt = 0;
        }
        
        [DebuggerHidden]
        private void Validate()
        {
            if (_clock == null)
                throw new InvalidOperationException("Cooldown is not initialized. Use clock.Cooldown(duration) to create it.");
            if (_duration <= 0)
                throw new InvalidOperationException("Cooldown duration must be greater than zero.");
        }
    }
}