using TimeKit.Unity.Link.Audio;
using TimeKit.Unity.Link.Core;
using UnityEngine;

namespace TimeKit
{
    public static class ClockAudioSourceExtensions
    {
        /// <summary>
        /// Links the audio source to the clock.
        /// </summary>
        public static Clock SetLink(this Clock clock, AudioSource audioSource)
        {
            ClockLinkBinder.Bind<AudioSource, AudioSourceClockLink>(clock.Type, audioSource);
            return clock;
        }

        /// <summary>
        /// Unlinks the audio source from the clock.
        /// </summary>
        public static Clock Unlink(this Clock clock, AudioSource audioSource)
        {
            ClockLinkBinder.TryUnbind<AudioSource, AudioSourceClockLink>(clock.Type, audioSource);
            return clock;
        }

        /// <summary>
        /// Binds the audio source to the specified clock.
        /// </summary>
        public static AudioSource WithClock(this AudioSource audioSource, IReadOnlyClock clock)
        {
            ClockLinkBinder.Bind<AudioSource, AudioSourceClockLink>(clock.Type, audioSource);
            return audioSource;
        }

        /// <summary>
        /// Unbinds the audio source from the specified clock.
        /// </summary>
        public static AudioSource UnlinkClock(this AudioSource audioSource, IReadOnlyClock clock)
        {
            ClockLinkBinder.TryUnbind<AudioSource, AudioSourceClockLink>(clock.Type, audioSource);
            return audioSource;
        }
    }
}