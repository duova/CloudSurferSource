using Entity.Turtle;
using UnityEngine;

namespace Terrain
{
    [RequireComponent(typeof(Collider))]
    public class Balloon : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent<ChargeTracker>(out var chargeTracker)) return;
            chargeTracker.Charge++;
            Destroy(gameObject);
        }
    }
}