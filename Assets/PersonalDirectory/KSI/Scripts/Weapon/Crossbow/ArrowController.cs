using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	public class ArrowController : MonoBehaviour
	{
		[Header("Arrow")]
		[SerializeField] private Renderer arrowRenderer;
		[SerializeField] int damage;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();

			if (arrowRenderer != null)
			{
				arrowRenderer.enabled = false;
			}
		}

		public void FireArrow(float force, int _damage)
		{
			gameObject.SetActive(false);
			gameObject.SetActive(true);
			if (arrowRenderer != null)
			{
				arrowRenderer.enabled = true;
			}
			
			damage = _damage;
			rb.AddForce(transform.forward * force, ForceMode.VelocityChange);
		}

		void OnCollisionEnter(Collision collision)
		{
			IHitable hitable = collision.transform.GetComponent<IHitable>();
			hitable?.TakeDamage(damage, collision.contacts[0].point, collision.contacts[0].normal);
		}
	}
}
