using Components;
using UnityEngine;

namespace Services
{
    public interface IGameFactory : IService
    {
        void SpawnPlayer(Vector2 location, string levelCode);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataProvider _staticDataProvider;
        private readonly IInputService _inputService;
        private readonly ITimeService _timeService;

        public GameFactory(IAssetProvider assetProvider, IStaticDataProvider staticDataProvider,
            IInputService inputService, ITimeService timeService)
        {
            _assetProvider = assetProvider;
            _staticDataProvider = staticDataProvider;
            _inputService = inputService;
            _timeService = timeService;
        }

        public void SpawnPlayer(Vector2 location, string levelCode)
        {
            var player = _assetProvider.Instantiate<Player>(Constants.PlayerPrefabResourcePath, location);

            var levelStaticData = _staticDataProvider.GetDataForLevel(levelCode);

            player.Init(_inputService, _timeService, levelStaticData.PlayerMaxHealth, levelStaticData.PlayerDamage,
                Constants.PlayerMoveSpeed, levelStaticData.PlayerShootRange, levelStaticData.PlayerShootCooldownSeconds,
                levelStaticData.PlayerProjectileSpeed);
        }
    }
}