using PID;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobotBaton : MonoBehaviour
{
    [SerializeField] string targetTag = "Player";
    [SerializeField] float attackInterval;
    Animator anim;
    Transform attacker; 
    float attackTimer; 
    int attackDamage;
    int attackRange;
    [SerializeField] float angle;
    float cosValue; 

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        attacker = anim.gameObject.transform; 
        attackTimer = 0f; 
    }
    private void Start()
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
        if (Time.time > attackTimer)
        {
            anim.SetTrigger("Strike"); 
            attackTimer = Time.time + attackInterval;
        }
    }

    public void AttackTiming()
    {
        Collider[] colliders = Physics.OverlapSphere(attacker.position, attackRange);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.tag != targetTag)
                continue; 
            Vector3 dirToTarget = (collider.transform.position - attacker.position).normalized;
            dirToTarget.y = 0f; 
            if (dirToTarget == Vector3.zero)
            {
                IHitable hitable = collider.GetComponent<IHitable>();
                hitable?.TakeDamage(attackDamage, Vector3.zero, Vector3.zero);
                break; ; 
            }
            if (Vector3.Dot(attacker.forward, dirToTarget) < cosValue)
            {
                Debug.Log($"{attacker.forward}: {dirToTarget}: {Vector3.Dot(attacker.forward, dirToTarget) < cosValue}");
                continue;
            }
            //Only hits once.  
            IHitable hittable = collider.gameObject.GetComponent<IHitable>();
            if (hittable != null)
            {
                hittable?.TakeDamage(attackDamage, Vector3.zero, Vector3.zero);
                break;
            }
        }
    }
}
