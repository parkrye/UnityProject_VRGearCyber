using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireController : MonoBehaviour
{
	public float fireRate = 0.1f;
	public GameObject bulletPrefab;
	public Transform muzzlePointTransform;

	private float elapsedTime;

	private void Update()
	{
		elapsedTime += Time.deltaTime;

		if (Input.GetMouseButtonDown(1))
		{
			Shoot();

			elapsedTime = 0;
		}
	}

	private void Shoot()
	{
		GameObject gameObject = Instantiate(bulletPrefab, muzzlePointTransform.position, Quaternion.Euler(0, 0, 0));
		gameObject.transform.forward = muzzlePointTransform.forward;
	}
}
