using Entity;
using UnityEngine;

namespace Terrain
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private float damage;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent<TurtleHealth>(out var health)) return;
            health.Damage(damage);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
