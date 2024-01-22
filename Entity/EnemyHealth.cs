using Entity.Enemy;
using Management;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Entity
{
    public class EnemyHealth : Health
    {
        private IEnemyBase _enemyComponent;
        [SerializeField] private GameObject explosionPrefab;
        
        protected override void Start()
        {
            base.Start();
            _enemyComponent = GetComponent<IEnemyBase>();
        }

        protected override void OnDeath()
        {
            _enemyComponent.IsAlive = false;
            CurrencyManager.Instance.Currency += (int)(_enemyComponent.CurrencyScaling * _enemyComponent.CurrencyValue);
            CurrentHealth = maxHealth;
            if (TryGetComponent<MissileBoss>(out var boss))
            {
                Destroy(boss.laser0Comp.gameObject);
                Destroy(boss.laser1Comp.gameObject);
            }
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            if (TryGetComponent<MissileBoss>(out _))
            {
                TerrainSpawner.Instance.OnBossLevel = false;
                Destroy(gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}