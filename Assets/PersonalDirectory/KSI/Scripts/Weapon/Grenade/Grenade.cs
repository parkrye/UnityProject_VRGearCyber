using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSI
{
	public class Grenade : MonoBehaviour
	{
		[SerializeField] private GameObject grenadeEffect; // 폭발 효과 파티클

		private Transform tr;
		private Rigidbody rb;

		private void Start()
		{
			tr = GetComponent<Transform>();
			rb = GetComponent<Rigidbody>();
		}
	}
}