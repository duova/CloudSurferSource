using System;
using UnityEngine;

namespace Management
{
    public class StatManager : MonoBehaviour
    {
        public static StatManager Instance { get; private set; }

        [SerializeField] private float baseDamage, baseHealth, baseParryCooldown, damagePerLevel, healthPerLevel, parryCooldownReductionPerLevel;

        public float Damage { get; set; }

        public float Health { get; set; }

        public float ParryCooldown { get; set; }

        public int Shield { get; set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of StatManager should not exist");
            }

            if (PlayerPrefs.HasKey("Damage"))
            {
                Damage = baseDamage + damagePerLevel * PlayerPrefs.GetInt("Damage");
            }
            else
            {
                Damage = baseDamage;
                PlayerPrefs.SetInt("Damage", 0);
                PlayerPrefs.Save();
            }
            
            if (PlayerPrefs.HasKey("Health"))
            {
                Health = baseHealth + healthPerLevel * PlayerPrefs.GetInt("Health");
            }
            else
            {
                Health = baseHealth;
                PlayerPrefs.SetInt("Health", 0);
                PlayerPrefs.Save();
            }
            
            if (PlayerPrefs.HasKey("Parry"))
            {
                ParryCooldown = baseParryCooldown - parryCooldownReductionPerLevel * PlayerPrefs.GetInt("Parry");
            }
            else
            {
                ParryCooldown = baseParryCooldown;
                PlayerPrefs.SetInt("Parry", 0);
                PlayerPrefs.Save();
            }
            
            if (PlayerPrefs.HasKey("Shield"))
            {
                Shield = PlayerPrefs.GetInt("Shield");
            }
            else
            {
                Shield = 0;
                PlayerPrefs.SetInt("Shield", 0);
                PlayerPrefs.Save();
            }
        }
    }
}