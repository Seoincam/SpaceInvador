using Services.Time;
using UnityEngine;

namespace Services
{
    public static class GameServices
    {
        public static TimeService Time { get; private set; }
        public static SpawnService Spawner { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Application.targetFrameRate = 60;
            
            Time = new TimeService();
            
            var assetProvider = new ResourcesAssetProvider("Prefabs/");
            var pool = new PoolService();
            Spawner = new SpawnService(assetProvider, pool);
            
            Debug.Log("[GameServices] Initialized");
        }
    }
}