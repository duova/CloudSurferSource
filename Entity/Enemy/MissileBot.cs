using UnityEngine;

namespace Entity.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public class MissileBot : MonoBehaviour, IEnemyBase
    {
        [SerializeField]
        private float baseHealth, baseDamage, baseCooldown, additionalHealthMultiplier, additionalDamageMultiplier, reducedCooldownMultiplier, minCooldown;

        private EnemyHealth _health;
        
        [SerializeField] private int currencyValue;

        [SerializeField]
        private GameObject[] barrelMarkers;

        [SerializeField]
        private GameObject missilePrefab;

        private float _damage, _cooldown, _cooldownTimer;

        public float CurrencyScaling { get; set; }
        public int CurrencyValue => currencyValue;
        public float HealthScaling { get; set; }
        public float DamageScaling { get; set; }
        public bool IsAlive { get; set; }
        public void Spawn()
        {
            print("spawn called");
            _health.CurrentHealth = _health.maxHealth;
            _damage = baseDamage + additionalDamageMultiplier * DamageScaling;
            _cooldown = Mathf.Clamp(baseCooldown - DamageScaling * reducedCooldownMultiplier, minCooldown, baseCooldown);
            _cooldownTimer = _cooldown;
        }

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
            _health.maxHealth = baseHealth + additionalHealthMultiplier * HealthScaling;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _cooldownTimer -= Time.deltaTime;
            //Fire missiles.
            if (!(_cooldownTimer < 0)) return;
            var selectedBarrel = barrelMarkers[Random.Range(0, barrelMarkers.Length)];
            var missile = Instantiate(missilePrefab, selectedBarrel.transform.position,
                selectedBarrel.transform.rotation);
            Destroy(missile, 10f);
            missile.GetComponent<Missile>().Damage = _damage;
            _cooldownTimer = _cooldown;
        }
    }
}