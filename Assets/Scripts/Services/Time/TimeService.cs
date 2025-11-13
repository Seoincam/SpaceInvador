using System;
using System.Collections.Generic;

namespace Services.Time
{
    public class TimeService
    {
        private readonly Dictionary<ClockType, IClock> _clocks = new();

        public IReadOnlyDictionary<ClockType, IClock> Clocks => _clocks;

        public TimeService()
        {
            // 모든 타입의 Clock 생성 보장
            foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
                _clocks[type] = new GameClock(type);
        }
        
        /// <param name="unscaledDeltaTime">TimeScale 등에 영향 받지 않는 Unity의 실제 시간.</param>
        public void Tick(float unscaledDeltaTime)
        {
            foreach (var clock in _clocks.Values)
            {
                clock.Tick(unscaledDeltaTime);
                clock.SyncTween();
            }
        }
    }
}