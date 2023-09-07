using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static PID.GuardEnemy;
using static PID.MeleeCombatHelper; 

namespace PID
{
    public class BaseEnemy : MonoBehaviour, IHitable, IStrikable
    {
        public UnityAction<Vector3, Vector3> onDeath;
        public UnityAction<bool> OnAndOff; 
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

        public virtual void Notified(Vector3 centrePoint, int size, int index)
        {
            //if (stateMachine.curStateName == State.Neutralized || stateMachine.curStateName == State.Alert)
            //{
            //    return;
            //}
            //AlertState alertState;
            //if (stateMachine.CheckState(State.Alert))
            //{
            //    alertState = stateMachine.RetrieveState(State.Alert) as AlertState;
            //    Vector3 gatherPos = RobotHelper.GroupPositionAllocator(centrePoint, size, index);
            //    alertState.SetGatherPoint(gatherPos);
            //    stateMachine.ChangeState(State.Alert);
            //}
        }
        protected virtual void Die()
        {
        }

        protected IEnumerator DeathCycle(bool deadOrAlive)
        {
            yield return new WaitForSeconds(.2f);
            OnAndOff?.Invoke(deadOrAlive); 
        }
    }
}