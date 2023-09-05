using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID; 

public class RobotBack : MonoBehaviour, IHitable, IStrikable
{
    GuardEnemy enemy; 
    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        enemy.TakeDamage(damage, hitPoint, hitNormal);
    }

    public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        enemy.TakeStrike(hitter, damage, hitPoint, hitNormal);
    }

    // Start is called before the first frame update
    void Awake()
    {
        enemy = GetComponentInParent<GuardEnemy>();
    }
}
