using TimeKit.Unity.Link.Audio;
using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAudioSourceExtensions
    {
        public static IClock SetLink(this IClock clock, AudioSource audioSource)
        {
            ClockLinkBinder.Bind<AudioSource, AudioSourceClockLink>(clock.Type, audioSource);
            return clock;
        }

        public static IClock Unlink(this IClock clock, AudioSource audioSource)
        {
            ClockLinkBinder.Unbind<AudioSource, AudioSourceClockLink>(clock.Type, audioSource);
            return clock;
        }

        public static AudioSource WithClock(this AudioSource audioSource, IReadOnlyClock clock)
        {
            ClockLinkBinder.Bind<AudioSource, AudioSourceClockLink>(clock.Type, audioSource);
            return audioSource;
        }

        public static AudioSource UnlinkClock(this AudioSource audioSource, IReadOnlyClock clock)
        {
            ClockLinkBinder.Unbind<AudioSource, AudioSourceClockLink>(clock.Type, audioSource);
            return audioSource;
        }
    }
}