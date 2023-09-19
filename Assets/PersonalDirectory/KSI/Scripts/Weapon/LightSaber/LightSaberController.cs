using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	[RequireComponent(typeof(AudioSource))]
	public class LightSaberController : MonoBehaviour
	{
		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip onOffSound;

		private Animator animator;

		private void Start()
		{
			animator = GetComponent<Animator>();

			audioSource = GetComponent<AudioSource>();
		}

		public void LightOnOff()
		{
			animator.SetBool("On", !animator.GetBool("On"));
			audioSource.PlayOneShot(onOffSound, 1.0f);
		}
	}
}