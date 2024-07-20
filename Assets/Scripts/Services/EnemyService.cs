using System;
using System.Collections.Generic;
using Components;
using Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public interface IEnemyService : IService, ITimeTickable
    {
        void StartSpawnEnemies(string levelCode);

        bool TryGetNearestEnemy(Vector3 position, out Enemy nearestEnemy);
    }

    public class EnemyService : IEnemyService
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IRandomService _randomService;
        private readonly IEnemyFactory _enemyFactory;
        private readonly ITimeService _timeService;

        private readonly List<Vector2> _spawnLocations;

        private readonly List<Enemy> _enemies = new List<Enemy>();

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

        public bool TryGetNearestEnemy(Vector3 position, out Enemy nearestEnemy)
        {
            nearestEnemy = null;
            var distanceToNearestEnemy = float.MaxValue;

            foreach (var enemy in _enemies)
            {
                var distanceToEnemy = Vector3.Distance(position, enemy.transform.position);

                if (distanceToEnemy >= distanceToNearestEnemy)
                {
                    continue;
                }

                nearestEnemy = enemy;
                distanceToNearestEnemy = distanceToEnemy;
            }

            return nearestEnemy != null;
        }

        private void SpawnEnemy()
        {
            var spawnLocation = GetRandomSpawnLocation();
            var enemy = _enemyFactory.SpawnEnemy(spawnLocation, _levelCode);

            enemy.OnCrossedFinishLine += OnEnemyCrossedFinishLine;

            _enemies.Add(enemy);

            _secondsTillSpawnEnemy = GetRandomSpawnEnemyCooldownSeconds();
        }

        private void OnEnemyCrossedFinishLine(object sender, EventArgs e)
        {
            var enemy = (Enemy)sender;
            Object.Destroy(enemy.gameObject);
            _enemies.Remove(enemy);
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