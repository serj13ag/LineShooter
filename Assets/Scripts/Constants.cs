using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const string FirstLevelCode = "Level1";
    public const string GameplaySceneName = "GameplayScene";

    // Resource Paths
    public const string LevelDataResourcesPath = "LevelStaticData";
    public const string PlayerPrefabResourcePath = "Player";
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