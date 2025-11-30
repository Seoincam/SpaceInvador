using System;
using System.Collections.Generic;
using TimeKit.Core.Linked;

namespace TimeKit
{
    [Serializable]
    internal sealed class Clock : IClock
    {
        internal ClockLinkedGroup linked;
        
        public double Time { get; private set; }

        public float DeltaTime { get; private set; }

        public bool IsPaused { get; private set; }

        public float TimeScale { get; private set; }

        public ClockType Type { get; private set; }

        public bool IsStopped => IsPaused || TimeScale <= 0f;
        
        public event Action<IReadOnlyClock> StateChanged;
        
        internal Clock(ClockType type)
        {
            Type = type;
            TimeScale = 1f;
            linked = new ClockLinkedGroup(this);
        }
        
        public void Tick(float unscaledDeltaTime)
        {
            if (IsStopped)
            {
                DeltaTime = 0f;
                return;
            }

            DeltaTime = unscaledDeltaTime * TimeScale;
            Time += DeltaTime;
        }

        public void Pause()
        {
            IsPaused = true;
            StateChanged?.Invoke(this);
        }

        public void Resume()
        {
            IsPaused = false;
            StateChanged?.Invoke(this);
        }

        public void SetTimeScale(float timeScale)
        {
            timeScale = Math.Max(0f, timeScale);
            TimeScale = timeScale;
            StateChanged?.Invoke(this);
        }

        public IDisposable PauseScope() => new PauseToken(this);

        private sealed class PauseToken : IDisposable
        {
            private readonly Clock _clock;
            private readonly bool _wasPaused;

            public PauseToken(Clock clock)
            {
                _clock = clock;
                _wasPaused = clock.IsPaused;
                clock.Pause();
            }
            public void Dispose()
            {
                if (!_wasPaused) _clock.Resume();
            }
        }
    }
}