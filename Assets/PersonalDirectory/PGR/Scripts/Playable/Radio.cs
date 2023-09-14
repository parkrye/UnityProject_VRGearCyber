using System.Collections;
using UnityEngine;

namespace PGR
{
    [RequireComponent(typeof(AudioSource))]
    public class Radio : PlayableObject
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] float volume;

        void Start()
        {
            StartCoroutine(SoundRoutine());
        }

        IEnumerator SoundRoutine()
        {
            yield return new WaitForSeconds(.1f);
            while (true)
            {
                yield return new WaitForSeconds(.1f);
                if (Physics.Raycast(transform.position, GameManager.Data.Player.IrisSystem.transform.position - transform.position, Vector3.Distance(transform.position, GameManager.Data.Player.IrisSystem.transform.position), LayerMask.GetMask("Wall")))
                {
                    audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, 0.1f);
                    continue;
                }

                audioSource.volume = Mathf.Lerp(audioSource.volume, volume, 0.1f);
            }
        }

        public void OnVolumeChanged(float value)
        {
            volume = value;
        }
    }
}