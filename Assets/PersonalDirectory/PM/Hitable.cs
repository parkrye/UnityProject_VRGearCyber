using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hitable
{
    public void Hit(int damage);
    protected void Die();
}
