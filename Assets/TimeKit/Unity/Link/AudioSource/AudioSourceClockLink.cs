using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit.Unity.Link.Audio
{
    [AddComponentMenu("")]
    public class AudioSourceClockLink : ClockLinkComponent<AudioSource>
    {
        private AudioSource _target;
        private float _basePitch;
        private ClockType _clockType;
        
        internal override AudioSource Target => _target;
        public override ClockType Type => _clockType;

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
            _target = audioSource;
            _basePitch = audioSource.pitch;
            
            TimeManager.GetRealClock(clockType).linked.Register(this);
        }
    }
}