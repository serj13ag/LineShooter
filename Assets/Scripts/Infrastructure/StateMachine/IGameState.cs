namespace Infrastructure.StateMachine
{
    public interface IGameState : IExitableGameState
    {
        void Enter();
    }

    public interface IPayloadedGameState<TPayload> : IExitableGameState
    {
        void Enter(TPayload payload);
    }

    public interface IExitableGameState
    {
        void Exit();
    }
}