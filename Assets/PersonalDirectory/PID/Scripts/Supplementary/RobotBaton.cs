using PID;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotBaton : MonoBehaviour
{
    [SerializeField] string targetTag; 
    int attackDamage;
    int attackRange;
    float angle; 

    public void SyncStatData(EnemyStat stat)
    {
        attackDamage = stat.attackDamage;
        attackRange = stat.attackRange;
        angle = stat.maxSightAngle; 
    }

    public void AttackTiming()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag != targetTag)
                return; 
            Vector3 dirToTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, dirToTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                continue;
            //Only hits once.  
            IHitable hittable = collider.GetComponent<IHitable>();
            if (hittable != null)
            {
                hittable?.TakeDamage(attackDamage, Vector3.zero, Vector3.zero);
                break;
            }
        }
    }
}
