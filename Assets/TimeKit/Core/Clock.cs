using System;

namespace TimeKit.Core
{
    [Serializable]
    internal sealed class Clock : IClock
    {
        private float _timeScale;
        
        public double Time { get; private set; }

        public float DeltaTime { get; private set; }

        public bool IsPaused { get; private set; }

        public float TimeScale
        {
            get => _timeScale;
            set
            {
                _timeScale = value;
                TimeScaleChanged?.Invoke(this);
            }
        }
        
        public ClockType Type { get; private set; }

        public bool IsStopped => IsPaused || TimeScale <= 0f;
        
        public event Action<IReadOnlyClock> TimeScaleChanged;
        public event Action<IReadOnlyClock> Paused;
        public event Action<IReadOnlyClock> Resumed;
        
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
            Paused?.Invoke(this);
        }

        public void Resume()
        {
            IsPaused = false;
            Resumed?.Invoke(this);
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

        public Clock(ClockType type)
        {
            Type = type;
            TimeScale = 1f;
        }
    }
}