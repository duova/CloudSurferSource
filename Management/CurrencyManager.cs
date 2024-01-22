using System;
using UnityEngine;

namespace Management
{
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance { get; private set; }

        private int _currency;
        
        public int Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                PlayerPrefs.SetInt("Currency", value);
                PlayerPrefs.Save();
            }
        }

        [SerializeField] private int maxHealth, maxDamage, maxParry, maxShield;

        public int upgradePointValue;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of CurrencyManager should not exist");
            }

            if (PlayerPrefs.HasKey("Currency"))
            {
                Currency = PlayerPrefs.GetInt("Currency");
            }
            else
            {
                PlayerPrefs.SetInt("Currency", 0);
                PlayerPrefs.Save();
            }
        }

        private void Buy(string key, int max)
        {
            while (true)
            {
                if (PlayerPrefs.HasKey(key))
                {
                    var current = PlayerPrefs.GetInt(key);
                    if (current >= max || Currency < upgradePointValue) return;
                    PlayerPrefs.SetInt(key, current + 1);
                    PlayerPrefs.Save();
                    Currency -= upgradePointValue;
                }
                else
                {
                    PlayerPrefs.SetInt(key, 0);
                    PlayerPrefs.Save();
                    continue;
                }

                break;
            }
        }

        public void BuyHealth()
        {
            Buy("Health", maxHealth);
        }
        
        public void BuyDamage()
        {
            Buy("Damage", maxDamage);
        }
        
        public void BuyParry()
        {
            Buy("Parry", maxParry);
        }
        
        public void BuyShield()
        {
            Buy("Shield", maxShield);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Z) &&
                Input.GetKey(KeyCode.P)) Currency += 100;
            
            if (!Input.GetKey(KeyCode.Q) || !Input.GetKey(KeyCode.P) || !Input.GetKey(KeyCode.Y)) return;
            PlayerPrefs.SetInt("Damage", 0);
            PlayerPrefs.SetInt("Health", 0);
            PlayerPrefs.SetInt("Parry", 0);
            PlayerPrefs.SetInt("Shield", 0);
            PlayerPrefs.Save();
            Currency = 0;
        }
    }
}