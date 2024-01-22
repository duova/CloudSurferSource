using UnityEngine;

namespace Entity.Enemy
{
    public class Laser : MonoBehaviour
    {
        public float Damage { get; set; }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent<TurtleHealth>(out var health))
            {
                health.Damage(Damage * Time.deltaTime);
            }
        }

        public void Draw(Vector3 a, Vector3 b)
        {
            transform.localScale = new Vector3(transform.localScale.x, (a - b).magnitude / 2, transform.localScale.z);
            transform.rotation =
                Quaternion.LookRotation(Vector3.Cross((a - b).normalized, Vector3.forward), (a - b).normalized);
            transform.position = Vector3.Lerp(a, b, 0.5f);
        }
    }
}