using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class BulletController : MonoBehaviour
	{
		[SerializeField] private float damage;
		[SerializeField] private float force;
		[SerializeField] private GameObject bulletSparkEffect;

		private Rigidbody rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * force);
			StartCoroutine(DestroyBulltRoutine(5f)); // 5초 후에 총알 제거
		}

		private IEnumerator DestroyBulltRoutine(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			Destroy(gameObject);
		}

		private void OnCollisionEnter(Collision coll)
		{
			if (coll.collider.CompareTag("Player") || coll.collider.CompareTag("Gun") || coll.collider.CompareTag("Bullet"))
			{
				return;
			}

			// 첫 번째 충돌 지점 정보 추출
			ContactPoint contactPoint = coll.GetContact(0);
			// 충돌한 총알의 법선 벡터를 쿼터니언 타입으로 변환
			Quaternion rotation = Quaternion.LookRotation(-contactPoint.normal);

			GameObject spark = Instantiate(bulletSparkEffect, contactPoint.point, rotation);
			Destroy(spark, 1f);
			Destroy(gameObject);
		}
	}
}