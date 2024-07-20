using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelDataScriptableObject : ScriptableObject
    {
        public string LevelCode;

        public int MixNumberOfKilledEnemiesForWin;
        public int MaxNumberOfKilledEnemiesForWin;

        public float MinSpawnEnemyCooldownSeconds;
        public float MaxSpawnEnemyCooldownSeconds;

        public int EnemyMaxHealth;

        public float MinEnemySpeed;
        public float MaxEnemySpeed;

        public int PlayerMaxHealth;
        public float PlayerShootRange;
        public float PlayerShootCooldownSeconds;
        public int PlayerDamage;
        public float PlayerProjectileSpeed;
    }
}