using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	public class ArrowController : MonoBehaviour
	{
		[Header("Arrow")]
		[SerializeField] private float destroyDelay = 3f;
		[SerializeField] private int damage;
		[SerializeField] private int explosionRadius = 1;
		[SerializeField] private int explosionForce = 100;
		[SerializeField] GameObject explosionEffect;

		[Header("Audio")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip boomSound;

		private MeshRenderer[] meshRenderers;

		private void Start()
		{
			meshRenderers = GetComponentsInChildren<MeshRenderer>();
		}

		public void ArrowExplode()
		{
			foreach (MeshRenderer meshRenderer in meshRenderers)
			{ 
				meshRenderer.enabled = false;
			}

			audioSource.PlayOneShot(boomSound, 1.0f);

			Instantiate(explosionEffect, transform.position, transform.rotation);

			Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
			foreach (Collider nearByObjects in colliders)
			{
				Rigidbody rb = nearByObjects.GetComponent<Rigidbody>();

				if (rb != null)
				{
					rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
				}

				IHitable hitable = nearByObjects.GetComponent<IHitable>();
				if (hitable != null)
				{
					hitable.TakeDamage(damage, Vector3.zero, Vector3.zero);
				}
			}
		}

		public void OnCollisionEnter(Collision collision)
		{
				ArrowExplode();

				StartCoroutine(DestroyWaitRoutine());
		}

		private IEnumerator DestroyWaitRoutine()
		{ 
			yield return new WaitForSeconds(destroyDelay);

			Destroy(gameObject);
		}
	}
}
