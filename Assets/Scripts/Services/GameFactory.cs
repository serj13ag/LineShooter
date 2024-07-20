using Components;
using UnityEngine;

namespace Services
{
    public interface IGameFactory : IService
    {
        Player Player { get; }

        Player SpawnPlayer(Vector2 location, string levelCode);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataProvider _staticDataProvider;

        public Player Player { get; private set; }

        public GameFactory(IAssetProvider assetProvider, IStaticDataProvider staticDataProvider)
        {
            _assetProvider = assetProvider;
            _staticDataProvider = staticDataProvider;
        }

        public Player SpawnPlayer(Vector2 location, string levelCode)
        {
            Player = _assetProvider.Instantiate<Player>(Constants.PlayerPrefabResourcePath, location);

            var levelStaticData = _staticDataProvider.GetDataForLevel(levelCode);

            Player.Init(levelStaticData.PlayerMaxHealth, levelStaticData.PlayerDamage, Constants.PlayerMoveSpeed,
                levelStaticData.PlayerShootRange, levelStaticData.PlayerShootCooldownSeconds,
                levelStaticData.PlayerProjectileSpeed);

            return Player;
        }
    }
}