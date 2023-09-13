using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class BulletController : MonoBehaviour
	{
		[SerializeField] private float damage;
		[SerializeField] private float force;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * force);
		}


	}
}