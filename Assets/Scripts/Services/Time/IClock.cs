namespace Services.Time
{
    public interface IClock
    {
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
        /// 외부에서 확인할 땐 <see cref="IsPaused"/>보단
        /// <see cref="IsStopped"/> 권장.
        /// </summary>
        bool IsStopped { get; }
        
        /// <summary>
        /// 매 프레임 호출 (<c>Update()</c> 등에서).
        /// </summary>
        void Tick(float unscaledDeltaTime);

        void Pause();
        void Resume();

        /// <summary>
        /// using 형태로 일시 정지 범위 제어용.
        /// </summary>
        System.IDisposable PauseScope();

        ClockType Type { get; }
    }
}