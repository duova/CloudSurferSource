using System;
using Management;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.Enemy
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Ink : MonoBehaviour, IProjectile
    {
        private GameObject _inkOverlay;

        private Image _inkImage;

        private MeshRenderer _renderer;

        private bool _hit;

        [SerializeField]
        private float inkTime;
        
        private float _inkTimer;

        private void Start()
        {
            _inkOverlay = InkScreenReference.GameObject;
            _inkImage = _inkOverlay.GetComponent<Image>();
            _renderer = GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent<TurtleHealth>(out _)) return;
            _inkImage.enabled = true;
            _hit = true;
        }

        private void Update()
        {
            if (_hit)
            {
                _inkTimer += Time.deltaTime;
            }

            if (_inkTimer <= inkTime) return;
            _inkImage.enabled = false;
            Destroy(gameObject);
        }
    }
}