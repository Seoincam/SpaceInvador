using TimeKit.Unity.Base;
using UnityEngine;

namespace TimeKit.Unity.ParticleSystemSupport
{
    [AddComponentMenu("")]
    public class ParticleSystemClockLink : ClockLinkComponent
    {
        internal ParticleSystem Target { get; private set; }
        private float _baseSimulationSpeed;
        private ClockType _clockType;

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

        internal void Bind(ClockType clockType, ParticleSystem system)
        {
            _clockType = clockType;
            Target = system;
            _baseSimulationSpeed = system.main.simulationSpeed;
        }
    }
}