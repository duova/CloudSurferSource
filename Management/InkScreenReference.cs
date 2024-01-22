using System;
using UnityEngine;

namespace Management
{
    public class InkScreenReference : MonoBehaviour
    {
        public static GameObject GameObject { get; private set; }

        private void Awake()
        {
            if (GameObject == null)
            {
                GameObject = gameObject;
            }
            else
            {
                throw new Exception("Multiple versions of InkScreenReference should not exist");
            }
        }
    }
}