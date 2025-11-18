using System;

namespace TimeKit.Core
{
    public interface IClock : IReadOnlyClock
    {
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