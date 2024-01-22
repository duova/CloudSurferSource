using Management;
using UnityEngine;

namespace Entity.Turtle
{
    [RequireComponent(typeof(Collider))]
    public class TurtleProjectile : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shield"))
            {
                Destroy(gameObject);
            }
            
            if (!other.gameObject.TryGetComponent<EnemyHealth>(out var enemyHealth)) return;
            enemyHealth.Damage(StatManager.Instance.Damage);
            Destroy(gameObject);
        }
    }
}