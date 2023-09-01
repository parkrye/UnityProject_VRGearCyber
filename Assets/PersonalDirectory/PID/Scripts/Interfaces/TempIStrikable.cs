using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public interface TempIHitable
    {
        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal, RaycastHit hitInfo); 
    }
}

