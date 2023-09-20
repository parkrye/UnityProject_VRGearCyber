using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{
    [RequireComponent(typeof(AudioSource))]
    public class HologramRadio : SceneUI, IHitable
    {
        [SerializeField] int maxHP, nowHP;
        [SerializeField] string songName;
        [SerializeField] AudioSource audioSource;
        [SerializeField] float volume;

        void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
            if (maxHP <= 0)
                maxHP = 1;
            nowHP = maxHP;
            texts["SongNameText"].text = songName;
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

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            nowHP -= damage;
            if(nowHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}