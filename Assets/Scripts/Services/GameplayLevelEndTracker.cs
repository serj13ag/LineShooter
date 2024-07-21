using System;
using Components;
using Enums;

namespace Services
{
    public interface IGameplayLevelEndTracker : IService
    {
        void StartTracking(string levelCode, Player player);
        void EnemyKilled();
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

        public void StartTracking(string levelCode, Player player)
        {
            var levelStaticData = _staticDataProvider.GetDataForLevel(levelCode);

            _numberOfKilledEnemiesForWin = _randomService.Range(levelStaticData.MixNumberOfKilledEnemiesForWin,
                levelStaticData.MaxNumberOfKilledEnemiesForWin);

            player.OnDied += OnPlayerDied;
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

        private void OnPlayerDied(object sender, EventArgs e)
        {
            var player = (Player)sender;

            StopGameSpeed();
            _windowService.ShowEndGameWindow(WindowType.Lose);

            player.OnDied -= OnPlayerDied;
        }

        private void StopGameSpeed()
        {
            _timeService.SetGameSpeed(0);
        }
    }
}