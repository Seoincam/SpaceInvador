using System;
using System.Collections.Generic;
using TimeKit.Core;
using UnityEngine;

namespace TimeKit.Unity.Animator
{
    internal static class AnimatorClockLinkGroup
    {
        private static readonly Dictionary<ClockType, List<UnityEngine.Animator>> Group = new();
        private static readonly Dictionary<UnityEngine.Animator, float> BaseSpeedMap = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            if (TimeManager.IsInitialized)
            {
                OnInitialized();
            }
            else
            {
                TimeManager.Initialized -= OnInitialized;
                TimeManager.Initialized += OnInitialized;
            }

            return;
            void OnInitialized()
            {
                foreach (ClockType type in Enum.GetValues(typeof(ClockType)))
                {
                    Group[type] = new List<UnityEngine.Animator>();
                
                    var clock = TimeManager.Clocks[type];

                    clock.TimeScaleChanged -= SyncAnimator;
                    clock.Paused -= SyncAnimator;
                    clock.Resumed -= SyncAnimator;

                    clock.TimeScaleChanged += SyncAnimator;
                    clock.Paused += SyncAnimator;
                    clock.Resumed += SyncAnimator;
                }
            }
        }

        private static void SyncAnimator(IReadOnlyClock clock)
        {
            var list = Group[clock.Type];
            foreach (var animator in list)
            {
                if (clock.IsStopped)
                    animator.speed = 0f;
                else
                    animator.speed = BaseSpeedMap[animator] * clock.TimeScale;
            }
        }

        public static void Register(ClockType type, UnityEngine.Animator animator)
        {
            if (Group[type].Contains(animator)) 
                return;
            Group[type].Add(animator);
            BaseSpeedMap[animator] = animator.speed;
        }

        public static void Unregister(ClockType type, UnityEngine.Animator animator)
        {
            Group[type].Remove(animator);
            BaseSpeedMap.Remove(animator);
        }
    }
}