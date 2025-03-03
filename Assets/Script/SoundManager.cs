using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource[] soundEffects;
        public static SoundManager Instance;

        public float[] OriginalVolumes; // Lưu volume ban đầu của soundEffects

        public bool IsSound = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            OriginalVolumes = new float[soundEffects.Length];
            for (int i = 0; i < soundEffects.Length; i++)
            {
                OriginalVolumes[i] = soundEffects[i].volume;
            }
        }

        public void PlaySFX(int sfxNumber)
        {
            if (IsSound && sfxNumber >= 0 && sfxNumber < soundEffects.Length)
            {
                soundEffects[sfxNumber].Play();
            }
        }

        public void StopSFX(int sfxNumber)
        {
            if (sfxNumber >= 0 && sfxNumber < soundEffects.Length)
            {
                soundEffects[sfxNumber].Stop();
            }
        }
    }
}
