using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public interface IAssetProvider
    {
        T Load<T>(string key) where T : Object;    
    }

    /// <summary>
    /// Resources 폴더 기반 로더.
    /// </summary>
    public class ResourcesAssetProvider : IAssetProvider
    {
        private readonly string _basePath;
    
        public ResourcesAssetProvider(string basePath = "")
        {
            _basePath = basePath;
        }
    
        public T Load<T>(string key) where T : Object
        {
            string fullPath = string.IsNullOrEmpty(_basePath)
                ? key
                : $"{_basePath}{key}";

            T asset = Resources.Load<T>(fullPath);
            if (!asset)
            {
                Debug.LogError($"[ResourcesAssetManager] Failed to load: {fullPath}");
            }

            return asset;
        }
    }
}