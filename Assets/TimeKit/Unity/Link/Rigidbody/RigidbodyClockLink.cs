using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit.Unity.Link.Rb
{
    [AddComponentMenu("")]
    public class RigidbodyClockLink : ClockLinkComponent<Rigidbody>
    {
        private Rigidbody _target;
        private Vector3 _baseLinearVelocity;
        private Vector3 _baseAngularVelocity;
        private bool _cached;
        private ClockType _clockType;

        internal override Rigidbody Target => _target;
        public override ClockType Type => _clockType;

        public override void SyncWithClock(IReadOnlyClock clock)
        {
            if (clock.Type != _clockType || !Target)
                return;
            
            if (!_cached)
                CacheVelocity();
            
            if (clock.IsStopped)
            {
                Target.linearVelocity = Vector3.zero;
                Target.angularVelocity = Vector3.zero;
            }
            else
            {
                Target.linearVelocity = _baseLinearVelocity * clock.TimeScale;
                Target.angularVelocity = _baseAngularVelocity * clock.TimeScale;
            }
        }

        internal override void Bind(ClockType clockType, Rigidbody rb)
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