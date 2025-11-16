using System;
using Attributes;
using UnityEngine;

namespace Services.Time
{
    [Serializable]
    public sealed class GameClock : IClock
    {
        [SerializeField, VisibleOnly] private double time;
        [SerializeField, VisibleOnly] private float deltaTime;
        [SerializeField, VisibleOnly] private bool isPaused;

        [Space, SerializeField] private float timeScale = 1f;

        public double Time => time;
        public float DeltaTime => deltaTime;
        public bool IsPaused => isPaused;
        public bool IsStopped => IsPaused || TimeScale <= 0f;

        public ClockType Type { get; private set; }

        public float TimeScale
        {
            get => timeScale;
            set { timeScale = value; this.SyncTween(); }
        }
        
        public void Tick(float unscaledDeltaTime)
        {
            if (IsStopped)
            {
                deltaTime = 0f;
                return;
            }

            deltaTime = unscaledDeltaTime * TimeScale;
            time += DeltaTime;
        }

        public void Pause()
        {
            isPaused = true;
            this.SyncTween();
        }

        public void Resume()
        {
            isPaused = false;
            this.SyncTween();
        }

        public IDisposable PauseScope() => new PauseToken(this);

        private sealed class PauseToken : IDisposable
        {
            private readonly GameClock _clock;
            private readonly bool _wasPaused;

            public PauseToken(GameClock clock)
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

        public GameClock(ClockType type) { Type = type; }
    }
}