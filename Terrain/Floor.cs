using UnityEngine;

namespace Terrain
{
    public class Floor : MonoBehaviour
    {
        public Vector3 Heading { get; private set; }
        public Vector3 GravityDirection { get; private set; }
        public Transform CachedTransform { get; private set; }

        private void Awake()
        {
            CachedTransform = transform;
        }

        private void Update()
        {
            Heading = CachedTransform.forward;
            GravityDirection = -CachedTransform.up;
        }
    }
}
