using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const string FirstLevelCode = "Level1";
    public const string GameplaySceneName = "GameplayScene";

    public const string PlayerProjectileTag = "PlayerProjectile";
    public const float ProjectileDestroyRange = 3f;

    public const string HealthLocale = "Health";
    public const string WinLocale = "Win";
    public const string LoseLocale = "Lose";
    public const string RestartLocale = "Restart";

    // Resource Paths
    public const string LevelDataResourcesPath = "LevelStaticData";
    public const string UiRootResourcePath = "UiRoot";
    public const string UiHudResourcePath = "UiHud";
    public const string UiEndGameWindowResourcePath = "UiEndGameWindow";
    public const string PlayerPrefabResourcePath = "Player";
    public const string PlayerProjectileResourcePath = "PlayerProjectile";
    public const string EnemyPrefabResourcePath = "GiantBeetle";

    // Player Settings
    public const float PlayerMoveSpeed = 3f;
    public static Vector2 PlayerSpawnLocation => new Vector2(0f, -2.5f);
    public const float PlayerMoveHorizontalBorder = 1.5f;
    public const float PlayerMoveTopBorder = -2f;
    public const float PlayerMoveBottomBorder = -3f;

    // Enemy Settings
    public const int EnemyDamage = 1;

    public static readonly List<Vector2> EnemySpawnLocations = new List<Vector2>()
    {
        new Vector2(-1f, 3.5f),
        new Vector2(0f, 3.5f),
        new Vector2(1f, 3.5f),
    };
}