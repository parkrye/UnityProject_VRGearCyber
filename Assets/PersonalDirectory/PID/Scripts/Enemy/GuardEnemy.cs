using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;

namespace PID
{
    [RequireComponent(typeof(NavMeshAgent), typeof(SightFunction))]
    public class GuardEnemy : BaseEnemy, IPointerClickHandler, IHitable
    {
        /// <summary>
        /// Hide and Clash should be sub-state driven by the Assault state. 
        /// </summary>
        public enum State
        {
            Idle,
            Infiltrated,
            Patrol,
            LookAround, 
            Alert,
            Assault,
            Trace,
            SoundReact,
            Hide,
            Clash,
            Neutralized,
            Size
            //Possibly extending beyond for Gathering Abilities. 
        }
        #region MACHINE PROPERTIES 
        //Base Properties 
        public Animator anim;
        Rigidbody rigid;
        NavMeshObstacle obstacle; 
        SightFunction guardSight;
        AuditoryFunction guardEars; 
        StateMachine<State, GuardEnemy> stateMachine;
        [SerializeField] Transform[] patrolPoints; 

        //Combat Properties
        [SerializeField] Transform muzzlePoint;
        [SerializeField] LayerMask headRegion;
        [SerializeField] LayerMask bodyRegion; 
        RobotGun robotGun; 
        public Transform playerBody; 
        public Vector3 focusDir;

        //Hack Properties 
        public State curState => stateMachine.curStateName; 
        //Debugging 
        public TMP_Text debugText;
        protected void Awake()
        {
            //DEBUGGING 
            enemyStat = GameManager.Resource.Load<EnemyStat>("Data/Guard");
            base.SetUp(enemyStat);

            guardEars = GetComponent<AuditoryFunction>();
            guardSight = GetComponent<SightFunction>();
            obstacle = GetComponent<NavMeshObstacle>();
            agent = GetComponent<NavMeshAgent>();
            rigid = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            robotGun = GetComponentInChildren<RobotGun>();
            debugText = GetComponentInChildren<TMP_Text>();

            base.SetUp(enemyStat);
            stateMachine = new StateMachine<State, GuardEnemy>(this);
            stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
            stateMachine.AddState(State.Infiltrated, new InfiltratedState(this, stateMachine));
            stateMachine.AddState(State.Patrol, new PatrolState(this, stateMachine));
            stateMachine.AddState(State.Alert, new AlertState(this, stateMachine));
            stateMachine.AddState(State.Assault, new AssaultState(this, stateMachine));
            stateMachine.AddState(State.Trace, new TraceState(this, stateMachine));
            stateMachine.AddState(State.LookAround, new LookAroundState(this, stateMachine));
            stateMachine.AddState(State.SoundReact, new SoundReactState(this, stateMachine));
            stateMachine.AddState(State.Neutralized, new NeutralizedState(this, stateMachine));
        }
        private void Start()
        {
            AgentSetUp(enemyStat);
            robotGun.SyncStatData(enemyStat);
            guardSight.SyncSightStat(enemyStat);
            guardSight.PlayerFound += DetectPlayer;
            guardSight.PlayerLost += TempPlayerLost;
            guardEars.trackSound += DetectSound; 
            stateMachine.SetUp(State.Idle);
            GameManager.Data.timeScaleEvent?.AddListener(TimeScale);
        }
        #endregion
        #region MACHINE UPDATES 
        public void AgentSetUp(EnemyStat robotStat)
        {
            agent.speed = moveSpeed;
            //Takes the General stat values, implement it on the nav_agent level. 
        }
        public void AgentStop()
        {

        }
        Vector3 localVel;
        Vector3 worldVel; 
        public void UpdateAnim()
        {
            localVel = agent.velocity.normalized;
            worldVel = transform.InverseTransformDirection(localVel);
            anim.SetFloat("Speed", agent.speed);
            if (agent.isStopped)
            {
                anim.SetFloat("Speed", 0);
                anim.SetFloat("ZSpeed", 0);
                anim.SetFloat("XSpeed", 0);
            }
            if (agent.speed > .1)
            {
                anim.SetFloat("ZSpeed", worldVel.z);
                anim.SetFloat("XSpeed", worldVel.x);
            }
        }
        private void OnDisable()
        {
            guardSight.PlayerFound -= DetectPlayer;
            guardSight.PlayerLost -= TempPlayerLost;
            guardEars.trackSound -= DetectSound;
            GameManager.Data.timeScaleEvent?.RemoveListener(TimeScale);
        }
        private void Update()
        {
            UpdateAnim(); 
            stateMachine.Update();
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
        #region  COMBAT INTERACTIONS 
        public void TimeScale()
        {
            anim.speed = GameManager.Data.TimeScale; 
            agent.speed = GameManager.Data.TimeScale;
        }
        public void DetectPlayer(Transform player)
        {
            //should Enemy be under hide and shoot, ignore further calls. 
            if (stateMachine.curStateName == State.Neutralized || 
                stateMachine.curStateName == State.Infiltrated)
                return; 
            playerBody = player;
            stateMachine.ChangeState(State.Assault); 
        }
        public void DetectSound(Vector3 soundPoint)
        {
            if (stateMachine.curStateName == State.Infiltrated ||
                stateMachine.curStateName == State.Neutralized ||
                stateMachine.curStateName == State.Assault ||
                stateMachine.curStateName == State.Trace)
                return; 
            SoundReactState soundReactState;
            if (stateMachine.CheckState(State.SoundReact))
            {
                soundReactState = stateMachine.RetrieveState(State.SoundReact) as SoundReactState; 
                soundReactState.SetSearchPoint(soundPoint);
            }
            stateMachine.ChangeState(State.SoundReact); 
        }

        public void TempPlayerLost()
        {
            if (stateMachine.curStateName == State.Assault)
                stateMachine.ChangeState(State.Trace); 
        }
        int incomingDamage;
        public override void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (stateMachine.curStateName == State.Neutralized)
                return;
            //GameManager.Resource.Instantiate<ParticleSystem>("Enemy/TakeDamage", hitPoint, Quaternion.LookRotation(hitNormal), true);
            base.TakeDamage(damage, hitPoint, hitNormal);
            anim.SetTrigger("TakeHit");
            if (currentHealth <= 0)
            {
                NeutralizedState neutralizeReason;
                if (stateMachine.CheckState(State.Neutralized))
                {
                    neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
                    neutralizeReason.SetDeathReason(NeutralizedState.DeathType.Health);
                }
                stateMachine.ChangeState(State.Neutralized);
                onDeath?.Invoke(hitNormal, hitPoint);
                StartCoroutine(DeathCycle(false)); 
            }
        }
        public override void Notified(Vector3 centrePoint, int size, int index)
        {
            if (stateMachine.curStateName == State.Neutralized 
                || stateMachine.curStateName == State.Alert)
            {
                return;
            }
            AlertState alertState;
            if (stateMachine.CheckState(State.Alert))
            {
                alertState = stateMachine.RetrieveState(State.Alert) as AlertState;
                Vector3 gatherPos = RobotHelper.GroupPositionAllocator(centrePoint, size, index); 
                alertState.SetGatherPoint(gatherPos);
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
        #region DEBUGGING ISSUES 
        public void OnPointerClick(PointerEventData eventData)
        {
            TakeDamage(50, eventData.pointerPressRaycast.worldPosition, eventData.pointerPressRaycast.worldNormal);
            Debug.Log(currentHealth); 
        }
        #endregion
        //Hackable Component. 
        public void Hacked()
        {
            NeutralizedState neutralizeReason;
            if (stateMachine.CheckState(State.Neutralized))
            {
                neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
                neutralizeReason.SetDeathReason(NeutralizedState.DeathType.Hacked);
            }
            stateMachine.ChangeState(State.Neutralized);
        }

        public void HackFailed(State prevState)
        {
            if (stateMachine.curStateName != State.Neutralized)
                stateMachine.ChangeState(prevState); 
        }
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
        #region Infiltrated State 
        public class InfiltratedState : GuardState
        {
            public InfiltratedState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.agent.isStopped = true;
                //owner.anim.speed = GameManager.Data.TimeScale; 
                //owner.anim.SetBool("Walking", false); 
            }

            public override void Exit()
            {
                owner.agent.isStopped = false; 
            }

            public override void Setup()
            {
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
            bool hasPatrolPath;
            const float distThreshold = 2.5f; 
            Vector3 patrolDestination; 
            PriorityQueue<DestinationPoint> patrolQueue; 
            public PatrolState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.debugText.text = "Patrol";
                if (patrolDestination != Vector3.zero || !hasPatrolPath)
                    return;
                //Compute to find nearest patrol starting point based on robot's current position, get the nearest 
                for (int i = 0; i < owner.patrolPoints.Length; i++)
                {
                    float tempDist = Vector3.SqrMagnitude(owner.patrolPoints[i].position - owner.transform.position);
                    DestinationPoint tempPoint = new DestinationPoint(owner.patrolPoints[i].position, tempDist);
                    patrolQueue.Enqueue(tempPoint);
                }
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
                if (owner.patrolPoints == null || owner.patrolPoints.Length <= 0)
                {
                    hasPatrolPath = false;
                    patrolFinished = true;
                    return;
                }
                hasPatrolPath = true;
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
                if (!hasPatrolPath)
                    return;
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
            Vector3 focusDir;
            float rotationTime = .01f;
            public float timer; 
            public LookAroundState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.debugText.text = "LookAround";
                owner.agent.isStopped = true;
                NextLookDir(); 
                //previousRotation = owner.transform.rotation;
                //searchRotation = Quaternion.LookRotation(nextLookDir, owner.transform.up); 
                //TODO: Add Method to check if the next search rotation is too close to current rotation; 
            }

            public override void Exit()
            {
                timer = 0f;
                owner.focusDir = Vector3.zero;
                owner.agent.isStopped = false;
                owner.agent.updateRotation = true;
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
                owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, searchRotation, rotationTime);
                if (RobotHelper.DirectionIntersect(owner.transform.forward, focusDir))
                    NextLookDir();
                //Make a look around? 
            }

            private void NextLookDir()
            {
                Vector2 nextLookRandom = UnityEngine.Random.insideUnitCircle.normalized;
                Vector3 nextLookDir = new Vector3(nextLookRandom.x, 0, nextLookRandom.y);
                focusDir = nextLookDir;
                searchRotation = Quaternion.LookRotation(focusDir);
            }
        }
        #endregion
        #region Alert State
        //Activated when CCTV detects a Player, 
        // Main Function being, Each assigned Guard should head back to the designated CCTV regions. 
        public class AlertState : GuardState
        {
            SightFunction guardSight;
            Vector3 destPoint = Vector3.zero;
            bool abortGather; 
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
                if (destPoint != Vector3.zero)
                    owner.agent.SetDestination(destPoint);
                else
                    abortGather = true; 
            }

            public override void Exit()
            {
            }

            public override void Setup()
            {
                abortGather = false;
                guardSight = owner.guardSight;
            }

            public void SetGatherPoint(Vector3 location)
            {
                destPoint = location;
            }

            public override void Transition()
            {
                if (distDelta <= distThreshold || abortGather)
                    stateMachine.ChangeState(State.LookAround); 
            }

            public override void Update()
            {
                distDelta = Vector3.SqrMagnitude(owner.agent.destination - owner.transform.position); 
            }
        }
        #endregion
        #region Assault 
        /// <summary>
        /// Precedent state to a 'Combat' State, where depending on robot's attack range,
        /// Charges toward found target. 
        /// Also manages target's transform information. 
        /// </summary>
        public class AssaultState : GuardState
        {
            RobotGun robotGun; 
            public AssaultState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.debugText.text = "Assault";
                //owner.agent.updateRotation = false;
                owner.agent.isStopped = true;
                owner.agent.updateRotation = false; 
                //if (owner.playerBody != null)
                //    owner.focusDir = (owner.playerBody.transform.position - owner.transform.position).normalized; 
            }

            public override void Exit()
            {
                //owner.focusDir = Vector3.zero;
                ////Perhaps for now, but should consider reasons for movement 
                //owner.StopFire(); 
                owner.agent.updateRotation = true; 
            }

            public override void Setup()
            {
                robotGun = owner.robotGun;
            }

            public override void Transition()
            {

            }

            public override void Update()
            {
                //owner.focusDir = (owner.playerBody.transform.position - owner.transform.position).normalized;
                RotateTowardPlayer();
                robotGun.AttemptFire(owner.playerBody);
            }

            public void RotateTowardPlayer()
            {
                Vector3 lookDir = (owner.playerBody.transform.position - owner.transform.position).normalized; 
                lookDir.y = 0;
                owner.transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
        #endregion
        #region HideState 
        /// <summary>
        /// Whenever Enemy needs to reload, it will look for a place to hide. 
        /// 1. It will evaluate place to hide. 
        /// 2. once reached, start reloading, but continue 'hiding phase'. 
        /// 3. if enemy is found, go back to assault state, 
        /// 4. else, return to the original spot to pre-hidiing. 
        /// </summary>
        public class HideState : GuardState
        {
            Vector3 preHiding;
            const float updateFrequency = .5f;
            WaitForSeconds updateInterval; 
            public HideState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                preHiding = owner.transform.position;
                owner.RunTheCoroutine(HideRoutine()); 
            }

            public override void Exit()
            {
                owner.StopTheCoroutine(HideRoutine()); 
            }

            public override void Setup()
            {
                updateInterval = new WaitForSeconds(updateFrequency);
            }

            public override void Transition()
            {
            }

            public override void Update()
            {
                
            }
            IEnumerator HideRoutine()
            {
                while (true)
                {
                    yield return updateInterval; 
                }
                yield return null; 
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
        #region SoundReact State 
        public class SoundReactState : GuardState
        {
            Vector3 searchPoint;
            const float distThreshold = 4f;
            float deltaDist; 
            bool finishedSearch; 
            public SoundReactState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                if (searchPoint == Vector3.zero)
                    return;
                owner.agent.SetDestination(searchPoint); 
                deltaDist = Vector3.SqrMagnitude(searchPoint - owner.transform.position);
            }

            public override void Exit()
            {
                finishedSearch = false;
                deltaDist = 0f; 
            }

            public override void Setup()
            {
                searchPoint = Vector3.zero;
                finishedSearch = false;
                deltaDist = 0f; 
            }

            public override void Transition()
            {
                if (finishedSearch)
                    stateMachine.ChangeState(State.LookAround);
            }

            public override void Update()
            {
                if (searchPoint == Vector3.zero)
                    finishedSearch = true;
                else if (deltaDist < distThreshold)
                    finishedSearch = true; 
                deltaDist = Vector3.SqrMagnitude(searchPoint - owner.transform.position); 
            }

            public void SetSearchPoint(Vector3 soundPoint)
            {
                searchPoint = soundPoint;
                finishedSearch = false; 
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
                owner.agent.isStopped = true;
                owner.debugText.text = "Neutralized";
                //Should Notify CCTV to erase from the list 
                //if (deathReason == DeathType.Health)
                //    owner.anim.SetBool("Destroyed", true);
                //else
                //    owner.anim.SetBool("Hack", true); 
            }
            public override void Exit()
            {
                //Called when reactivated through EnemyManager. 
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
            {
               
            }

            

        }
        #endregion
    }
}