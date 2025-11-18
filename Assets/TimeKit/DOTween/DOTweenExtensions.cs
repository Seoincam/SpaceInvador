#if DOTWEEN
using TimeKit.Core;

namespace TimeKit.DOTween
{
    public static class DOTweenExtensions
    {
        /// <summary>
        /// 지정된 <see cref="IClock"/> 상태에 따라 DOTween 트윈의 재생 여부 및
        /// <c>timeScale</c>을 동기화.
        /// </summary>
        /// <param name="clock">DOTween 트윈과 동기화할 대상 <see cref="IClock"/>.</param>
        public static void SyncTween(ClockType type)
        {
            /*
            if (clock.IsStopped)
                DG.Tweening.DOTween.Pause(clock.Type);
            else
                DG.Tweening.DOTween.Play(clock.Type);

            var list = DG.Tweening.DOTween.TweensById(clock.Type);
            if (list == null)
                return;
            foreach (var tween in list)
                tween.timeScale = clock.TimeScale;
                */
        }
    }
}
#endif