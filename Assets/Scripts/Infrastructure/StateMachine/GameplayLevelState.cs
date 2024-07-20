using Services;

namespace Infrastructure.StateMachine
{
    public class GameplayLevelState : IPayloadedGameState<string>
    {
        private readonly ISceneLoader _sceneLoader;

        public GameplayLevelState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter(string levelName)
        {
            _sceneLoader.LoadScene(Constants.GameplaySceneName);
        }

        public void Exit()
        {
        }
    }
}