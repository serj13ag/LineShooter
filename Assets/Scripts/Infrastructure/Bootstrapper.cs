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
            var sceneLoader = new SceneLoader(this);
            serviceLocator.Register<ISceneLoader>(sceneLoader);
        }
    }
}