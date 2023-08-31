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
            maxSightAngle = stat.maxSightAngle;
            maxSightRange = stat.maxSightRange;
            attackDamage = stat.attackDamage;
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            currentHealth -= damage;
        }

        protected virtual void Die()
        {
        }
    }
}
