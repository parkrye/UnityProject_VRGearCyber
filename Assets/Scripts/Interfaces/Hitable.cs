using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hitable
{
    IEnumerator Hit(int damage);
    IEnumerator Break();
}
