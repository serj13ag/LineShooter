using Infrastructure.StateMachine;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private ITimeService _timeService;
        private IGameStateMachine _gameStateMachine;

        private void Awake()
        {
            var serviceLocator = new ServiceLocator();
            CreateServices(serviceLocator);

            _timeService = serviceLocator.Get<ITimeService>();

            _gameStateMachine = new GameStateMachine(serviceLocator);
            _gameStateMachine.Enter<GameplayLevelState, string>(Constants.FirstLevelCode);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            _timeService.UpdateTick(Time.deltaTime);
        }

        private void CreateServices(ServiceLocator serviceLocator)
        {
            IRandomService randomService = new RandomService();
            serviceLocator.Register(randomService);

            ISceneLoader sceneLoader = new SceneLoader(this);
            serviceLocator.Register(sceneLoader);

            IAssetProvider assetProvider = new AssetProvider();
            serviceLocator.Register(assetProvider);

            IStaticDataProvider staticDataProvider = new StaticDataProvider();
            staticDataProvider.LoadData();
            serviceLocator.Register(staticDataProvider);

            IInputService inputService = new InputService();
            serviceLocator.Register(inputService);

            ITimeService timeService = new TimeService();
            serviceLocator.Register(timeService);

            IGameplayLevelEndTracker gameEndTracker = new GameplayLevelEndTracker(
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IStaticDataProvider>(),
                serviceLocator.Get<ITimeService>(),
                serviceLocator.Get<IWindowService>());
            serviceLocator.Register(gameEndTracker);

            IUiFactory uiFactory = new UiFactory(serviceLocator.Get<IAssetProvider>());
            serviceLocator.Register(uiFactory);

            IGameFactory gameFactory = new GameFactory(
                serviceLocator.Get<IAssetProvider>(),
                serviceLocator.Get<IStaticDataProvider>());
            serviceLocator.Register(gameFactory);

            IEnemyFactory enemyFactory = new EnemyFactory(
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IAssetProvider>(),
                serviceLocator.Get<IStaticDataProvider>());
            serviceLocator.Register(enemyFactory);

            IWindowService windowService = new WindowService(serviceLocator.Get<IUiFactory>());
            serviceLocator.Register(windowService);

            IEnemyService enemyService = new EnemyService(
                serviceLocator.Get<IStaticDataProvider>(),
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IGameFactory>(),
                serviceLocator.Get<IEnemyFactory>(),
                serviceLocator.Get<ITimeService>(),
                serviceLocator.Get<IGameplayLevelEndTracker>());
            serviceLocator.Register(enemyService);
        }
    }
}