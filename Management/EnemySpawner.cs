using System;
using System.Linq;
using Entity.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Management
{
    public enum EnemyDifficulty
    {
        Static,
        Easy,
        Medium,
        Hard,
        Insane
    }

    [Serializable]
    public struct EnemyData
    {
        public GameObject enemyGameObject;
        public EnemyDifficulty[] enemyDifficulty;
    }

    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        [SerializeField]
        private EnemyData[] enemies;

        [SerializeField] private float spawnCooldown;

        private float _spawnCooldownTimer;

        private IEnemyBase _currentEnemyComponent;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of EnemySpawner should not exist");
            }
        }

        private void Update()
        {
            if (_currentEnemyComponent is { IsAlive: true } ||
                TerrainSpawner.Instance.OnBossLevel) return;
            _spawnCooldownTimer -= Time.deltaTime;
            if (_spawnCooldownTimer >= 0) return;
            _currentEnemyComponent = Spawn(TerrainSpawner.Instance.CurrentTerrainDifficulty);
            _spawnCooldownTimer = spawnCooldown;
        }

        public IEnemyBase Spawn(TerrainDifficulty difficulty)
        {
            //Enables a enemy based on the current difficulty and terrain difficulty and returns the enemy.
            var spawnableEnemyList = enemies.Where(enemy => enemy.enemyDifficulty.Contains((EnemyDifficulty)(int)difficulty)).ToArray();
            if (spawnableEnemyList.Length == 0) return null;
            var random = Random.Range(0, spawnableEnemyList.Length);
            var spawnedEnemy = spawnableEnemyList[random];
            var enemyObject = spawnedEnemy.enemyGameObject;
            enemyObject.SetActive(true);
            var enemyComponent = enemyObject.GetComponent<IEnemyBase>();
            enemyComponent.IsAlive = true;
            ApplyDifficultyModifications(enemyObject.GetComponent<IEnemyBase>());
            enemyComponent.Spawn();
            return enemyComponent;
        }

        /// <summary>
        /// Calculate multipliers for enemy stats based on current difficulty.
        /// </summary>
        /// <param name="enemy"></param>
        public void ApplyDifficultyModifications(IEnemyBase enemy)
        {
            enemy.CurrencyScaling = DifficultyManager.Instance.DifficultyLevel;
            enemy.DamageScaling = DifficultyManager.Instance.DifficultyLevel;
            enemy.HealthScaling = DifficultyManager.Instance.DifficultyLevel;
        }
    }
}