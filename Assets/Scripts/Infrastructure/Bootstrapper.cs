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

            ITimeService timeService = new TimeService();
            serviceLocator.Register(timeService);

            IInputService inputService = new InputService();
            serviceLocator.Register(inputService);

            IGameFactory gameFactory = new GameFactory(
                serviceLocator.Get<IAssetProvider>(),
                serviceLocator.Get<IStaticDataProvider>(),
                serviceLocator.Get<IInputService>(),
                serviceLocator.Get<ITimeService>());
            serviceLocator.Register(gameFactory);

            IEnemyFactory enemyFactory = new EnemyFactory(
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IAssetProvider>(),
                serviceLocator.Get<IStaticDataProvider>(),
                serviceLocator.Get<ITimeService>());
            serviceLocator.Register(enemyFactory);

            IEnemyService enemyService = new EnemyService(
                serviceLocator.Get<IStaticDataProvider>(),
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IEnemyFactory>(),
                serviceLocator.Get<ITimeService>());
            serviceLocator.Register(enemyService);
        }
    }
}