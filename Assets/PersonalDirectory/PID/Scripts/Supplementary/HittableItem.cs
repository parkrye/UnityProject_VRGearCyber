using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableItem : MonoBehaviour, IHitable
{
    [SerializeField] int durability;
    Rigidbody rb;
    [SerializeField] float flightForce; 
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        durability--; 
        if (durability < 0)
        {
            rb.isKinematic = false;
            Invoke("ReturnToAshes", 5f); 
        }
    }

    public void ReturnToAshes()
    {
        GameManager.Pool.Release(this);
    }
}
