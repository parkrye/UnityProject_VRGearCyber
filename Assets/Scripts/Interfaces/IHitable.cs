using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal);
}
