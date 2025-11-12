using System;
using UnityEngine;

namespace Services.Time
{
    [Serializable]
    public sealed class GameClock : IClock
    {
        [SerializeField] private double time;
        [SerializeField] private float deltaTime;
        [SerializeField] private bool isPaused;

        [Space, SerializeField] private float timeScale = 1f;

        public double Time => time;
        public float DeltaTime => deltaTime;
        public bool IsPaused => isPaused;

        public float TimeScale { get => timeScale; set => timeScale = value; }
        
        public void Tick(float unscaledDeltaTime)
        {
            if (IsPaused || TimeScale <= 0)
            {
                deltaTime = 0f;
                return;
            }

            deltaTime = unscaledDeltaTime * TimeScale;
            time += DeltaTime;
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;

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
    }
}