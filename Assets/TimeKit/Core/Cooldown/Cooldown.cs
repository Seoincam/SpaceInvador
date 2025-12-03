using System;
using System.Diagnostics;
using TimeKit.Core.Clock;
using TimeKit.Core.Linked;
using UnityEngine;

namespace TimeKit
{
    [Serializable]
    public sealed class Cooldown : IReadOnlyCooldown, IClockTickLinked, IDisposable
    {
        public event Action CooldownEnded;

        private readonly Clock _clock;
        private float _duration;
        private double _readyAt;

        private bool _isActive;
        
        // IClockLinked
        public ClockType ClockType => _clock.Type;
        public object Target => this;
        public GameObject GameObject => null;

        internal Cooldown(IClock clock, float duration,
            string file, int line, string member)
        {
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));
            if (duration <= 0)
                throw new InvalidOperationException("Cooldown duration must be greater than zero.");
            
            _duration = duration;
            _readyAt = 0;

            _clock = TimeManager.GetRealClock(clock.Type);
            _clock.Linked.Register(this);
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
            _clock.Linked.Unregister(this);
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