using TimeKit.Unity.Base;
using UnityEngine;

namespace TimeKit.Unity.AudioSourceSupport
{
    [AddComponentMenu("")]
    public class AudioSourceClockLink : ClockLinkComponent
    {
        internal AudioSource Target { get; private set; }
        private float _basePitch;
        private ClockType _clockType;
        
        public override void SyncWithClock(IReadOnlyClock clock)
        {
            if (clock.Type != _clockType || !Target)
                return;
            
            if (clock.IsStopped)
                Target.Pause();
            else
            {
                Target.UnPause();
                Target.pitch = _basePitch * clock.TimeScale;
            }
        }

        internal void Bind(ClockType clockType, AudioSource audioSource)
        {
            _clockType = clockType;
            Target = audioSource;
            _basePitch = audioSource.pitch;
        }
    }
}