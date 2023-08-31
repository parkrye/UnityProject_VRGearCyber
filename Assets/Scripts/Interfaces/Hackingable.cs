using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hackingable
{
    IEnumerator HackingCheck(bool success);
}
