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

        public GameFactory(IAssetProvider assetProvider, IStaticDataProvider staticDataProvider)
        {
            _assetProvider = assetProvider;
            _staticDataProvider = staticDataProvider;
        }

        public void SpawnPlayer(Vector2 location, string levelCode)
        {
            var player = _assetProvider.Instantiate<Player>(Constants.PlayerPrefabResourcePath, location);

            var levelStaticData = _staticDataProvider.GetDataForLevel(levelCode);

            player.Init(levelStaticData.PlayerMaxHealth, levelStaticData.PlayerDamage, Constants.PlayerMoveSpeed,
                levelStaticData.PlayerShootRange, levelStaticData.PlayerShootCooldownSeconds,
                levelStaticData.PlayerProjectileSpeed);
        }
    }
}