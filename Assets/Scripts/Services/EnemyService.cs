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

        bool TryGetNearestEnemy(Vector3 position, float searchRange, out Enemy nearestEnemy);
    }

    public class EnemyService : IEnemyService
    {
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IRandomService _randomService;
        private readonly IGameFactory _gameFactory;
        private readonly IEnemyFactory _enemyFactory;
        private readonly ITimeService _timeService;

        private readonly List<Vector2> _spawnLocations;

        private readonly List<Enemy> _enemies = new List<Enemy>();

        private string _levelCode;
        private float _numberOfKilledEnemiesForWin;
        private float _minSpawnEnemyCooldownSeconds;
        private float _maxSpawnEnemyCooldownSeconds;

        private float _secondsTillSpawnEnemy;
        private int _numberOfKilledEnemies;

        public EnemyService(IStaticDataProvider staticDataProvider, IRandomService randomService,
            IGameFactory gameFactory, IEnemyFactory enemyFactory, ITimeService timeService)
        {
            _staticDataProvider = staticDataProvider;
            _randomService = randomService;
            _gameFactory = gameFactory;
            _enemyFactory = enemyFactory;
            _timeService = timeService;

            _spawnLocations = Constants.EnemySpawnLocations;
        }

        public void StartSpawnEnemies(string levelCode)
        {
            _timeService.Subscribe(this); // TODO unsubscribe

            _levelCode = levelCode;

            var levelStaticData = _staticDataProvider.GetDataForLevel(_levelCode);

            _numberOfKilledEnemiesForWin = _randomService.Range(levelStaticData.MixNumberOfKilledEnemiesForWin, levelStaticData.MaxNumberOfKilledEnemiesForWin);

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

        public bool TryGetNearestEnemy(Vector3 position, float searchRange, out Enemy nearestEnemy)
        {
            nearestEnemy = null;
            var distanceToNearestEnemy = float.MaxValue;

            foreach (var enemy in _enemies)
            {
                var distanceToEnemy = Vector3.Distance(position, enemy.transform.position);

                if (distanceToEnemy > searchRange || distanceToEnemy >= distanceToNearestEnemy)
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
            enemy.OnDied += OnEnemyDied;

            _enemies.Add(enemy);

            _secondsTillSpawnEnemy = GetRandomSpawnEnemyCooldownSeconds();
        }

        private void OnEnemyCrossedFinishLine(object sender, EventArgs e)
        {
            var enemy = (Enemy)sender;
            enemy.OnCrossedFinishLine -= OnEnemyCrossedFinishLine;

            _gameFactory.Player.HealthBlock.TakeDamage(Constants.EnemyDamage);

            DestroyEnemy(enemy);
        }

        private void OnEnemyDied(object sender, EventArgs e)
        {
            var enemy = (Enemy)sender;
            enemy.OnDied -= OnEnemyDied;

            DestroyEnemy(enemy);

            _numberOfKilledEnemies++;

            if (_numberOfKilledEnemies >= _numberOfKilledEnemiesForWin)
            {
                _timeService.SetGameSpeed(0);
                Debug.LogWarning("Player win!"); // TODO show window
            }
        }

        private void DestroyEnemy(Enemy enemy)
        {
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