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

        void Start()
        {
            if (maxHP <= 0)
                maxHP = 1;
            nowHP = maxHP;
            texts["SongNameText"].text = songName;
        }

        public void OnVolumeChanged(float value)
        {
            audioSource.volume = value;
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