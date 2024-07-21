using Infrastructure.StateMachine;
using Services;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;
        private ITimeService _timeService;

        private void Awake()
        {
            var serviceLocator = new ServiceLocator();
            CreateGlobalServices(serviceLocator);

            _gameStateMachine = new GameStateMachine(serviceLocator);
            serviceLocator.Register(_gameStateMachine);

            _gameStateMachine.Enter<GameplayLevelState, string>(Constants.FirstLevelCode);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            _timeService.UpdateTick(Time.deltaTime);
        }

        private void CreateGlobalServices(ServiceLocator serviceLocator)
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
            _timeService = timeService;

            IUiFactory uiFactory = new UiFactory(serviceLocator.Get<IAssetProvider>());
            serviceLocator.Register(uiFactory);

            IWindowService windowService = new WindowService(serviceLocator.Get<IUiFactory>());
            serviceLocator.Register(windowService);
        }
    }
}