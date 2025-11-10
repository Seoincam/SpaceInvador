using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public interface IPoolService
    {
        bool TryGet(GameObject prefab, Transform parent, out GameObject instance);
        void Return(GameObject instance);
    }
    
    public class PoolService : IPoolService
    {
        // 프리팹 -> 오브젝트 풀 매핑
        private readonly Dictionary<GameObject, Stack<GameObject>> _pools = new();

        // 인스턴스 -> 원본(프리팹) 역매핑
        private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new();
        
        public bool TryGet(GameObject prefab, Transform parent, out GameObject instance)
        {
            if (!prefab)
            {
                instance = null;
                return false;
            }

            if (_pools.TryGetValue(prefab, out var stack) && stack.Count > 0)
            {
                instance = stack.Pop();
                instance.transform.SetParent(parent, false);
                instance.SetActive(true);
                return true;
            }

            instance = null;
            return false;
        }

        public void RegisterNewInstance(GameObject prefab, GameObject instance)
        {
            if (!prefab || !instance)
                return;
            
            _instanceToPrefab[instance] = prefab;
        }

        public void Return(GameObject instance)
        {
            if (!instance)
                return;

            if (!_instanceToPrefab.TryGetValue(instance, out var prefab))
            {
                Object.Destroy(instance);
                return;
            }

            if (!_pools.TryGetValue(prefab, out var stack))
            {
                stack = new();
                _pools[prefab] = stack;
            }
            
            instance.SetActive(false);
            instance.transform.SetParent(null);
            stack.Push(instance);
        }
    }
}