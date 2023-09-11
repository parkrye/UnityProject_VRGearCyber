using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static PID.GuardEnemy;
using static PID.MeleeCombatHelper; 

namespace PID
{
    public class BaseEnemy : MonoBehaviour, IHitable, IStrikable
    {
        public UnityAction<Vector3, Vector3> onDeath;
        public UnityAction<Vector3, int, int> cctvNotified; 
        public UnityAction<bool> OnAndOff;
        public Animator anim;

        //SetUp Base States 
        protected NavMeshAgent agent;
        protected SightFunction robotSight;
        protected AuditoryFunction robotEars;
        public NavMeshAgent Agent => agent;
        protected float moveSpeed;
        protected int maxHealth;
        protected float maxSightRange;
        protected float maxSightAngle;
        protected int attackDamage;
        protected int attackRange; 
        protected EnemyStat enemyStat;
        protected int currentHealth
        {
            get;
            private set;
        }

        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            robotSight = GetComponent<SightFunction>();
            robotEars = GetComponent<AuditoryFunction>();
            agent = GetComponent<NavMeshAgent>();

        }

        protected virtual void SetUp(EnemyStat stat)
        {
            attackRange = stat.attackRange;
            moveSpeed = stat.moveSpeed;
            maxHealth = stat.maxHealth;
            attackDamage = stat.attackDamage;
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            //GameManager.Resource.Instantiate<ParticleSystem>("Enemy/TakeDamage", hitPoint, Quaternion.LookRotation(hitNormal), true);
            currentHealth -= damage;
        }
        public virtual void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            FlankJudgement(hitter, transform, damage, hitPoint, hitNormal, CheckFlank);
        }

        public void CheckFlank(float damage, Vector3 hitPoint, Vector3 hitNormal, bool success)
        {
            if (success)
            {
                currentHealth -= ((int)damage * flankDamageMultiplier); 
            }
            else
            {
                currentHealth -= (int)damage; 
            }
        }

        public virtual void Notified(Vector3 centrePoint, int size, int index)
        {
            
        }
        protected virtual void Die()
        {
        }
        protected IEnumerator DeathCycle(bool deadOrAlive)
        {
            yield return new WaitForSeconds(.2f);
            OnAndOff?.Invoke(deadOrAlive); 
        }


        #region Default General States 
        #endregion
    }
}