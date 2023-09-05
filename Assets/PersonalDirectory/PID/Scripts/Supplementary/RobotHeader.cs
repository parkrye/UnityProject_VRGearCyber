using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHeader : MonoBehaviour, IHitable
{
    GuardEnemy enemy;
    [SerializeField] int headShotMultiplier; 
    private void Awake()
    {
        enemy = GetComponentInParent<GuardEnemy>();
    }
    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        enemy.TakeDamage(damage* headShotMultiplier, hitPoint, hitNormal);
    }
}
