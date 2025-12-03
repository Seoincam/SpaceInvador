using System;
using TimeKit.Core.Linked;
using UnityEngine;

namespace TimeKit
{
    [Serializable]
    public sealed class Cooldown : IReadOnlyCooldown, IClockTickLinked, IDisposable
    {
        public float Duration => _duration;

        public bool IsReady => _readyAt <= _clock.Time;

        public float Remaining => Math.Max(0f, (float)(_readyAt - _clock.Time));

        public float RemainingRatio => _duration <= 0 ? 0f : Math.Clamp(Remaining / _duration, 0f, 1f);
        
        public event Action CooldownEnded;

        public bool TryConsume()
        {
            if (!IsReady)
                return false;
                
            _readyAt = _clock.Time + _duration;
            _isActive = true;
            return true;
        }

        public void Reset()
        {
            _readyAt = 0;
            _isActive = false;
        }
        
        public void Dispose()
        {
            _clock.Linked.Unregister(this);
            // TODO observer도 알려야함
        }
        
        
        // IClockLinked
        public ClockType ClockType => _clock.Type;
        public object Target => this;
        public GameObject GameObject => null;
        
        internal Cooldown(IReadOnlyClock clock, float duration)
        {
            Validate();
            
            _duration = duration;
            _readyAt = 0;

            _clock = TimeManager.GetClock(clock.Type);
            _clock.Linked.Register(this);
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
        
        private readonly Clock _clock;
        
        private float _duration;
        private double _readyAt;
        private bool _isActive;
        
        private void Validate()
        {
            if (_clock == null)
                throw new InvalidOperationException("Cooldown is not initialized. Use clock.Cooldown(duration) to create it.");
            if (_duration <= 0)
                throw new InvalidOperationException("Cooldown duration must be greater than zero.");
        }
    }
}