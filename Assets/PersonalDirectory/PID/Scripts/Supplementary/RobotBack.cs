using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID; 

namespace PID
{
    public class RobotBack : RobotParts
    {
        [TextArea]
        string description;
        public override void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            enemy.TakeDamage(damage, hitPoint, hitNormal);
        }

        public override void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            enemy.TakeStrike(hitter, damage, hitPoint, hitNormal);
        }
    }
}

