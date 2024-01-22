using Management;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PointDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private string prefix, key;
        private void Update()
        {
            if (PlayerPrefs.HasKey(key))
            {
                text.text = string.Concat(prefix, PlayerPrefs.GetInt(key));
            }
            else
            {
                PlayerPrefs.SetInt(key, 0);
            }
        }
    }
}
