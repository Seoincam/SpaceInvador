using System.Collections.Generic;
using UnityEngine;

namespace TimeKit.Unity.AnimatorSupport
{
    [AddComponentMenu("")]
    public class AnimatorClockLink : ClockLinkComponent<Animator>
    {
        private readonly Dictionary<Animator, float> _baseSpeed = new();
        
        public override void SyncWithClock(IReadOnlyClock clock)
        {
            var listByType = _linked[clock.Type];
            if (listByType == null)
                return;
            
            foreach (var animator in listByType)
            {
                if (clock.IsStopped)
                    animator.speed = 0f;
                else
                    animator.speed = _baseSpeed[animator] * clock.TimeScale;
            }
        }

        internal override void Register(ClockType type, Animator animator)
        {
            base.Register(type, animator);
            _baseSpeed[animator] = animator.speed;
        }
    }
}