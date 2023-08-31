using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using PID;
using static PID.RobotHelper; 

namespace PID
{
    [RequireComponent(typeof(NavMeshAgent), typeof(SightFunction))]
    public class GuardEnemy : BaseEnemy
    {
        public enum State
        {
            Idle,
            Patrol,
            LookAround, 
            Alert,
            Assault,
            Trace,
            Neutralized,
            Size
            //Possibly extending beyond for Gathering Abilities. 
        }
        #region Machine Properties 

        //Base Properties 
        Animator anim;
        Rigidbody rigid;
        NavMeshAgent agent;
        NavMeshObstacle obstacle; 
        SightFunction guardSight;
        StateMachine<State, GuardEnemy> stateMachine;
        [SerializeField] Transform robotBody; 
        public Transform RobotBody => robotBody;
        [SerializeField] Transform[] patrolPoints; 
        bool notified;

        //Combat Extra Properties
        [SerializeField] Transform muzzlePoint; 
        
        [SerializeField] float fireInterval_s;
        [SerializeField] float randomShotRadius;
        public Transform playerBody; 
        WaitForSeconds fireInterval;
        public Vector3 focusDir; 
        //Debugging 
        public TMP_Text debugText; 
        #endregion
        protected void Awake()
        {
            enemyStat = GameManager.Resource.Load<EnemyStat>("Data/Guard");
            base.SetUp(enemyStat);

            guardSight = GetComponent<SightFunction>(); 
            obstacle = GetComponent<NavMeshObstacle>();
            agent = GetComponent<NavMeshAgent>();
            rigid = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            debugText = GetComponentInChildren<TMP_Text>();  
            notified = false;

            base.SetUp(enemyStat);
            stateMachine = new StateMachine<State, GuardEnemy>(this);
            fireInterval = new WaitForSeconds(fireInterval_s);
            stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
            stateMachine.AddState(State.Patrol, new PatrolState(this, stateMachine));
            stateMachine.AddState(State.Alert, new AlertState(this, stateMachine));
            stateMachine.AddState(State.Assault, new AssaultState(this, stateMachine));
            stateMachine.AddState(State.Trace, new TraceState(this, stateMachine));
            stateMachine.AddState(State.LookAround, new LookAroundState(this, stateMachine));
            stateMachine.AddState(State.Neutralized, new NeutralizedState(this, stateMachine));
        }
        #region MACHINE RUNNING 
        private void Start()
        {
            guardSight.SyncSightStat(enemyStat); 
            guardSight.PlayerFound += DetectPlayer; 
            guardSight.PlayerLost += TempPlayerLost;
            stateMachine.SetUp(State.Idle);
        }
        private void OnDisable()
        {
            guardSight.PlayerFound -= DetectPlayer;
            guardSight.PlayerLost -= TempPlayerLost;
        }
        private void Update()
        {
            stateMachine.Update();
        }

        private void LateUpdate()
        {
            if (focusDir == Vector3.zero)
                return; 
            RobotBody.transform.localRotation = Quaternion.Slerp(RobotBody.transform.localRotation, Quaternion.LookRotation(focusDir), .1f); 
        }
        protected override void Die()
        {
            NeutralizedState neutralizeReason;
            if (stateMachine.CheckState(State.Neutralized))
            {
                neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
            }
            stateMachine.ChangeState(State.Neutralized);
        }
        #endregion

        #region  Combat Interaction 
        Coroutine fireCoroutine; 
        public void Fire()
        {
            fireCoroutine = StartCoroutine(FireRoutine());
        }

        public void StopFire()
        {
            if (fireCoroutine != null)
                StopCoroutine(fireCoroutine);
        }
        Vector3 shotAttempt;
        RaycastHit hitAttempt; 
        IEnumerator FireRoutine()
        {
            while (true)
            {
                anim.SetTrigger("GunFire"); 
                shotAttempt = FinalShotDir(muzzlePoint.position, playerBody.position, attackRange, randomShotRadius); 
                if (Physics.Raycast(muzzlePoint.position, shotAttempt, out hitAttempt, attackRange))
                {
                    IHitable hitable =  hitAttempt.collider.GetComponent<IHitable>();
                    hitable?.TakeDamage(attackDamage, hitAttempt.point, hitAttempt.normal); 
                }
                yield return fireInterval;
            }
        }

        public void DetectPlayer(Transform player)
        {
            if (stateMachine.curStateName == State.Neutralized)
                return; 
            playerBody = player;
            stateMachine.ChangeState(State.Assault); 
        }

        public void TempPlayerLost()
        {
            if (stateMachine.curStateName == State.Assault)
                stateMachine.ChangeState(State.Trace); 
        }

        public override void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            base.TakeDamage(damage, hitPoint, hitNormal);
            if (currentHealth <= 0)
            {
                NeutralizedState neutralizeReason;
                if (stateMachine.CheckState(State.Neutralized))
                {
                    neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
                    neutralizeReason.SetDeathReason(NeutralizedState.DeathType.Health); 
                }
                stateMachine.ChangeState(State.Neutralized);
            }
        }

        public void Notified(Vector3 centrePoint, int size, int index)
        {
            if (stateMachine.curStateName == State.Neutralized)
            {
                return;
            }
            AlertState alertState;
            if (stateMachine.CheckState(State.Alert))
            {
                alertState = stateMachine.RetrieveState(State.Alert) as AlertState;
                Vector3 gatherPos = RobotHelper.GroupPositionAllocator(centrePoint, size, index); 
                alertState.SetDestination(gatherPos);
                stateMachine.ChangeState(State.Alert); 
            }
        }

        public void Reactivate()
        {
            NeutralizedState.DeathType deathState; 
            if (stateMachine.curStateName != State.Neutralized)
                return;
            //Run the reason to see if the robot was destroyed or hacked. 
            NeutralizedState neutralizeReason;
            if (stateMachine.CheckState(State.Neutralized))
            {
                neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
                deathState = neutralizeReason.DeathReason; 
                if (deathState == NeutralizedState.DeathType.Hacked)
                {
                    //Reactivate; => Should add a state where it is now looking for the vengence. 
                    stateMachine.ChangeState(State.Idle); 
                }
            }

        }

        //Hackable Component. 
        #endregion

        public abstract class GuardState : StateBase<State, GuardEnemy>
        {
            public GuardState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }
        }

        #region Idle State 
        public class IdleState : GuardState
        {
            public IdleState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                //Game Started, Robots are Initialized. 
                // Depending on the Start Up notion, this should determine type of States this should switch to. 
                owner.debugText.text = "Idle";

                stateMachine.ChangeState(State.Patrol);
            }

            public override void Exit()
            {
            }

            public override void Setup()
            {
                //Could Be Random, LookAround First, and then Patrol State. 
            }

            public override void Transition()
            {
            }

            public override void Update()
            {
            }
        }
        #endregion
        #region Patrol State 
        public class PatrolState : GuardState
        {
            int lastPatrolPoint;
            int nextPatrolPoint;
            int patrolCount;
            float distDelta;
            bool patrolFinished;
            const float distThreshold = 2.5f; 
            Vector3 patrolDestination; 
            PriorityQueue<DestinationPoint> patrolQueue; 
            public PatrolState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.debugText.text = "Patrol";
                if (patrolDestination != Vector3.zero)
                    return;
                //Compute to find nearest patrol starting point based on robot's current position, get the nearest 
                for (int i = 0; i < owner.patrolPoints.Length; i++)
                {
                    float tempDist = Vector3.SqrMagnitude(owner.patrolPoints[i].position - owner.transform.position);
                    DestinationPoint tempPoint = new DestinationPoint(owner.patrolPoints[i].position, tempDist);
                    patrolQueue.Enqueue(tempPoint);
                }
                owner.anim.SetBool("Walking", true);
                patrolDestination = patrolQueue.Dequeue().destinationVectorPoint;
            }

            public override void Exit()
            {
                patrolCount = 0;
                patrolFinished = false; 
                patrolDestination = Vector3.zero; 
            }

            public override void Setup()
            {
                patrolFinished = false; 
                patrolCount = 0; 
                distDelta = 0f; 
                lastPatrolPoint = 0;
                patrolQueue = new PriorityQueue<DestinationPoint> ();
                for (int i = 0; i < owner.patrolPoints.Length; i++)
                {
                    float tempDist = Vector3.SqrMagnitude(owner.patrolPoints[i].position - owner.transform.position);
                    DestinationPoint tempPoint = new DestinationPoint(owner.patrolPoints[i].position, tempDist); 
                    patrolQueue.Enqueue(tempPoint);
                }
                patrolDestination = patrolQueue.Dequeue().destinationVectorPoint; 
            }

            public override void Transition()
            {
                if (patrolFinished)
                {
                    stateMachine.ChangeState(State.LookAround); 
                }
            }

            public override void Update()
            {
                distDelta = Vector3.SqrMagnitude(patrolDestination - owner.transform.position); 
                owner.agent.SetDestination(patrolDestination);
                if (distDelta <= distThreshold && patrolQueue.Count >= 1)
                {
                    patrolDestination = patrolQueue.Dequeue().destinationVectorPoint;
                    patrolCount++;
                }
                else if (distDelta <= distThreshold && patrolQueue.Count == 0)
                {
                    patrolFinished = true; 
                }
            }
        }
        #endregion
        #region LookAround State 
        public class LookAroundState : GuardState
        {
            public const float lookAroundTime = 5f;
            const float rotateInterval = .35f; 
            Quaternion previousRotation;
            Quaternion searchRotation; 
            public float timer; 
            public LookAroundState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.debugText.text = "LookAround";
                owner.agent.isStopped = true;
                owner.anim.SetBool("Walking", false); 
                Vector2 nextLookRandom = UnityEngine.Random.insideUnitCircle.normalized;
                Vector3 nextLookDir = new Vector3(nextLookRandom.x, owner.transform.position.y, nextLookRandom.y); 
                owner.focusDir = nextLookDir;
                //previousRotation = owner.transform.rotation;
                //searchRotation = Quaternion.LookRotation(nextLookDir, owner.transform.up); 
                //TODO: Add Method to check if the next search rotation is too close to current rotation; 
            }

            public override void Exit()
            {
                timer = 0f;
                owner.focusDir = Vector3.zero; 
                owner.RobotBody.rotation = owner.transform.rotation;
                owner.agent.isStopped = false; 
            }

            public override void Setup()
            {
                //Set Next Random Pointer to Look At, 
                owner.focusDir = Vector3.zero; 
            }

            public override void Transition()
            {
                if (timer >= lookAroundTime)
                    stateMachine.ChangeState(State.Patrol); 
            }

            public override void Update()
            {
                //owner.RobotBody.localRotation = Quaternion.Slerp(owner.RobotBody.rotation, searchRotation, rotateInterval);
                timer += Time.deltaTime; 
                //Make a look around? 
            }
        }
        #endregion
        #region Alert State
        //Activated when CCTV detects a Player, 
        // Main Function being, Each assigned Guard should head back to the designated CCTV regions. 
        public class AlertState : GuardState
        {
            SightFunction guardSight;
            Vector3 destPoint;
            float distDelta;
            const float holdPeriod = 5f; 
            const float distThreshold = 3.5f;
            public AlertState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.debugText.text = "Alert";
                owner.agent.isStopped = false;
                owner.anim.SetBool("Walking", true); 
            }

            public override void Exit()
            {
                owner.notified = false;
            }

            public override void Setup()
            {
                guardSight = owner.guardSight;
            }

            public void SetDestination(Vector3 location)
            {
                destPoint = location;
            }

            public override void Transition()
            {
                if (distDelta <= distThreshold)
                    stateMachine.ChangeState(State.LookAround); 
            }

            public override void Update()
            {
                distDelta = Vector3.SqrMagnitude(owner.agent.destination - owner.transform.position); 
            }
        }
        #endregion
        #region Assault 
        public class AssaultState : GuardState
        {
            public AssaultState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.debugText.text = "Assault";
                owner.agent.isStopped = true;
                owner.anim.SetBool("Walking", false);
                if (owner.playerBody != null)
                    owner.focusDir = (owner.playerBody.transform.position - owner.transform.position).normalized; 
                owner.Fire(); 
            }

            public override void Exit()
            {
                owner.focusDir = Vector3.zero;
                //Perhaps for now, but should consider reasons for movement 
                owner.StopFire(); 
            }

            public override void Setup()
            {

            }

            public override void Transition()
            {

            }

            public override void Update()
            {
                owner.focusDir = (owner.playerBody.transform.position - owner.transform.position).normalized;
            }
        }
        #endregion
        #region Trace State 
        // Given an Enemy previously 'Found' the enemy, but temporaily have the player gone missing, for a certain interval, Enemy will Search for the Player Under 2 Conditions. 
        // 1. Timer Runs out => Returns to the Patrol or Look Around State
        // 2. Player Dies. 
        public class TraceState : GuardState
        {
            const float maxTraceTime = 3f;
            float trackTimer;

            public TraceState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.agent.isStopped = false;
                owner.anim.SetBool("Walking", true);
                owner.debugText.text = "Trace";
                trackTimer = 0f;
                if (owner.playerBody == null)
                    return;
                owner.agent.SetDestination(owner.playerBody.position); 
            }

            public override void Exit()
            {
                trackTimer = 0f;
            }

            public override void Setup()
            {
                trackTimer = 0f; 
            }

            public override void Transition()
            {
                if (trackTimer >= maxTraceTime)
                {
                    owner.playerBody = null;
                    stateMachine.ChangeState(State.LookAround);
                    return;
                }
            }

            public override void Update()
            {
                trackTimer += Time.deltaTime;
                if (owner.playerBody != null)
                    owner.agent.SetDestination(owner.playerBody.position);
            }
        }
        #endregion
        #region Neutralized 
        public class NeutralizedState : GuardState
        {
            public enum DeathType
            {
                Health,
                Hacked,
                None
            }
            DeathType deathReason;
            public DeathType DeathReason => deathReason; 
            public NeutralizedState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public void SetDeathReason(DeathType type)
            {
                deathReason = type;
            }
            public override void Enter()
            {
                owner.debugText.text = "Neutralized";
                //Should Notify CCTV to erase from the list 
                if (deathReason == DeathType.Health)
                    owner.anim.SetBool("HealthDown", true);
                else
                    owner.anim.SetBool("Hacked", true); 
            }
            public override void Exit()
            {
                owner.anim.Rebind();
                deathReason = DeathType.None;
            }
            public override void Setup()
            {
                deathReason = DeathType.None; 
                //Should consider placing in other effects for further interactable elements. 
            }
            public override void Transition()
            { }
            public override void Update()
            { }
        }
        #endregion
    }
}