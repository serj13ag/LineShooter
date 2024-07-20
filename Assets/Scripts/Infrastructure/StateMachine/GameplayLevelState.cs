using Services;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameplayLevelState : IPayloadedGameState<string>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IGameFactory _gameFactory;
        private readonly IEnemyService _enemyService;

        private string _levelCode;

        public GameplayLevelState(ISceneLoader sceneLoader, IStaticDataProvider staticDataProvider,
            IGameFactory gameFactory, IEnemyService enemyService)
        {
            _sceneLoader = sceneLoader;
            _staticDataProvider = staticDataProvider;
            _gameFactory = gameFactory;
            _enemyService = enemyService;
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
            var levelStaticData = _staticDataProvider.GetDataForLevel(_levelCode);
            Debug.Log(levelStaticData.LevelCode); // TODO remove

            _gameFactory.SpawnPlayer(Constants.PlayerSpawnLocation, _levelCode);

            _enemyService.StartSpawnEnemies(_levelCode);
        }
    }
}