using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Services.Time
{
    public static class ClockExtensions
    {
        /// <summary>
        /// <see cref="root"/>를 포함한 모든 자식 오브젝트를 순회하며
        /// <see cref="IClockAware"/>를 조회해 <see cref="clock"/>을 주입.
        /// </summary>
        public static void Inject(this IClock clock, GameObject root)
        {
            var awares = root.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var a in awares)
            {
                if (a is IClockAware aware)
                    aware.Clock = clock;
            }
        }

        /// <summary>
        /// 지정된 <see cref="IClock"/> 상태에 따라 DOTween 트윈의 재생 여부 및
        /// <c>timeScale</c>을 동기화.
        /// </summary>
        /// <param name="clock">DOTween 트윈과 동기화할 대상 <see cref="IClock"/>.</param>
        public static void SyncTween(this IClock clock)
        {
            if (clock.IsStopped)
                DOTween.Pause(clock.Type);
            else
                DOTween.Play(clock.Type);
            
            var list = DOTween.TweensById(clock.Type);
            if (list == null)
                return;
            foreach (var tween in list)
                tween.timeScale = clock.TimeScale;
        }

        /// <summary>
        /// 지정된 <see cref="TimeSpan"/> 동안 <see cref="IClock"/> 기준으로 지연.
        /// </summary>
        public static async UniTask Delay(this IClock clock, TimeSpan delayTimeSpan, CancellationToken cancellationToken = default)
        {
            var sec = (float)delayTimeSpan.TotalSeconds;
            await clock.Delay(sec, cancellationToken);
        }
        
        /// <summary>
        /// 지정된 초 동안 <see cref="IClock"/> 기준으로 지연.
        /// </summary>
        public static async UniTask Delay(this IClock clock, float seconds, CancellationToken cancellationToken = default)
        {
            if (seconds < 0)
                throw new ArgumentOutOfRangeException("Delay does not allow minus second. second: " + seconds);

            double end = clock.Time + seconds;
            while (clock.Time < end)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }
    }
}