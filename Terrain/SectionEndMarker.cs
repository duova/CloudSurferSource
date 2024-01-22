using Management;
using UnityEngine;

namespace Terrain
{
    [RequireComponent(typeof(Collider))]
    public class SectionEndMarker : MonoBehaviour
    {
        public int SectionIndex { get; set; }
        private Collider _collider;
        private bool _sectionFinished;

        private void Start()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 3) return;
            if (_sectionFinished) return;
            TerrainSpawner.Instance.SectionsTraveled++;
            TerrainSpawner.Instance.BossHeatRaw++;
            _sectionFinished = true;
        }
    }
}
