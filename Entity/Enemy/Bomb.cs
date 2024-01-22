using Terrain;
using UnityEngine;

namespace Entity.Enemy
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Bomb : MonoBehaviour, IProjectile
    {
        public float Damage { get; set; }
        
        [SerializeField] private GameObject explosionPrefab;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<TurtleHealth>(out var health))
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                health.Damage(Damage);
                Destroy(gameObject);
            }
            if (other.gameObject.TryGetComponent<Floor>(out _))
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}