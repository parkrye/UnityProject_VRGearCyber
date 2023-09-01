using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGetHit : MonoBehaviour, IHitable
{
    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("GotHIt"); 
    }
}
