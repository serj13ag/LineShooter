using Infrastructure.StateMachine;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;

        private void Awake()
        {
            var serviceLocator = new ServiceLocator();
            CreateServices(serviceLocator);

            _gameStateMachine = new GameStateMachine(serviceLocator);
            _gameStateMachine.Enter<GameplayLevelState, string>(Constants.FirstLevelCode);

            DontDestroyOnLoad(gameObject);
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

            IGameFactory gameFactory = new GameFactory(
                serviceLocator.Get<IAssetProvider>(),
                serviceLocator.Get<IStaticDataProvider>(),
                serviceLocator.Get<IInputService>());
            serviceLocator.Register(gameFactory);

            IEnemyFactory enemyFactory = new EnemyFactory(
                serviceLocator.Get<IAssetProvider>(),
                serviceLocator.Get<IStaticDataProvider>());
            serviceLocator.Register(enemyFactory);

            IEnemyService enemyService = new EnemyService(
                serviceLocator.Get<IRandomService>(),
                serviceLocator.Get<IEnemyFactory>());
            serviceLocator.Register(enemyService);
        }
    }
}