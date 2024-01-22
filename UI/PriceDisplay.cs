using Management;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PriceDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Update()
        {
            text.text = string.Concat("Price: $", CurrencyManager.Instance.upgradePointValue);
        }
    }
}