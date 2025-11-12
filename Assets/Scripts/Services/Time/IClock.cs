namespace Services.Time
{
    public interface IClock
    {
        /// <summary>누적 시간 (초).</summary>
        double Time { get; }
        /// <summary>이번 프레임 진행 시간 (초).</summary>
        float DeltaTime { get; }
        bool IsPaused { get; }
        
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
    }
}