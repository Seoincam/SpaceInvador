using System;

namespace TimeKit
{
    public interface IReadOnlyClock
    {
        ClockType Type { get; }
        
        /// <summary>
        /// 누적 시간 (초).
        /// </summary>
        double Time { get; }
        
        /// <summary>
        /// 이번 프레임 진행 시간 (초).
        /// </summary>
        float DeltaTime { get; }
        
        /// <summary>
        /// 시간의 진행 속도.
        /// </summary>
        float TimeScale { get; }
        
        /// <summary>
        /// 메뉴 등으로 인해 의도된 정지 여부.
        /// </summary>
        bool IsPaused { get; }
        
        /// <summary>
        /// 타임 스케일 등도 고려.
        /// 외부에서 확인할 땐 <see cref="IReadOnlyClock.IsPaused"/>보단
        /// <see cref="IsStopped"/> 권장.
        /// </summary>
        bool IsStopped { get; }
        
        event Action<IReadOnlyClock> StateChanged;
    }
}