using TimeKit.Core;
using UnityEngine;

namespace TimeKit.Unity
{
    public static class ClockInjectionExtensions
    {
        public static void Inject(this IClock clock, Component root)
            => clock.Inject(root.gameObject);
        
        /// <summary>
        /// <see cref="root"/>를 포함한 모든 자식 오브젝트를 순회하며
        /// <see cref="IClockAware"/>를 조회해 <see cref="clock"/>을 주입.
        /// </summary>
        public static void Inject(this IClock clock, GameObject root)
        {
            var awares = root.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var a in awares)
            {
                if (a is IClockAware aware)
                    aware.Clock = clock;
            }
        }
    }
}