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

            if (ReferenceEquals(Target, cooldown))
                return;

            if (Target != null)
                Target.Disposed -= OnTargetDisposed;
            
            Target = cooldown ?? throw new ArgumentNullException(nameof(cooldown));
            Target.Disposed += OnTargetDisposed;
            _lastRemaining = float.NaN;
        }

        public void ClearTarget()
        {
            ThrowIfDisposed();

            if (Target == null)
                return;
            
            Target.Disposed -= OnTargetDisposed;
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
        
        private void OnTargetDisposed(IReadOnlyCooldown disposedTarget)
        {
            if (!ReferenceEquals(Target, disposedTarget))
                return;

            Target.Disposed -= OnTargetDisposed;
            Target = null;
            _lastRemaining = float.NaN;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(CooldownObserver));
        }
    }
}