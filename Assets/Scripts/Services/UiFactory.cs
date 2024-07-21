using Components;
using Ui;
using UnityEngine;

namespace Services
{
    public interface IUiFactory : IService
    {
        void CreateUiRoot();
        void CreateHud(Player player);
    }

    public class UiFactory : IUiFactory
    {
        private readonly IAssetProvider _assetProvider;

        private Transform _uiRootTransform;

        public UiFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void CreateUiRoot()
        {
            var uiRoot = _assetProvider.Instantiate<Canvas>(Constants.UiRootResourcePath, Vector3.zero);
            uiRoot.worldCamera = Camera.main;

            _uiRootTransform = uiRoot.transform;
        }

        public void CreateHud(Player player)
        {
            var hud = _assetProvider.Instantiate<GameObject>(Constants.UiHudResourcePath, _uiRootTransform);
            hud.GetComponentInChildren<UiHealthBar>().Init(player.HealthBlock);
        }
    }
}