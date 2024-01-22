using Management;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Update()
        {
            text.text = string.Concat("$", CurrencyManager.Instance.Currency);
        }
    }
}
