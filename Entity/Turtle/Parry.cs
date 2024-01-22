using System;
using Entity.Enemy;
using Management;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Entity.Turtle
{
    public class Parry : MonoBehaviour
    {
        [SerializeField] private GameObject modelObject, vfxObject;

        [SerializeField]
        private AudioSource audioSource;
        
        private SkinnedMeshRenderer _renderer;

        private VisualEffect _visualEffect;

        private float _parryTime, _parryCooldown;

        [SerializeField] private float durationOfParry;

        [SerializeField] private ChargeTracker chargeTracker;
    
        private void Start()
        {
            _renderer = modelObject.GetComponent<SkinnedMeshRenderer>();
            _visualEffect = vfxObject.GetComponent<VisualEffect>();
        }
    
        private void Update()
        {
            _parryTime -= Time.deltaTime;
            _parryCooldown -= Time.deltaTime;
            if (_parryTime > 0f) return;
            if (!_renderer.enabled) _renderer.enabled = true;
            if (!Input.GetKeyDown(KeyCode.Mouse1)) return;
            if (_parryCooldown > 0f) return;
            _parryTime = durationOfParry;
            _parryCooldown = StatManager.Instance.ParryCooldown;
            _visualEffect.Play();
            _renderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!(_parryTime > 0f)) return;
            
            if (other.gameObject.TryGetComponent<IProjectile>(out _))
            {
                chargeTracker.Charge++;
                Destroy(other.gameObject);
                audioSource.Play();
                
            }
            else if (other.gameObject.TryGetComponent<SlamObject>(out var slamObject))
            {
                chargeTracker.Charge++;
                slamObject.SlamTimer = -0.1f;
                audioSource.Play();
            }
        }
    }
}
