using Unity.Mathematics;
using UnityEngine;

namespace Services
{
    public interface IAssetProvider : IService
    {
        T Instantiate<T>(string path, Transform transform) where T : Object;
        T Instantiate<T>(string path, Vector3 location) where T : Object;
        T Instantiate<T>(string path, Vector3 location, Quaternion rotation) where T : Object;
    }

    public class AssetProvider : IAssetProvider
    {
        public T Instantiate<T>(string path, Transform transform) where T : Object
        {
            return Object.Instantiate(GetPrefab<T>(path), transform);
        }

        public T Instantiate<T>(string path, Vector3 location) where T : Object
        {
            return Instantiate<T>(path, location, quaternion.identity);
        }

        public T Instantiate<T>(string path, Vector3 location, Quaternion rotation) where T : Object
        {
            return Object.Instantiate(GetPrefab<T>(path), location, rotation);
        }

        private static T GetPrefab<T>(string path) where T : Object
        {
            var prefab = Resources.Load<T>(path);
            return prefab;
        }
    }
}