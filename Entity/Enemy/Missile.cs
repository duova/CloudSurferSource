using System.Collections.Generic;
using Entity.Turtle;
using Terrain;
using UnityEngine;

namespace Entity.Enemy
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Missile : MonoBehaviour, IProjectile
    {
        public float Damage { get; set; }

        private Queue<Vector3> _targets = new();

        private Vector3 _target;

        private MovementController _movementController;

        private Rigidbody _rigidbody;

        [SerializeField] private float velocity, turnRateRadians, simulatedLagDuration;

        [SerializeField] private GameObject explosionPrefab;

        private GameObject _turtle;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _turtle = TurtleHealth.Instance.gameObject;
            _movementController = _turtle.GetComponent<MovementController>();
            _target = _turtle.transform.position;
        }
        
        private void FixedUpdate()
        {
            _targets.Enqueue(_turtle.transform.position + _movementController.TravelDirection * (_movementController.moveSpeed * simulatedLagDuration)); //
        }

        private void Update()
        {
            if (_targets.Count >= (int)(simulatedLagDuration / Time.fixedDeltaTime))
            {
                _target = _targets.Dequeue();
            }

            //Lerp the rotation of the missile towards the predicted target.
            var targetDirection = Vector3.Normalize(_target - transform.position);
            var newDirection =
                Vector3.RotateTowards(transform.forward, targetDirection, turnRateRadians * Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
            
            //Apply velocity.
            _rigidbody.velocity = transform.forward * velocity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<TurtleHealth>(out var health))
            {
                health.Damage(Damage);
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            if (!other.gameObject.TryGetComponent<Floor>(out _)) return;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}