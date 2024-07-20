using Components;
using UnityEngine;

namespace Services
{
    public interface IEnemyFactory : IService
    {
        Enemy SpawnEnemy(Vector2 location, string levelCode);
    }

    public class EnemyFactory : IEnemyFactory
    {
        private readonly IRandomService _randomService;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataProvider _staticDataProvider;

        public EnemyFactory(IRandomService randomService, IAssetProvider assetProvider,
            IStaticDataProvider staticDataProvider)
        {
            _randomService = randomService;
            _assetProvider = assetProvider;
            _staticDataProvider = staticDataProvider;
        }

        public Enemy SpawnEnemy(Vector2 location, string levelCode)
        {
            var enemy = _assetProvider.Instantiate<Enemy>(Constants.EnemyPrefabResourcePath, location);

            var levelStaticData = _staticDataProvider.GetDataForLevel(levelCode);

            var enemySpeed = _randomService.Range(levelStaticData.MinEnemySpeed, levelStaticData.MaxEnemySpeed);
            enemy.Init(levelStaticData.EnemyMaxHealth, enemySpeed);
            return enemy;
        }
    }
}