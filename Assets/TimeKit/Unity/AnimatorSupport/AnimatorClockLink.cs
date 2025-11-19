using TimeKit.Unity.Base;
using UnityEngine;

namespace TimeKit.Unity.AnimatorSupport
{
    [AddComponentMenu("")]
    public class AnimatorClockLink : ClockLinkComponent
    {
        internal Animator Target { get; private set; }
        private float _baseSpeed;
        private ClockType _clockType;

        public override void SyncWithClock(IReadOnlyClock clock)
        {
            if (clock.Type != _clockType || !Target)
                return;
            
            if (clock.IsStopped)
                Target.speed = 0f;
            else
                Target.speed = _baseSpeed * clock.TimeScale;
        }

        internal void Bind(ClockType clockType, Animator animator)
        {
            _clockType = clockType;
            Target = animator;
            _baseSpeed = Target.speed;
        }
    }
}