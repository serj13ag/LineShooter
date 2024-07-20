using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Services
{
    public interface IEnemyService : IService, ITimeTickable
    {
        void StartSpawnEnemies(string levelCode);
    }

    public class EnemyService : IEnemyService
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IRandomService _randomService;
        private readonly IEnemyFactory _enemyFactory;
        private readonly ITimeService _timeService;

        private readonly List<Vector2> _spawnLocations;

        private float _minSpawnEnemyCooldownSeconds;
        private float _maxSpawnEnemyCooldownSeconds;
        private float _secondsTillSpawnEnemy;
        private string _levelCode;

        public EnemyService(IStaticDataProvider staticDataProvider, IRandomService randomService,
            IEnemyFactory enemyFactory, ITimeService timeService)
        {
            _staticDataProvider = staticDataProvider;
            _randomService = randomService;
            _enemyFactory = enemyFactory;
            _timeService = timeService;

            _spawnLocations = Constants.EnemySpawnLocations;
        }

        public void StartSpawnEnemies(string levelCode)
        {
            _timeService.Subscribe(this); // TODO unsubscribe

            _levelCode = levelCode;

            var levelStaticData = _staticDataProvider.GetDataForLevel(_levelCode);

            _minSpawnEnemyCooldownSeconds = levelStaticData.MinSpawnEnemyCooldownSeconds;
            _maxSpawnEnemyCooldownSeconds = levelStaticData.MaxSpawnEnemyCooldownSeconds;

            SpawnEnemy();
        }

        public void TimeTick(float deltaTime)
        {
            if (_secondsTillSpawnEnemy <= 0)
            {
                SpawnEnemy();
            }
            else
            {
                _secondsTillSpawnEnemy -= deltaTime;
            }
        }

        private void SpawnEnemy()
        {
            var spawnLocation = GetRandomSpawnLocation();
            _enemyFactory.SpawnEnemy(spawnLocation, _levelCode);
            _secondsTillSpawnEnemy = GetRandomSpawnEnemyCooldownSeconds();
        }

        private Vector2 GetRandomSpawnLocation()
        {
            return _spawnLocations[_randomService.Range(_spawnLocations.Count)];
        }

        private float GetRandomSpawnEnemyCooldownSeconds()
        {
            return _randomService.Range(_minSpawnEnemyCooldownSeconds, _maxSpawnEnemyCooldownSeconds);
        }
    }
}