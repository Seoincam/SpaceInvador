using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit.Unity.Link.Particle
{
    [AddComponentMenu("")]
    public class ParticleSystemClockLink : ClockLinkComponent<ParticleSystem>
    {
        private ParticleSystem _target;
        private float _baseSimulationSpeed;
        private ClockType _clockType;

        internal override ParticleSystem Target => _target;
        public override ClockType Type => _clockType;
        
        internal override void Bind(ClockType clockType, ParticleSystem system)
        {
            _clockType = clockType;
            _target = system;
            _baseSimulationSpeed = system.main.simulationSpeed;
            
            TimeManager.GetClock(clockType).Linked.Register(this);
        }

        internal override void Unbind()
        {
            TimeManager.GetClock(_clockType).Linked.Unregister(this);
        }

        public override void SyncWithClock(IReadOnlyClock clock)
        {
            if (clock.Type != _clockType || !Target)
                return;
            
            var main = Target.main;
            if (clock.IsStopped)
                main.simulationSpeed = 0f;
            else
                main.simulationSpeed = _baseSimulationSpeed * clock.TimeScale;
        }
    }
}