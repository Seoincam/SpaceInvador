#if HAS_UNITASK
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TimeKit
{
    public static class UniTaskExtensions
    {
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
                 await Cysharp.Threading.Tasks.UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
             }
         }

         public static async UniTask WaitUntilReady(this Cooldown cooldown, CancellationToken cancellationToken = default)
         {
             while (!cooldown.IsReady)
                 await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
         }
    }
}
#endif