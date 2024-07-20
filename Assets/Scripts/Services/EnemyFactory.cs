using UnityEngine;

namespace Services
{
    public interface IEnemyFactory : IService
    {
        void SpawnEnemy(Vector2 location, string levelCode);
    }

    public class EnemyFactory : IEnemyFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataProvider _staticDataProvider;

        public EnemyFactory(IAssetProvider assetProvider, IStaticDataProvider staticDataProvider)
        {
            _assetProvider = assetProvider;
            _staticDataProvider = staticDataProvider;
        }

        public void SpawnEnemy(Vector2 location, string levelCode)
        {
            _assetProvider.Instantiate<GameObject>(Constants.EnemyPrefabResourcePath, location);
        }
    }
}