using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace KSI
{
	public class ArrowController : MonoBehaviour
	{
		[Header("Arrow")]
		[SerializeField] private int damage;
		[SerializeField] private int force;
		[SerializeField] private Renderer arrowRenderer;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();

			if (arrowRenderer != null)
			{
				arrowRenderer.enabled = false;
			}
		}

		public void FireArrow()
		{
			if (arrowRenderer != null)
			{
				arrowRenderer.enabled = true;
			}

			rb.AddForce(transform.forward * force);
		}
	}
}
