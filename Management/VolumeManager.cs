using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Management
{
    public class VolumeManager : MonoBehaviour
    {
        public static VolumeManager Instance { get; private set; }

        public int volume;
        
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
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("Volume"))
            {
                volume = PlayerPrefs.GetInt("Volume");
            }
            else
            {
                PlayerPrefs.SetInt("Volume", volume);
            }
        }

        public void Add()
        {
            if (volume < 10) volume++;
            Save();
        }

        public void Minus()
        {
            if (volume > 0) volume--;
            Save();
        }

        private void Save()
        {
            PlayerPrefs.SetInt("Volume", volume);
            PlayerPrefs.Save();
        }
    }
}
