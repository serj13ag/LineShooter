using System;
using Components;
using Services;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameplayLevelState : IPayloadedGameState<string>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IUiFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IEnemyService _enemyService;
        private readonly ITimeService _timeService;

        private string _levelCode;

        public GameplayLevelState(ISceneLoader sceneLoader, IUiFactory uiFactory, IGameFactory gameFactory,
            IEnemyService enemyService, ITimeService timeService)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
            _enemyService = enemyService;
            _timeService = timeService;
        }

        public void Enter(string levelCode)
        {
            _levelCode = levelCode;

            _sceneLoader.LoadScene(Constants.GameplaySceneName, OnSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnSceneLoaded()
        {
            _uiFactory.CreateUiRoot();

            var player = _gameFactory.SpawnPlayer(Constants.PlayerSpawnLocation, _levelCode);
            player.OnDied += OnPlayerDied;

            _uiFactory.CreateHud(player);

            _enemyService.StartSpawnEnemies(_levelCode);
        }

        private void OnPlayerDied(object sender, EventArgs e)
        {
            var player = (Player)sender;

            _timeService.SetGameSpeed(0);
            Debug.LogWarning("Player died!"); // TODO show window

            player.OnDied -= OnPlayerDied;
        }
    }
}