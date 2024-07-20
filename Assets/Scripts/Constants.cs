using UnityEngine;

public static class Constants
{
    public const string FirstLevelCode = "Level1";
    public const string GameplaySceneName = "GameplayScene";

    // Resource Paths
    public const string LevelDataResourcesPath = "LevelStaticData";
    public const string PlayerPrefabResourcePath = "Player";

    // Player Settings
    public const float PlayerMoveSpeed = 3f;
    public static Vector2 PlayerSpawnLocation => new Vector2(0f, -2.5f);
    public const float PlayerMoveHorizontalBorder = 1.5f;
    public const float PlayerMoveTopBorder = -2f;
    public const float PlayerMoveBottomBorder = -3f;
}