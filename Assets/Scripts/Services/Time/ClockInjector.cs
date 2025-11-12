using UnityEngine;

namespace Services.Time
{
    public static class ClockInjector
    {
        /// <summary>
        /// <see cref="root"/>를 포함한 모든 자식 오브젝트에서
        /// <see cref="IClockAware"/>를 검색해 <see cref="clock"/>을 주입.
        /// </summary>
        public static void Inject(GameObject root, IClock clock)
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