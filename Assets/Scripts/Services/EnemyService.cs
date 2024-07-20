using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public interface IEnemyService : IService
    {
        void StartSpawnEnemies(string levelCode);
    }

    public class EnemyService : IEnemyService
    {
        private readonly IRandomService _randomService;
        private readonly IEnemyFactory _enemyFactory;

        private readonly List<Vector2> _spawnLocations;

        public EnemyService(IRandomService randomService, IEnemyFactory enemyFactory)
        {
            _randomService = randomService;
            _enemyFactory = enemyFactory;

            _spawnLocations = Constants.EnemySpawnLocations;
        }

        public void StartSpawnEnemies(string levelCode)
        {
            var spawnLocation = GetRandomSpawnLocation();

            _enemyFactory.SpawnEnemy(spawnLocation, levelCode);
        }

        private Vector2 GetRandomSpawnLocation()
        {
            return _spawnLocations[_randomService.Range(_spawnLocations.Count)];
        }
    }
}