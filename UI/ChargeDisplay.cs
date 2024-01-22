using Entity.Turtle;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ChargeDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Update()
        {
            text.text = string.Concat("Charges: ", ChargeTracker.Instance.Charge);
        }
    }
}