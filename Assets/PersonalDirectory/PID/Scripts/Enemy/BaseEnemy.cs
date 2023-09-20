using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static PID.RobotHelper; 
using static PID.GuardEnemy;
using static PID.MeleeCombatHelper; 

namespace PID
{
    public class BaseEnemy : MonoBehaviour, IHitable, IStrikable
    {
        public RobotType robotType; 
        public UnityAction<Vector3, Vector3> onDeath;
        public UnityAction<Vector3, int, int> cctvNotified; 
        public UnityAction<bool> OnAndOff;
        public Animator anim;
        [SerializeField] protected State curStateDebug; 

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
        [SerializeField] int currentHealth; 
        protected int CurrentHealth
        {
            get => currentHealth; 
            set => currentHealth = value;
        }

        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            robotSight = GetComponent<SightFunction>();
            robotEars = GetComponent<AuditoryFunction>();
            agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start()
        {
            GameManager.Data.timeScaleEvent?.AddListener(TimeScale);
        }

        protected virtual void OnDisable()
        {
            GameManager.Data.timeScaleEvent?.RemoveListener(TimeScale);
        }
        protected virtual void SetUp(EnemyStat stat)
        {
            attackRange = stat.attackRange;
            moveSpeed = stat.moveSpeed;
            maxHealth = stat.maxHealth;
            attackDamage = stat.attackDamage;
            CurrentHealth = maxHealth;
        }

        public virtual void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            //GameManager.Resource.Instantiate<ParticleSystem>("Enemy/TakeDamage", hitPoint, Quaternion.LookRotation(hitNormal), true);
            CurrentHealth -= damage;
        }
        public virtual void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            FlankJudgement(hitter, transform, damage, hitPoint, hitNormal, CheckFlank);
        }

        public void CheckFlank(float damage, Vector3 hitPoint, Vector3 hitNormal, bool success)
        {
            if (success)
            {
                CurrentHealth -= ((int)damage * flankDamageMultiplier); 
            }
            else
            {
                CurrentHealth -= (int)damage; 
            }
        }

        public virtual void Notified(Vector3 centrePoint, int size, int index)
        {
        }
        public void TimeScale()
        {
            anim.speed = GameManager.Data.TimeScale;
            agent.speed = GameManager.Data.TimeScale;
        }

        public virtual State CurState()
        {
            return State.Idle; 
        }
        protected virtual void Die()
        {
        }
        public virtual void Hacked()
        {
            
        }
        public virtual void HackFailed(State prevState)
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