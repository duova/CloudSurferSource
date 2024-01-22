using Management;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ShieldDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Update()
        {
            text.text = string.Concat("Shields: ", StatManager.Instance.Shield);
        }
    }
}