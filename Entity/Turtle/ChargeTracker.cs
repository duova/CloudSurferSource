using System;
using UnityEngine;

namespace Entity.Turtle
{
    public class ChargeTracker : MonoBehaviour
    {
        private int _charge;

        public static ChargeTracker Instance { get; private set; }

        public int Charge
        {
            get => _charge;
            set => _charge = Mathf.Clamp(value, 0, 999999);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Multiple versions of ChargeTracker should not exist");
            }
        }
    }
}