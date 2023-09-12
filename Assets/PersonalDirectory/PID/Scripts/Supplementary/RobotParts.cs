using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotParts : MonoBehaviour, IHitable, IStrikable
{
    HittableItem wearable; 
    GuardEnemy enemy;
    bool isWearing; 
    public bool IsWearing
    { get => isWearing;
    set => isWearing = value;}
    [SerializeField] int headShotMultiplier;
    private void Awake()
    {
        enemy = GetComponentInParent<GuardEnemy>();
    }
    public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isWearing)
        {
            wearable.TakeDamage(damage, hitPoint, hitNormal);
        }
        enemy.TakeDamage(damage * headShotMultiplier, hitPoint, hitNormal);
    }

    public void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isWearing)
        {
            wearable.TakeStrike(hitter, damage, hitPoint, hitNormal);   
        }
        enemy.TakeStrike(hitter, damage, hitPoint, hitNormal);
    }
}
