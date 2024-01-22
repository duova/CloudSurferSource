using Terrain;
using UnityEngine;

namespace Entity
{
    public abstract class Health : MonoBehaviour
    {
        [HideInInspector]
        public float maxHealth;
        public float CurrentHealth { get; set; }

        protected virtual void Start()
        {
            CurrentHealth = maxHealth;
        }

        protected virtual void Update()
        {
            if (CurrentHealth <= 0f)
            {
                OnDeath();
            }
        }
    
        public virtual void Damage(float value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0, maxHealth);
        }

        public virtual void Heal(float value)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
        }

        protected abstract void OnDeath();
    }
}