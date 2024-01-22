using System;
using Entity.Turtle;
using Management;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entity.Enemy
{
    public enum MissileBossAbilities
    {
        Barrage,
        Laser,
        Slam
    }
    
    [RequireComponent(typeof(EnemyHealth))]
    public class MissileBoss : MonoBehaviour, IEnemyBase
    {
        //Missile barrage, laser, slam, and rotating shields.
        [SerializeField]
        private float baseHealth, baseMissileDamage, baseCooldown, additionalHealthMultiplier, additionalMissileDamageMultiplier, baseSlamDamage, additionalSlamDamageMultiplier, baseLaserDamage, additionalLaserDamageMultiplier, reducedCooldownMultiplier, minCooldown, missileRate, engagementRange;

        private EnemyHealth _health;
        
        [SerializeField] private int currencyValue, missileCount;

        [SerializeField]
        private GameObject[] barrelMarkers, laserEyes, lasers;

        [SerializeField]
        private GameObject missilePrefab;

        [SerializeField]
        private Vector3 laserOffset;

        [SerializeField]
        private AudioSource bossAudioSource;
        
        private GameObject _turtle;

        private SlamObject _slamObject;

        private float _missileDamage, _slamDamage, _laserDamage, _cooldown, _cooldownTimer, _barrageTimer, _laserTimer;

        private int _barrageCounter;

        private readonly Vector3[] _laserTargets = new Vector3[2];
        
        private MovementController _movementController;
        public Laser laser0Comp;
        public Laser laser1Comp;

        public float CurrencyScaling { get; set; }
        public int CurrencyValue => currencyValue;
        public float HealthScaling { get; set; }
        public float DamageScaling { get; set; }
        public bool IsAlive { get; set; }
        public void Spawn()
        {
            _health.CurrentHealth = _health.maxHealth;
            _missileDamage = baseMissileDamage + additionalMissileDamageMultiplier * DamageScaling;
            _laserDamage = baseLaserDamage + additionalLaserDamageMultiplier * DamageScaling;
            _slamDamage = baseSlamDamage + additionalSlamDamageMultiplier * DamageScaling;
            _cooldown = Mathf.Clamp(baseCooldown - DamageScaling * reducedCooldownMultiplier, minCooldown, baseCooldown);
            _cooldownTimer = _cooldown;
            _laserTimer = 0;
            _laserTargets[0] = Vector3.zero;
            _laserTargets[1] = Vector3.zero;
        }

        private void Start()
        {
            _turtle = TurtleHealth.Instance.gameObject;
            _movementController = _turtle.GetComponent<MovementController>();
            laser1Comp = lasers[1].GetComponent<Laser>();
            laser0Comp = lasers[0].GetComponent<Laser>();
            _slamObject = SlamObject.Instance;
            _health = GetComponent<EnemyHealth>();
            _health.maxHealth = baseHealth + additionalHealthMultiplier * HealthScaling;
            lasers[0].SetActive(false);
            lasers[1].SetActive(false);
            laserEyes[0].SetActive(false);
            laserEyes[1].SetActive(false);
            lasers[0].transform.parent = null;
            lasers[1].transform.parent = null;
            EnemySpawner.Instance.ApplyDifficultyModifications(this);
            Spawn();
        }

        private void Update()
        {
            if ((_turtle.transform.position - transform.position).sqrMagnitude >
                engagementRange * engagementRange || _turtle.transform.position.y > transform.position.y + 20 || _turtle.transform.position.y < transform.position.y - 20) return;
            if (!bossAudioSource.isPlaying) bossAudioSource.Play();
            transform.rotation = Quaternion.LookRotation((_turtle.transform.position - transform.position).normalized);
            _cooldownTimer -= Time.deltaTime;
            _barrageTimer -= Time.deltaTime;
            _laserTimer -= Time.deltaTime;

            if (_cooldownTimer < 0)
            {
                var nextAbility =
                    (MissileBossAbilities)Enum.GetValues(typeof(MissileBossAbilities)).GetValue(Random.Range(0, 3));
                switch (nextAbility)
                {
                    case MissileBossAbilities.Barrage:
                    {
                        _barrageCounter = missileCount;
                        break;
                    }
                    case MissileBossAbilities.Laser:
                        _laserTimer = 2.5f;
                        break;
                    case MissileBossAbilities.Slam:
                        _slamObject.Damage = _slamDamage;
                        _slamObject.Slam(GetComponentInChildren<MeshRenderer>());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _cooldownTimer = _cooldown;
            }

            switch (_laserTimer)
            {
                case > 2f and < 2.5f:
                    laserEyes[0].SetActive(true);
                    laserEyes[1].SetActive(true);
                    break;
                case > 1.5f and < 2f:
                    if (_laserTargets[0] == Vector3.zero)
                    {
                        _laserTargets[0] = _turtle.transform.position + _turtle.GetComponent<Rigidbody>().velocity * 0.5f + laserOffset;
                    }
                    break;
                case > 1f and < 1.5f:
                    if (lasers[0].activeSelf == false)
                    {
                        lasers[0].SetActive(true);
                        laser0Comp.Damage = _laserDamage;
                        laser0Comp.Draw(laserEyes[0].transform.position, _laserTargets[0]);
                    }
                    break;
                case > 0.5f and < 1f:
                    lasers[0].SetActive(false);
                    if (_laserTargets[1] == Vector3.zero)
                    {
                        _laserTargets[1] = _turtle.transform.position + _turtle.GetComponent<Rigidbody>().velocity * 0.5f + laserOffset;
                    }
                    break;
                case > 0f and < 0.5f:
                    if (lasers[1].activeSelf == false)
                    {
                        lasers[1].SetActive(true);
                        laser1Comp.Damage = _laserDamage;
                        laser1Comp.Draw(laserEyes[1].transform.position, _laserTargets[1]);
                    }
                    break;
                case < 0f:
                    lasers[1].SetActive(false);
                    laserEyes[0].SetActive(false);
                    laserEyes[1].SetActive(false);
                    _laserTargets[0] = Vector3.zero;
                    _laserTargets[1] = Vector3.zero;
                    break;
            }
            
            
            if (_barrageCounter <= 0) return;
            if (_barrageTimer > 0) return;
            var selectedBarrel = barrelMarkers[Random.Range(0, barrelMarkers.Length)];
            var missile = Instantiate(missilePrefab, selectedBarrel.transform.position,
                selectedBarrel.transform.rotation);
            Destroy(missile, 10f);
            missile.GetComponent<Missile>().Damage = _missileDamage;
            _barrageTimer = missileRate;
            _barrageCounter--;
        }
    }
}