using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR; 

namespace KSI
{
	public class MagazineRemove : MonoBehaviour
	{
		public GameObject magazine;
		public Transform ejectPosition;
		public float ejectForce = 10.0f;

		private Rigidbody rb;


		private void Start()
		{
			rb = magazine.GetComponent<Rigidbody>();
			if (rb == null)
			{
				rb = magazine.AddComponent<Rigidbody>();
			}

			StartCoroutine(WaitRoutine());
		}

		private IEnumerator WaitRoutine()
		{
			yield return new WaitUntil(()=> GameManager.Data != null);
			yield return new WaitUntil(()=> GameManager.Data.Player != null);
			yield return new WaitUntil(()=> GameManager.Data.Player.ExtraInput != null);

			GameManager.Data.Player.ExtraInput.RightHandPrimaryButtonEvent.AddListener(EjectMagazine);
		}

		void EjectMagazine(bool isPressed)
		{
			if (magazine != null && rb != null)
			{
				magazine.transform.SetParent(null);
				rb.AddForce(ejectPosition.forward * ejectForce, ForceMode.Impulse);
			}
		}
	}
}
