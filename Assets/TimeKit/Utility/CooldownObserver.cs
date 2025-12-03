using System;

namespace TimeKit
{
    public sealed class CooldownObserver : IDisposable
    {
        public IReadOnlyCooldown Target { get; private set; }

        public event Action<float> RemainingChanged;
        public event Action<float> RatioChanged;

        public CooldownObserver()
        {
            TimeManager.Ticked += OnTick;
        }
        public CooldownObserver(IReadOnlyCooldown cooldown)
        {
            SetTarget(cooldown);
            TimeManager.Ticked += OnTick;
        }

        public void SetTarget(IReadOnlyCooldown cooldown)
        {
            ThrowIfDisposed();
            Target = cooldown ?? throw new ArgumentNullException(nameof(cooldown));
            _lastRemaining = float.NaN;
        }

        public void ClearTarget()
        {
            ThrowIfDisposed();
            Target = null;
            _lastRemaining = float.NaN;
        }
        
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            
            TimeManager.Ticked -= OnTick;
            Target = null;
        }
        
        
        private bool _disposed;
        private float _lastRemaining;

        private void OnTick()
        {
            if (Target == null)
                return;
            
            float curRemaining = Target.Remaining;
            float ratio = Target.RemainingRatio;

            if (float.IsNaN(_lastRemaining) || Math.Abs(curRemaining - _lastRemaining) > 0.0001f)
            {
                _lastRemaining = curRemaining;
                RemainingChanged?.Invoke(curRemaining);
            }
            
            RatioChanged?.Invoke(ratio);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(CooldownObserver));
        }
    }
}