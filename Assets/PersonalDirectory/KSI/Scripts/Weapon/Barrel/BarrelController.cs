using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class BarrelController : MonoBehaviour
	{
		[SerializeField] private GameObject barrelEffect; // 폭발 효과 파티클

		private Transform tr;
		private Rigidbody rb;
		private int shootCount = 0; // 총에 맞은 횟수

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

			// Barrel의 무게를 가볍게 해서 위로 날려버림
			rb.mass = 1.0f;
			rb.AddForce(Vector3.up * 1500.0f);

			Destroy(gameObject, 3.0f);
		}
	}
}
