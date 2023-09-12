using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	[RequireComponent(typeof(AudioSource))]
	public class FlashLightController : MonoBehaviour
	{
		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip onOffSound;

		private Light spotLight;

		private void Start()
		{
			spotLight = GetComponentInChildren<Light>();
			audioSource = spotLight.GetComponent<AudioSource>();
		}

		public void LightOn()
		{
			audioSource.PlayOneShot(onOffSound, 1.0f);
			spotLight.enabled = true;
		}

		public void LightOff()
		{
			audioSource.PlayOneShot(onOffSound, 1.0f);
			spotLight.enabled = false;
		}
	}
}
