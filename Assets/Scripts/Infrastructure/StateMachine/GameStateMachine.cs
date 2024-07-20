using System;
using System.Collections.Generic;
using Services;

namespace Infrastructure.StateMachine
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedGameState<TPayload>;
        void Enter<TState>() where TState : class, IGameState;
    }

    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableGameState> _states;

        private IExitableGameState _currentState;

        public GameStateMachine(ServiceLocator serviceLocator)
        {
            _states = new Dictionary<Type, IExitableGameState>()
            {
                [typeof(GameplayLevelState)] = new GameplayLevelState(
                    serviceLocator.Get<ISceneLoader>(),
                    serviceLocator.Get<IStaticDataProvider>(),
                    serviceLocator.Get<IGameFactory>()),
            };
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedGameState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Enter<TState>() where TState : class, IGameState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitableGameState
        {
            _currentState?.Exit();

            var state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableGameState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}