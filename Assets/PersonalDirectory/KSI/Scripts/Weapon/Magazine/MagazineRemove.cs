using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR; 

public class MagazineRemove : MonoBehaviour
{
	public GameObject magazine; // 제거할 매거진 게임 오브젝트
	public Transform ejectPosition;
	public float ejectForce = 10.0f;
	public XRController controller;

	private Rigidbody rb;

	private void Start()
	{
		rb = magazine.GetComponent<Rigidbody>();
		if (rb == null)
		{
			rb = magazine.AddComponent<Rigidbody>();
		}
	}

	void Update()
	{
		//if (controller.GetDown(controller.Button.One))
		//{
		//	EjectMagazine();
		//}
	}

	void EjectMagazine()
	{
		if (magazine != null && rb != null)
		{
			magazine.transform.SetParent(null);
			rb.AddForce(ejectPosition.forward * ejectForce, ForceMode.Impulse);
		}
	}
}
