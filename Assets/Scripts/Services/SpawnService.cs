using TimeKit;
using UnityEngine;

namespace Services
{
    public sealed class SpawnService
    {
        private readonly IAssetProvider _assets;
        private readonly IPoolService _pool;
        private readonly IClock _clock;

        public SpawnService(IAssetProvider assets, IPoolService pool)
        {
            _assets = assets;
            _pool = pool;
            _clock = TimeManager.GetClock(ClockType.GamePlay);
        }

        public GameObject Spawn(string key, Transform parent = null)
            => SpawnInternal(key, parent, null, Quaternion.identity);
        
        public GameObject Spawn(string key, Vector3 position, Transform parent = null)
            => SpawnInternal(key, parent, position, Quaternion.identity);
        
        public GameObject Spawn(string key, Vector3 position, Quaternion rotation, Transform parent = null)
            => SpawnInternal(key, parent, position, rotation);

        public void Despawn(GameObject instance)
        {
            if (!instance)
                return;
            
            if (_pool != null)
                _pool.Return(instance);
            else
                Object.Destroy(instance);
        }
        
        private GameObject SpawnInternal(string key, Transform parent, Vector3? worldPos, Quaternion rotation)
        {
            // 1. 프리팹 로드
            GameObject prefab = _assets.Load<GameObject>(key);
            if (prefab == null)
            {
                Debug.LogError($"[SpawnManager] Prefab not found: {key}");
                return null;
            }
            
            // 2. 풀에서 Get 시도
            if (_pool != null && _pool.TryGet(prefab, parent, out GameObject instance))
            {
                if (worldPos.HasValue)
                {
                    instance.transform.position = worldPos.Value;
                    instance.transform.rotation = rotation;
                }

                _clock.Inject(instance);
                return instance;
            }
            
            // 3-1. 풀에 없다면 새로 Instantiate
            instance = worldPos.HasValue 
                ? Object.Instantiate(prefab, worldPos.Value, rotation, parent) 
                : Object.Instantiate(prefab, parent);
            instance.name = prefab.name;
            
            // 3-2. 풀에 등록
            if (_pool is PoolService pool)
                pool.RegisterNewInstance(prefab, instance);

            _clock.Inject(instance);
            return instance;
        }
    }
}