using System;
using System.Collections.Generic;

namespace Services.Time
{
    public static class TimeManager
    {
        private static readonly Dictionary<ClockType, IClock> _clockMap = new();

        public static IReadOnlyDictionary<ClockType, IClock> Clocks => _clockMap;

        public static void Initialize()
        {
            // 모든 타입의 Clock 생성 보장
            foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
                _clockMap[type] = new GameClock(type);
        }
        
        public static void Tick()
        {
            float unscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;
            foreach (var clock in _clockMap.Values)
            {
                clock.Tick(unscaledDeltaTime);
                clock.SyncTween();
            }
        }
    }
}