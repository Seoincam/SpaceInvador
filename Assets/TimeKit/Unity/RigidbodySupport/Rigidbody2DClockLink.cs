using TimeKit.Unity.Base;
using UnityEngine;

namespace TimeKit.Unity.RigidbodySupport
{
    [AddComponentMenu("")]
    public class Rigidbody2DClockLink : ClockLinkComponent<Rigidbody2D>
    {
        private Rigidbody2D _target;
        private Vector2 _baseLinearVelocity;
        private float _baseAngularVelocity;
        private bool _cached;
        private ClockType _clockType;

        internal override Rigidbody2D Target => _target;
        public override ClockType Type => _clockType;

        public override void SyncWithClock(IReadOnlyClock clock)
        {
            if (clock.Type != _clockType || !Target)
                return;
            
            if (!_cached)
                CacheVelocity();

            if (clock.IsStopped)
            {
                Target.linearVelocity = Vector2.zero;
                Target.angularVelocity = 0;
            }
            else
            {
                Target.linearVelocity = _baseLinearVelocity * clock.TimeScale;
                Target.angularVelocity = _baseAngularVelocity * clock.TimeScale;
            }
        }

        internal void Bind(ClockType clockType, Rigidbody2D rb)
        {
            _clockType = clockType;
            _target = rb;
            CacheVelocity();
            
            TimeManager.GetRealClock(clockType).linked.Register(this);
        }

        private void CacheVelocity()
        {
            if (!Target) return;
            _baseLinearVelocity = Target.linearVelocity;
            _baseAngularVelocity = Target.angularVelocity;
            _cached = true;
        }
    }
}