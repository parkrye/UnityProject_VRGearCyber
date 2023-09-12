using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PID.RobotHelper; 

namespace PID
{
    public class RobotParts : MonoBehaviour, IHitable, IStrikable
    {
        HittableItem wearable;
        protected BaseEnemy enemy; 
        private void Awake()
        {
            wearable = GetComponent<HittableItem>();
            if (!wearable)
                wearable = GetComponentInChildren<HittableItem>();
            enemy = GetComponentInParent<BaseEnemy>();
            if (enemy.robotType == RobotType.Guard)
                enemy = enemy as GuardEnemy;
            else if (enemy.robotType == RobotType.Tackler)
                enemy = enemy as TackleEnemy; 
        }
        public virtual void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (wearable.IsWearing)
            {
                wearable.TakeDamage(damage, hitPoint, hitNormal);
            }
            enemy.TakeDamage(damage, hitPoint, hitNormal);
        }

        public virtual void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (wearable.IsWearing)
            {
                wearable.TakeStrike(hitter, damage, hitPoint, hitNormal);
            }
            enemy.TakeStrike(hitter, damage, hitPoint, hitNormal);
        }
    }

}
