using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Ranged : MonoBehaviour
{
	public float fireRate = 0.1f;
	public GameObject bulletPrefab;
	public Transform muzzlePointTransform;
	
	private Animator animator;

	public void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void PullTheTrigger()
	{
		animator.SetTrigger("Fire");
	}

	public void Shoot()
	{
		GameObject gameObject = Instantiate(bulletPrefab, muzzlePointTransform.position, Quaternion.Euler(0, 0, 0));
		gameObject.transform.forward = muzzlePointTransform.forward;
	}
}
