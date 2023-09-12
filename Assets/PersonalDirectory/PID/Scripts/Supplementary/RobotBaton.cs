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
    float cosValue; 

    private void Awake()
    {
        cosValue = Mathf.Cos(angle * .5f * Mathf.Deg2Rad);
    }
    public void SyncStatData(EnemyStat stat)
    {
        attackDamage = stat.attackDamage;
        attackRange = stat.attackRange;
        angle = stat.maxSightAngle; 
    }
    public void AttackAttempt()
    {

    }

    public void AttackTiming()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag != targetTag)
                return; 
            Vector3 dirToTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, dirToTarget) < cosValue)
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

    IEnumerator TryAttack()
    {
        yield return null;
    }
}
