using System.Collections.Generic;
using Entity.Turtle;
using UnityEngine;

namespace Entity.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public class Ufo : MonoBehaviour, IEnemyBase
    {
        [SerializeField]
        private float baseHealth, baseDamage, baseCooldown, additionalHealthMultiplier, additionalDamageMultiplier, reducedCooldownMultiplier, minCooldown, activeTime, simulatedLagDuration;

        private EnemyHealth _health;

        private Laser _laserComp;
        
        [SerializeField] private int currencyValue;

        [SerializeField]
        private GameObject laser, indicator;

        private float _damage, _cooldown, _cooldownTimer, _activeTimer;

        private Queue<Vector3> _targets = new();

        private Vector3 _target;
        
        private MovementController _movementController;

        private GameObject _turtle;

        public float CurrencyScaling { get; set; }
        public int CurrencyValue => currencyValue;
        public float HealthScaling { get; set; }
        public float DamageScaling { get; set; }
        public bool IsAlive { get; set; }
        public void Spawn()
        {
            _health.CurrentHealth = _health.maxHealth;
            _damage = baseDamage + additionalDamageMultiplier * DamageScaling;
            _cooldown = Mathf.Clamp(baseCooldown - DamageScaling * reducedCooldownMultiplier, minCooldown, baseCooldown);
            _cooldownTimer = _cooldown;
            _activeTimer = 0;
        }

        private void Awake()
        {
            _laserComp = laser.GetComponent<Laser>();
            _health = GetComponent<EnemyHealth>();
            _health.maxHealth = baseHealth + additionalDamageMultiplier * HealthScaling;
            _laserComp.Damage = _damage;
            laser.SetActive(false);
            indicator.SetActive(false);
            gameObject.SetActive(false);
        }

        private void Start()
        {
            _turtle = TurtleHealth.Instance.gameObject;
            _movementController = _turtle.GetComponent<MovementController>();
        }
        
        private void FixedUpdate()
        {
            _targets.Enqueue(_turtle.transform.position + _movementController.TravelDirection * (_movementController.moveSpeed * simulatedLagDuration));
        }

        //Local space tracking with delay with an indicator that is enabled before firing.
        private void Update()
        {
            if (_targets.Count >= (int)(simulatedLagDuration / Time.fixedDeltaTime))
            {
                _target = _targets.Dequeue();
            }
            
            var targetDirection = Vector3.Normalize(_target - transform.position);
            var newDirection =
                Vector3.RotateTowards(transform.forward, targetDirection, 100f, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
            
            _cooldownTimer -= Time.deltaTime;
            _activeTimer -= Time.deltaTime;
            //Fire laser.
            if (_cooldownTimer < 1f) indicator.SetActive(true);
            if (_cooldownTimer < 0)
            {
                _activeTimer = activeTime;
                _laserComp.Damage = _damage;
                _cooldownTimer = _cooldown;
                indicator.SetActive(false);
            }

            if (_activeTimer > 0)
            {
                laser.SetActive(true);
            }
            else
            {
                laser.SetActive(false);
            }
        }
    }
}