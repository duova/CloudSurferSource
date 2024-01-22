using System;
using Management;
using UnityEngine;

namespace Entity.Enemy
{
    public class SlamObject : MonoBehaviour
    {
        private MeshRenderer _missileBossRenderer;

        private MeshRenderer _renderer;

        private Collider _collider;

        public static SlamObject Instance { get; private set; }

        [SerializeField] private float slamDuration;

        public float SlamTimer { get; set; }

        private Vector3 _origin;

        public float Damage { get; set; }

        private void Awake()
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
            _collider = GetComponent<Collider>();
            _origin = transform.localPosition;
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of SlamObject should not exist");
            }
            SlamTimer = -1f;
            _renderer.enabled = false;
            _collider.enabled = false;
        }

        private void Update()
        {
            if (!TerrainSpawner.Instance.OnBossLevel)
            {
                _renderer.enabled = false;
                _collider.enabled = false;
                return;
            }
            
            if (SlamTimer is < 0 and > -0.2f)
            {
                _missileBossRenderer.enabled = true;
                _renderer.enabled = false;
                _collider.enabled = false;
            }

            SlamTimer -= Time.deltaTime;
            transform.localPosition = Vector3.Lerp(Vector3.zero, _origin, SlamTimer / slamDuration);
        }

        public void Slam(MeshRenderer bossRenderer)
        {
            _missileBossRenderer = bossRenderer;
            _missileBossRenderer.enabled = false;
            _renderer.enabled = true;
            _collider.enabled = true;
            SlamTimer = slamDuration;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if ((!other.gameObject.TryGetComponent<TurtleHealth>(out var health)) || !_renderer.enabled) return;
            health.Damage(Damage);
        }
    }
}