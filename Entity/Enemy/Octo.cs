using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public class Octo : MonoBehaviour, IEnemyBase
    {
        [SerializeField]
        private float baseHealth, baseCooldown, additionalHealthMultiplier, reducedCooldownMultiplier, minCooldown, inkVelocity;

        private EnemyHealth _health;
        
        [SerializeField] private int currencyValue;

        [SerializeField]
        private GameObject inkPrefab, shootLocationMarker;
        
        private GameObject _turtle;

        private float _damage, _cooldown, _cooldownTimer;

        private Vector3 _cachedRelativeLocation;

        public float CurrencyScaling { get; set; }
        public int CurrencyValue => currencyValue;
        public float HealthScaling { get; set; }
        public float DamageScaling { get; set; }
        public bool IsAlive { get; set; }
        public void Spawn()
        {
            _health.CurrentHealth = _health.maxHealth;
            _cooldown = Mathf.Clamp(baseCooldown - DamageScaling * reducedCooldownMultiplier, minCooldown, baseCooldown);
            _cooldownTimer = _cooldown;
        }

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
            _health.maxHealth = baseHealth + additionalHealthMultiplier * HealthScaling;
            _cachedRelativeLocation = transform.localPosition;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _turtle = TurtleHealth.Instance.gameObject;
        }

        private void Update()
        {
            transform.rotation = Quaternion.identity;
            transform.position = _turtle.transform.position + _cachedRelativeLocation;
            _cooldownTimer -= Time.deltaTime;
            //Fire ink.

            transform.localScale = _cooldownTimer switch
            {
                < 0.5f and > 0.2f => new Vector3(transform.localScale.x, 0.5f, transform.localScale.z),
                < 0.6f and > 0.1f => new Vector3(transform.localScale.x, 0.75f, transform.localScale.z),
                _ => new Vector3(transform.localScale.x, 1f, transform.localScale.z)
            };

            if (!(_cooldownTimer < 0)) return;
            var ink = Instantiate(inkPrefab, shootLocationMarker.transform.position,
                shootLocationMarker.transform.rotation);
            Destroy(ink, 7f);
            ink.GetComponent<Rigidbody>().velocity =
                _turtle.GetComponent<Rigidbody>().velocity + Vector3.down * inkVelocity;
            _cooldownTimer = _cooldown;
        }
    }
}