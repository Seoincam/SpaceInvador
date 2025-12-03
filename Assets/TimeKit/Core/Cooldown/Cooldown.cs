using System;
using TimeKit.Core.Linked;
using TimeKit.Core.Save;
using UnityEngine;

namespace TimeKit
{
    [Serializable]
    public sealed class Cooldown : IReadOnlyCooldown, IClockTickLinked, IDisposable
    {
        public float Duration { get; private set; }

        public bool IsReady => _readyAt <= _clock.Time;

        public float Remaining => Math.Max(0f, (float)(_readyAt - _clock.Time));

        public float RemainingRatio => Duration <= 0 ? 0f : Math.Clamp(Remaining / Duration, 0f, 1f);
        
        public event Action CooldownEnded;
        public event Action<IReadOnlyCooldown> Disposed;

        public bool TryConsume()
        {
            ThrowIfDisposed();
            if (!IsReady)
                return false;
                
            _readyAt = _clock.Time + Duration;
            _isActive = true;
            return true;
        }

        public void Reset()
        {
            ThrowIfDisposed();
            _readyAt = 0;
            _isActive = false;
        }

        public void Reset(float duration)
        {
            Reset();
            Duration = duration;
        }
        
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            
            _clock.Linked.Unregister(this);
            Disposed?.Invoke(this);
        }
        
        public CooldownSnapshot CreateSnapshot()
        {
            return new CooldownSnapshot()
            {
                isActive = _isActive,
                remaining = Remaining,
                duration = Duration
            };
        }

        public void Restore(CooldownSnapshot data)
        {
            Duration = data.duration > 0 ? data.duration : Duration;
            
            if (data.isActive)
            {
                _isActive = true;
                _readyAt = _clock.Time + data.remaining;
                return;
            }
            
            _isActive = false;
            _readyAt = 0;
        }
        
        
        // IClockLinked
        public ClockType ClockType => _clock.Type;
        public object Target => this;
        public GameObject GameObject => null;
        
        internal Cooldown(IReadOnlyClock clock, float duration)
        {
            Validate(clock, duration);
            
            Duration = duration;
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
        
        private double _readyAt;
        private bool _isActive;
        private bool _disposed;
        
        private void Validate(IReadOnlyClock clock, float duration)
        {
            if (clock == null)
                throw new InvalidOperationException("Cooldown is not initialized. Use clock.Cooldown(duration) to create it.");
            if (duration <= 0)
                throw new InvalidOperationException("Cooldown duration must be greater than zero.");
        }
        
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Cooldown));
        }
    }
}