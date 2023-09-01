using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    public float force;
    
    private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * force);
	}
}
