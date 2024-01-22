using System;
using Management;
using UnityEngine;

    public class VolumeController: MonoBehaviour
    {
        private AudioSource _audioSource;
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            _audioSource.volume = VolumeManager.Instance.volume * 0.1f;
        }
    }