using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGR;

namespace KSI
{
	[RequireComponent(typeof(AudioSource))]
	public class GrenadeController : MonoBehaviour
	{
		[Header("Grenade")]
		[SerializeField] private float delay =3f;
		[SerializeField] private float destoryDelay = 0.1f;
		[SerializeField] private int damage;
		[SerializeField] private int ExplosionRadius = 4;
		[SerializeField] private int ExplosionForce = 400;
		[SerializeField] private GameObject grenadeEffect;
		
		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip boomSound;

		private PlayerHandMotion playerHandMotion;
		private bool isRightHanded;

		public void TriggerBoom()
		{
			StartCoroutine(ExplosionWaitCouRoutine());
		}

		private IEnumerator ExplosionWaitCouRoutine()
		{ 
			yield return new WaitForSeconds(delay);

			GrenadeExplode();

			yield return new WaitForSeconds(destoryDelay);

			Destroy(gameObject);
		}

		public void GrenadeExplode()
		{
			audioSource.PlayOneShot(boomSound, 1.0f);

			Instantiate(grenadeEffect, transform.position, transform.rotation);

			Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
			foreach (Collider nearByObjects in colliders)
			{
				Rigidbody rb = nearByObjects.GetComponent<Rigidbody>();

				if (rb != null)
				{
					rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
				}

				IHitable hitable = nearByObjects.GetComponent<IHitable>();
				if (hitable != null)
				{
					hitable.TakeDamage(damage, Vector3.zero, Vector3.zero);
				}
			}
		}
	}
}