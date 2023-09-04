using UnityEngine;

namespace PGR
{
    [RequireComponent(typeof(AudioSource))]
    public class Radio : PlayableObject
    {
        [SerializeField] AudioSource audioSource;

        public void OnVolumeChanged(float value)
        {
            audioSource.volume = value;
        }
    }
}