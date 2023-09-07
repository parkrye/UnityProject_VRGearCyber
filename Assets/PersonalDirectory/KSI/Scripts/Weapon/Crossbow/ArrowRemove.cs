using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
    public class ArrowRemove : MonoBehaviour
    {
		[SerializeField] private GameObject arrowSparkEffect;

		private void OnCollisionEnter(Collision coll)
		{
			if (coll.collider.CompareTag("Arrow"))
			{
				ContactPoint contactPoint = coll.GetContact(0);
				Quaternion rotation = Quaternion.LookRotation(-contactPoint.normal);

				GameObject spark = Instantiate(arrowSparkEffect, contactPoint.point, rotation);
				Destroy(spark, 1f);

				Destroy(coll.gameObject);
			}
		}
	}
}
