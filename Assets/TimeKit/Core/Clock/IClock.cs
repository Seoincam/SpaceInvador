namespace TimeKit
{
    public interface IClock : IReadOnlyClock
    {
        void Pause();
        void Resume();
        
        void SetTimeScale(float timeScale);
        
        /// <summary>
        /// using 형태로 일시 정지 범위 제어용.
        /// </summary>
        System.IDisposable PauseScope();
    }
}