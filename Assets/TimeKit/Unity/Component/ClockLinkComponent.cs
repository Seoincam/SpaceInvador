using System.Collections.Generic;
using TimeKit.Unity.Animator;
using TimeKit.Unity.AudioSource;
using UnityEngine;

namespace TimeKit.Unity.Component
{
    [AddComponentMenu("")]
    public class ClockLinkComponent : MonoBehaviour
    {
        private readonly Dictionary<ClockType, List<UnityEngine.Animator>> _linkedAnimators = new();
        private readonly Dictionary<ClockType, List<UnityEngine.AudioSource>> _linkedAudioSources = new();
        
        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
        }

        internal void LinkAnimator(ClockType type, UnityEngine.Animator animator)
        {
            if (!_linkedAnimators.ContainsKey(type))
                _linkedAnimators[type] = new List<UnityEngine.Animator>();

            _linkedAnimators[type].Add(animator);
        }

        internal void LinkAudioSource(ClockType type, UnityEngine.AudioSource audioSource)
        {
            if (!_linkedAudioSources.ContainsKey(type))
                _linkedAudioSources[type] = new List<UnityEngine.AudioSource>();
            
            _linkedAudioSources[type].Add(audioSource);
        }

        private void OnDestroy()
        {
            foreach (var (type, list) in _linkedAnimators)
            {
                if (list != null)
                    foreach (var animator in list)
                        AnimatorClockLinkGroup.Unregister(type, animator);
            }

            foreach (var (type, list) in _linkedAudioSources)
            {
                if (list != null)
                    foreach (var audioSource in list)
                        AudioSourceClockLinkGroup.Unregister(type, audioSource);
            }
        }
    }
}