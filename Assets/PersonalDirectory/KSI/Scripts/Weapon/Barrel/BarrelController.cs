using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID;
using PGR;

namespace KSI
{
	public class BarrelController : MonoBehaviour
	{
		[SerializeField] private GameObject barrelEffect;

		private Transform tr;
		private Rigidbody rb;
		private int shootCount = 0;

		private void Start() 
		{ 
			tr = GetComponent<Transform>();
			rb = GetComponent<Rigidbody>();
		}

        private void OnCollisionEnter(Collision coll)
		{
			if (coll.collider.CompareTag("Bullet"))
			{
				if (++shootCount == 3)
				{
					BarrelHP();
				}
			}
		}

		private void BarrelHP()
		{
			GameObject barrelHP = Instantiate(barrelEffect, tr.position, Quaternion.identity);

			Destroy(barrelHP, 5.0f);

			rb.mass = 1.0f;
			rb.AddForce(Vector3.up * 1500.0f);

			Destroy(gameObject, 3.0f);
		}
	}
}
