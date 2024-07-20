using UnityEngine;

public static class Constants
{
    public const string FirstLevelCode = "Level1";
    public const string GameplaySceneName = "GameplayScene";

    public const string LevelDataResourcesPath = "LevelStaticData";
    public const string PlayerPrefabResourcePath = "Player";

    public static Vector2 PlayerSpawnLocation => new Vector2(0f, -2.5f);
}