using System.Collections.Generic;
using TimeKit.Unity.Animator;
using UnityEngine;

namespace TimeKit.Unity.Component
{
    [AddComponentMenu("")]
    public class ClockLinkComponent : MonoBehaviour
    {
        private Dictionary<ClockType, List<UnityEngine.Animator>> _linkedAnimators;
        
        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector;
        }

        internal void LinkAnimator(ClockType type, UnityEngine.Animator animator)
        {
            _linkedAnimators ??= new Dictionary<ClockType, List<UnityEngine.Animator>>();

            if (!_linkedAnimators.ContainsKey(type))
            {
                _linkedAnimators[type] = new List<UnityEngine.Animator>();
            }

            _linkedAnimators[type].Add(animator);
        }

        private void OnDestroy()
        {
            foreach (var (type, list) in _linkedAnimators)
            {
                if (list != null)
                    foreach (var animator in list)
                        AnimatorClockLinkGroup.Unregister(type, animator);
            }
        }
    }
}