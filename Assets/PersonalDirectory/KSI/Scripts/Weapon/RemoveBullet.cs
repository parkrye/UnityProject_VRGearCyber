using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Bullet")
		{
			Destroy(collision.gameObject);
			Debug.Log("Bullet hit detected. Destroying bullet.");
		}
		// IEnumerator를 사용하여 약간의 딜레이를 준 후에 확인
		StartCoroutine(CheckIfDestroyed(collision.gameObject));
	}


	IEnumerator CheckIfDestroyed(GameObject obj)
	{
		// 1초의 딜레이를 줍니다.
		yield return new WaitForSeconds(1f);

		if (obj == null)
		{
			Debug.Log("Bullet has been destroyed.");
		}
		else
		{
			Debug.Log("Bullet was NOT destroyed.");
		}
	}
}
