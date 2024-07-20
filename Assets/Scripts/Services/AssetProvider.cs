using UnityEngine;

namespace Services
{
    public interface IAssetProvider : IService
    {
        T Instantiate<T>(string path, Vector3 position) where T : Object;
    }

    public class AssetProvider : IAssetProvider
    {
        public T Instantiate<T>(string path, Vector3 position) where T : Object
        {
            var prefab = Resources.Load<T>(path);
            return Object.Instantiate(prefab, position, Quaternion.identity);
        }
    }
}