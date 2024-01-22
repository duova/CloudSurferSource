using Entity;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Update()
        {
            text.text = string.Concat("Health: ", (int)TurtleHealth.Instance.CurrentHealth);
        }
    }
}
