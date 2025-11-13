using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Time
{
    public class TimeService : MonoBehaviour
    {
        public static TimeService Instance { get; private set; }
        
        private readonly Dictionary<ClockType, IClock> _clocks;

        public IReadOnlyDictionary<ClockType, IClock> Clocks => _clocks;

        private void Awake()
        {
            if (Instance != null) 
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 모든 타입의 Clock 생성 보장
            foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
                _clocks[type] = new GameClock(type);
        }

        private void Update()
        {
            // TimeScale 등에 영향 받지 않는 Unity의 실제 시간.
            float udt = UnityEngine.Time.unscaledDeltaTime;

            foreach (var clock in _clocks.Values)
            {
                clock.Tick(udt);
            }
        }
    }
}