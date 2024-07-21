using Services;

namespace Infrastructure.StateMachine
{
    public class GameplayLevelState : IPayloadedGameState<string>
    {
        private readonly ServiceLocator _serviceLocator;

        private readonly ISceneLoader _sceneLoader;
        private readonly IUiFactory _uiFactory;
        private readonly ITimeService _timeService;

        private string _levelCode;

        public GameplayLevelState(ServiceLocator serviceLocator, ISceneLoader sceneLoader, IUiFactory uiFactory,
            ITimeService timeService)
        {
            _serviceLocator = serviceLocator;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _timeService = timeService;
        }

        public void Enter(string levelCode)
        {
            _levelCode = levelCode;

            CreateGameplayServices(_serviceLocator);

            _sceneLoader.LoadScene(Constants.GameplaySceneName, OnSceneLoaded, true);
        }

        public void Exit()
        {
            StopGameplayServices(_serviceLocator);
        }

        private static void CreateGameplayServices(ServiceLocator serviceLocator)
        {
            IGameFactory gameFactory = new GameFactory(
                serviceLocator.Get<IAssetProvider>(),
                serviceLocator.Get<IStaticDataProvider>());
            serviceLocator.Register(gameFactory);

            IEnemyFactory enemyFactory = new EnemyFactory(
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IAssetProvider>(),
                serviceLocator.Get<IStaticDataProvider>());
            serviceLocator.Register(enemyFactory);

            IGameplayLevelEndTracker gameEndTracker = new GameplayLevelEndTracker(
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IStaticDataProvider>(),
                serviceLocator.Get<ITimeService>(),
                serviceLocator.Get<IWindowService>());
            serviceLocator.Register(gameEndTracker);

            IEnemyService enemyService = new EnemyService(
                serviceLocator.Get<IStaticDataProvider>(),
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IGameFactory>(),
                serviceLocator.Get<IEnemyFactory>(),
                serviceLocator.Get<ITimeService>(),
                serviceLocator.Get<IGameplayLevelEndTracker>());
            serviceLocator.Register(enemyService);
        }

        private static void StopGameplayServices(ServiceLocator serviceLocator)
        {
            serviceLocator.Remove<IGameplayLevelEndTracker>();
            serviceLocator.Remove<IGameFactory>();
            serviceLocator.Remove<IEnemyFactory>();
            serviceLocator.Remove<IEnemyService>();
        }

        private void OnSceneLoaded()
        {
            _uiFactory.CreateUiRoot();

            var player = _serviceLocator.Get<IGameFactory>().SpawnPlayer(Constants.PlayerSpawnLocation, _levelCode);

            _uiFactory.CreateHud(player);

            _serviceLocator.Get<IEnemyService>().StartSpawnEnemies(_levelCode);
            _serviceLocator.Get<IGameplayLevelEndTracker>().StartTracking(_levelCode);

            _timeService.SetGameSpeed(1);
        }
    }
}