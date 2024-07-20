using UnityEngine;

namespace Services
{
    public interface IGameFactory : IService
    {
        void SpawnPlayer(Vector2 location);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void SpawnPlayer(Vector2 location)
        {
            _assetProvider.Instantiate<GameObject>(Constants.PlayerPrefabResourcePath, location);
        }
    }
}