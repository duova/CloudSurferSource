using Management;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Update()
        {
            text.text = string.Concat("Score: ", TerrainSpawner.Instance.SectionsTraveled);
        }
    }
}