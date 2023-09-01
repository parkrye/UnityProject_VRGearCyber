using UnityEngine;

namespace PID
{
    public class BaseEnemy : MonoBehaviour, IHitable, IHackable
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
            moveSpeed = stat.moveSpeed;
            maxHealth = stat.maxHealth;
            attackDamage = stat.attackDamage;
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            GameManager.Resource.Instantiate<ParticleSystem>("Enemy/TakeDamage", hitPoint, Quaternion.LookRotation(hitNormal), true);
            currentHealth -= damage;
        }

        protected virtual void Die()
        {
        }
    }
}
