using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit.Unity.Link.Animation
{
    [AddComponentMenu("")]
    public class AnimatorClockLink : ClockLinkComponent<Animator>
    {
        private Animator _target;
        private float _baseSpeed;
        private ClockType _clockType;
        
        internal override Animator Target => _target;
        public override ClockType Type => _clockType;
        
        internal override void Bind(ClockType clockType, Animator animator)
        {
            _clockType = clockType;
            _target = animator;
            _baseSpeed = Target.speed;
            
            TimeManager.GetRealClock(clockType).linked.Register(this);
        }

        internal override void Unbind()
        {
            TimeManager.GetRealClock(_clockType).linked.Unregister(this);
        }

        public override void SyncWithClock(IReadOnlyClock clock)
        {
            if (clock.Type != _clockType || !Target)
                return;
            
            if (clock.IsStopped)
                Target.speed = 0f;
            else
                Target.speed = _baseSpeed * clock.TimeScale;
        }
    }
}