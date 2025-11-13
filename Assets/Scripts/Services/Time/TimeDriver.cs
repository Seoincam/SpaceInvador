using System.Threading;
using UnityEngine;

namespace Services.Time
{
    public sealed class TimeDriver : MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            var existing = FindObjectsByType<TimeDriver>(FindObjectsSortMode.None);
            if (existing.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            GameServices.Time.Tick(UnityEngine.Time.unscaledDeltaTime);
        }
    }
}