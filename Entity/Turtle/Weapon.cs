using Unity.VisualScripting;
using UnityEngine;

namespace Entity.Turtle
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private GameObject projectile, firePoint, turtle;

        [SerializeField]
        private float projectileVelocity;

        [SerializeField]
        private GameObject tailTarget;

        private Transform _cachedTransform;
        private Transform _cachedTailTargetTransform;
        private LayerMask _notTurtleMask;
        private ChargeTracker _chargeTracker;

        private void Start()
        {
            _cachedTransform = transform;
            _notTurtleMask = ~LayerMask.GetMask("Turtle");
            _cachedTailTargetTransform = tailTarget.transform;
            _chargeTracker = turtle.GetComponent<ChargeTracker>();
        }

        private void Update()
        {
            _cachedTailTargetTransform.position = _cachedTransform.position + _cachedTransform.forward * 100f;
            if (Input.GetKeyDown(KeyCode.Mouse0)) Fire();
        }
    
        public void Fire()
        {
            if (_chargeTracker.Charge <= 0) return;
            Vector3 direction;
            //Play firing effects.
            direction = Physics.Raycast(_cachedTransform.position, _cachedTransform.forward, out var hit, 100f)
                ? (hit.point - firePoint.transform.position).normalized
                : _cachedTransform.forward;

            var instance = Instantiate(projectile, firePoint.transform.position,
                Quaternion.LookRotation(direction));
            instance.GetComponent<Rigidbody>().velocity = direction * projectileVelocity;
            Destroy(instance, 6f);
            _chargeTracker.Charge--;
        }
    }
}
