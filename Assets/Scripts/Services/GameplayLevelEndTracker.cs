using Enums;

namespace Services
{
    public interface IGameplayLevelEndTracker : IService
    {
        void StartTracking(string levelCode);
        void EnemyKilled();
        void PlayerDied();
    }

    public class GameplayLevelEndTracker : IGameplayLevelEndTracker
    {
        private readonly IRandomService _randomService;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly ITimeService _timeService;
        private readonly IWindowService _windowService;

        private float _numberOfKilledEnemiesForWin;
        private int _numberOfKilledEnemies;

        public GameplayLevelEndTracker(IRandomService randomService, IStaticDataProvider staticDataProvider,
            ITimeService timeService, IWindowService windowService)
        {
            _randomService = randomService;
            _staticDataProvider = staticDataProvider;
            _timeService = timeService;
            _windowService = windowService;
        }

        public void StartTracking(string levelCode)
        {
            var levelStaticData = _staticDataProvider.GetDataForLevel(levelCode);

            _numberOfKilledEnemiesForWin = _randomService.Range(levelStaticData.MixNumberOfKilledEnemiesForWin,
                levelStaticData.MaxNumberOfKilledEnemiesForWin);
        }

        public void EnemyKilled()
        {
            _numberOfKilledEnemies++;

            if (_numberOfKilledEnemies >= _numberOfKilledEnemiesForWin)
            {
                StopGameSpeed();
                _windowService.ShowEndGameWindow(WindowType.Win);
            }
        }

        public void PlayerDied()
        {
            StopGameSpeed();
            _windowService.ShowEndGameWindow(WindowType.Lose);
        }

        private void StopGameSpeed()
        {
            _timeService.SetGameSpeed(0);
        }
    }
}