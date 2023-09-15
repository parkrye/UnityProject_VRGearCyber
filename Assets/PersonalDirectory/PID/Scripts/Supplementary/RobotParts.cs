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
        [SerializeField] bool armorAvailable; 
        protected BaseEnemy enemy; 
        private void Awake()
        {
            if (armorAvailable) 
            {
                wearable = GetComponent<HittableItem>();
                if (!wearable)
                    wearable = GetComponentInChildren<HittableItem>();
            }
            
            enemy = GetComponentInParent<BaseEnemy>();
            if (enemy.robotType == RobotType.Guard)
                enemy = enemy as GuardEnemy;
            else if (enemy.robotType == RobotType.Tackler)
                enemy = enemy as TackleEnemy;
            else
                enemy = enemy as ScoutEnemy; 
        }
        public virtual void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (!armorAvailable)
            {
                enemy.TakeDamage(damage, hitPoint, hitNormal);
                return; 
            }
            if (wearable.IsWearing)
            {
                wearable.TakeDamage(damage, hitPoint, hitNormal);
                return; 
            }
            enemy.TakeDamage(damage, hitPoint, hitNormal);
        }

        public virtual void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (!armorAvailable)
            {
                enemy.TakeStrike(hitter, damage, hitPoint, hitNormal);
                return;
            }
            if (wearable.IsWearing)
            {
                wearable.TakeStrike(hitter, damage, hitPoint, hitNormal);
                return; 
            }
            enemy.TakeStrike(hitter, damage, hitPoint, hitNormal);
        }
    }

}
