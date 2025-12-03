using System;
using TimeKit.Core.Linked;
using TimeKit.Core.Save;

namespace TimeKit
{
    public sealed class Clock : IReadOnlyClock
    {
        public ClockType Type { get; }
        
        public double Time { get; private set; }

        public float DeltaTime { get; private set; }

        public bool IsPaused { get; private set; }

        public float TimeScale { get; private set; }

        public bool IsStopped => IsPaused || TimeScale <= 0f;
        
        public event Action<IReadOnlyClock> StateChanged;

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
        
        
        internal ClockLinkedGroup Linked { get; } 
        
        internal Clock(ClockType type)
        {
            Type = type;
            TimeScale = 1f;
            Linked = new ClockLinkedGroup(this);
        }
        
        internal void Tick(float unscaledDeltaTime)
        {
            if (IsStopped)
            {
                DeltaTime = 0f;
                return;
            }

            DeltaTime = unscaledDeltaTime * TimeScale;
            Time += DeltaTime;
        }
        
        internal ClockSnapshot CreateSnapshot()
        {
            return new ClockSnapshot()
            {
                type = Type,
                time = Time,
                timeScale = TimeScale,
                isPaused = IsPaused
            };
        }
        
        internal void Restore(ClockSnapshot snapshot)
        {
            if (snapshot.type != Type)
                throw new InvalidOperationException($"Clock type mismatch. Expected {Type}, but got {snapshot.type}.");
            
            Time = snapshot.time;
            TimeScale = snapshot.timeScale;
            IsPaused = snapshot.isPaused;
            
            StateChanged?.Invoke(this);
        }
        
        
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