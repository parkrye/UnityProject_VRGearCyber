using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PID
{
    public class RobotHeader : RobotParts
    {
        [TextArea]
        public string description; 
        [SerializeField] int headShotMultiplier;
        
        public override void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            base.TakeDamage(damage * headShotMultiplier, hitPoint, hitNormal);
        }

        public override void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            base.TakeStrike(hitter, damage * headShotMultiplier, hitPoint, hitNormal);
        }
    }
}