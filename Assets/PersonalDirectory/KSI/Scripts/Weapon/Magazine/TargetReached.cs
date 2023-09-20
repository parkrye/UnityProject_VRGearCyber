using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetReached : MonoBehaviour
{
    [SerializeField] private float threshold = 0.02f;
    [SerializeField] private Transform target;
	[SerializeField] UnityEvent OnReached;
	private bool reached = false;

	private void FixedUpdate()
	{
		float distance = Vector3.Distance(transform.position, target.position);

		if (distance < threshold && !reached)
		{
			OnReached.Invoke();
			reached = true;
		}
		else if (distance >= threshold)
		{ 
			reached = false;
		}
	}
}
