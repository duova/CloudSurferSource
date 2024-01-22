using System;
using Entity.Turtle;
using Unity.Mathematics;
using UnityEngine;

namespace Entity.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public class Jet : MonoBehaviour, IEnemyBase
    {
         [SerializeField]
        private float baseHealth, baseDamage, additionalHealthMultiplier, additionalDamageMultiplier, travelDistance, travelTime, dropCooldown;

        [SerializeField] private int currencyValue;

        [SerializeField] private Vector3 relativeSpawnPosition;

        private EnemyHealth _health;

        private Bomb _bombComp;

        [SerializeField]
        private GameObject turtle, bombPrefab, bombBayMarker;

        private float _damage, _dropTimer, _travelTimer;

        public float CurrencyScaling { get; set; }
        public int CurrencyValue => currencyValue;
        public float HealthScaling { get; set; }
        public float DamageScaling { get; set; }
        public bool IsAlive { get; set; }
        public void Spawn()
        {
            _health.CurrentHealth = _health.maxHealth;
            _damage = baseDamage + additionalDamageMultiplier * DamageScaling;
            transform.localPosition = relativeSpawnPosition;
            _travelTimer = 0;
            _dropTimer = dropCooldown;
        }

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();
            _health.maxHealth = baseHealth + additionalDamageMultiplier * HealthScaling;
            gameObject.SetActive(false);
        }

        private void Start()
        {
            turtle = TurtleHealth.Instance.gameObject;
        }

        //Travel forwards and drop bombs.
        private void Update()
        {
            _travelTimer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(relativeSpawnPosition, (new Vector3(0, relativeSpawnPosition.y, 0) - relativeSpawnPosition).normalized * travelDistance + relativeSpawnPosition, _travelTimer/travelTime);
            if (_travelTimer >= travelTime)
            {
                IsAlive = false;
                gameObject.SetActive(false);
            }

            _dropTimer -= Time.deltaTime;
            if (_dropTimer < 0)
            {
                var bomb = Instantiate(bombPrefab, bombBayMarker.transform.position, bombBayMarker.transform.rotation);
                bomb.GetComponent<Bomb>().Damage = _damage;
                _dropTimer = dropCooldown;
                Destroy(bomb, 7f);
            }
        }
    }
}