using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TimeKit.Core.Linked;

namespace TimeKit
{
    [Serializable]
    public sealed class Cooldown : IReadOnlyCooldown, IClockTickLinked, IDisposable
    {
        public event Action CooldownEnded;

        private readonly Clock _realClock;
        private readonly IClock _clock;
        private float _duration;
        private double _readyAt;

        private bool _isActive;
        
#if UNITY_EDITOR
        public string Trace { get; }
#endif

        internal Cooldown(IClock clock, float duration,
            string file, int line, string member)
        {
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            if (duration <= 0)
                throw new InvalidOperationException("Cooldown duration must be greater than zero.");
            
            _duration = duration;
            _readyAt = 0;

            _realClock = TimeManager.GetRealClock(_clock.Type);
            _realClock.Linked.Register(this);

#if UNITY_EDITOR
            Trace = $"{file}:{line} {member}";
#endif
        }

        public bool TryConsume()
        {
            Validate();
            if (!IsReady)
                return false;
                
            _readyAt = _clock.Time + _duration;
            _isActive = true;
            return true;
        }
        
        public void Tick()
        {
            if (!_isActive)
                return;

            if (IsReady)
            {
                _isActive = false;
                CooldownEnded?.Invoke();
            }
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
        
        public void Dispose()
        {
            _realClock.Linked.Unregister(this);
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