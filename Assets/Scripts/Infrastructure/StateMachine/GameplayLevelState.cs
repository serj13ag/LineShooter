using Services;

namespace Infrastructure.StateMachine
{
    public class GameplayLevelState : IPayloadedGameState<string>
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IUiFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IEnemyService _enemyService;
        private readonly IGameplayLevelEndTracker _gameplayLevelEndTracker;

        private string _levelCode;

        public GameplayLevelState(ISceneLoader sceneLoader, IUiFactory uiFactory, IGameFactory gameFactory,
            IEnemyService enemyService, IGameplayLevelEndTracker gameplayLevelEndTracker)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
            _enemyService = enemyService;
            _gameplayLevelEndTracker = gameplayLevelEndTracker;
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

            _uiFactory.CreateHud(player);

            _enemyService.StartSpawnEnemies(_levelCode);
            _gameplayLevelEndTracker.StartTracking(_levelCode, player);
        }
    }
}