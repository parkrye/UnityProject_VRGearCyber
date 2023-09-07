using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class BulletRemove : MonoBehaviour
	{
		[SerializeField] private GameObject bulletSparkEffect;

		private void OnCollisionEnter(Collision coll)
		{
			if (coll.collider.CompareTag("Bullet"))
			{
				// 첫 번째 충돌 지점 정보 추출
				ContactPoint contactPoint = coll.GetContact(0);
				// 충돌한 총알의 법선 벡터를 쿼터니언 타입으로 변환
				Quaternion rotation = Quaternion.LookRotation(-contactPoint.normal);

				GameObject spark = Instantiate(bulletSparkEffect, contactPoint.point, rotation);
				Destroy(spark, 1f);

				Destroy(coll.gameObject);
			}
		}
	}
}
