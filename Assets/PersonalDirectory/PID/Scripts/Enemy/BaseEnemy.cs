using UnityEngine;
using static PID.MeleeCombatHelper; 

namespace PID
{
    public class BaseEnemy : MonoBehaviour, IHitable, IStrikable
    {
        //SetUp Base States 
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
        protected virtual void Die()
        {
        }

        
    }
}
