using UnityEngine;

namespace PGR
{
    public class Radio : PlayableObject
    {
        [SerializeField] float volume;
        [SerializeField] AudioSource audioSource;

        void Start()
        {
            volume = audioSource.volume;
        }

        public void OnVolumeChanged(float value)
        {
            audioSource.volume = value;
        }
    }
}