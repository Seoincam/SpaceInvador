using System.Collections.Generic;
using UnityEngine;

namespace TimeKit.Unity.AudioSource
{
    [AddComponentMenu("")]
    public class AudioSourceClockLink : ClockLinkComponent<UnityEngine.AudioSource>
    {
        private readonly Dictionary<UnityEngine.AudioSource, float> _basePitch = new();
        
        public override void SyncWithClock(IReadOnlyClock clock)
        {
            var listByType = _linked[clock.Type];
            if (listByType == null)
                return;

            foreach (var audioSource in listByType)
            {
                if (clock.IsStopped)
                    audioSource.Pause();
                else
                {
                    audioSource.UnPause();
                    audioSource.pitch = _basePitch[audioSource] * clock.TimeScale;
                }
            }
        }

        internal override void Register(ClockType type, UnityEngine.AudioSource audioSource)
        {
            base.Register(type, audioSource);
            _basePitch[audioSource] = audioSource.pitch;
        }
    }
}