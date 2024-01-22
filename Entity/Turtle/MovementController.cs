using Terrain;
using UnityEngine;

namespace Entity.Turtle
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField] public float moveSpeed, gravity, rotateSpeed;

        private Rigidbody _rb;
        [SerializeField]
        private Vector3 travelDirection;
        [SerializeField]
        private Vector3 gravityDirection;
        private Vector3 _inputVelocity;

        [SerializeField]
        private float horizontalSpeed;

        private Quaternion _targetRot;

        private float _jumpTimer, _jumpCooldownTimer;

        public Vector3 TravelDirection
        {
            get => travelDirection;
            set => travelDirection = value;
        }

        public Vector3 GravityDirection
        {
            get => gravityDirection;
            private set => gravityDirection = value;
        }

        private void Start()
        {
            GravityDirection = Vector3.down;
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _inputVelocity = Vector3.zero;
            if (Input.GetKey(KeyCode.A))
            {
                _inputVelocity -= transform.right * horizontalSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _inputVelocity += transform.right * horizontalSpeed;
            }

            ConstantMovement(_inputVelocity);
        }

        private void ConstantMovement(Vector3 inputVelocity)
        {
            var cachedTransform = transform;
            Physics.Raycast(new Ray(cachedTransform.position, -cachedTransform.up), out var hit, 2f);
            if (hit.collider && hit.collider.gameObject.TryGetComponent<Floor>(out var floor))
            {
                TravelDirection = floor.Heading;
                GravityDirection = floor.GravityDirection;
            }
            _rb.velocity = (TravelDirection * moveSpeed) + (GravityDirection * gravity) + inputVelocity;
        
            _targetRot = Quaternion.LookRotation(TravelDirection, -GravityDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, 0.05f);
        }
    }
}
