using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static PID.RobotHelper;

namespace PID
{
    [RequireComponent(typeof(NavMeshAgent), typeof(SightFunction))]
    public class GuardEnemy : BaseEnemy, IHitable
    {
        /// <summary>
        /// Hide and Clash should be sub-state driven by the Assault state. 
        /// </summary>
        #region MACHINE PROPERTIES 
        //Base Properties 
        Rigidbody rigid;
        NavMeshObstacle obstacle; 
         
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
        protected override void Awake()
        {
            //DEBUGGING 
            enemyStat = GameManager.Resource.Load<EnemyStat>("Data/Guard");
            base.SetUp(enemyStat);
            base.Awake(); 
            
            obstacle = GetComponent<NavMeshObstacle>();
            rigid = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            robotGun = GetComponentInChildren<RobotGun>();

            stateMachine = new StateMachine<State, GuardEnemy>(this);
            stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
            stateMachine.AddState(State.Infiltrated, new InfiltratedState(this, stateMachine));
            stateMachine.AddState(State.Patrol, new PatrolState(this, stateMachine));
            stateMachine.AddState(State.Alert, new AlertState(this, stateMachine));
            stateMachine.AddState(State.Assault, new AssaultState(this, stateMachine));
            stateMachine.AddState(State.Hide, new HideState(this, stateMachine));
            stateMachine.AddState(State.Trace, new TraceState(this, stateMachine));
            stateMachine.AddState(State.LookAround, new LookAroundState(this, stateMachine));
            stateMachine.AddState(State.SoundReact, new SoundReactState(this, stateMachine));
            stateMachine.AddState(State.Neutralized, new NeutralizedState(this, stateMachine));
        }
        protected override void Start()
        {
            base.Start(); 
            AgentSetUp(enemyStat);
            robotGun.SyncStatData(enemyStat);
            robotSight.SyncSightStat(enemyStat);
            robotSight.PlayerFound += DetectPlayer;
            robotSight.PlayerLost += TempPlayerLost;
            robotEars.trackSound += DetectSound;
            cctvNotified += Notified; 
            stateMachine.SetUp(State.Idle);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            robotSight.PlayerFound -= DetectPlayer;
            robotSight.PlayerLost -= TempPlayerLost;
            robotEars.trackSound -= DetectSound;
            cctvNotified -= Notified;
        }

        #endregion
        #region MACHINE UPDATES 
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
        private void Update()
        {
            UpdateAnim();
            stateMachine.Update();
            curStateDebug = stateMachine.curStateName;
        }
        public void AgentSetUp(EnemyStat robotStat)
        {
            agent.speed = moveSpeed;
            //Takes the General stat values, implement it on the nav_agent level. 
        }
        public void AgentStop()
        {

        }


        #endregion
        #region  COMBAT INTERACTIONS 
        public void DetectPlayer(Transform player)
        {
            //should Enemy be under hide and shoot, ignore further calls. 
            if (stateMachine.curStateName == State.Neutralized || 
                stateMachine.curStateName == State.Infiltrated ||
                stateMachine.curStateName == State.Assault ||
                stateMachine.curStateName == State.Hide)
                return; 
            playerBody = player;
            stateMachine.ChangeState(State.Assault); 
        }
        public void DetectSound(Vector3 soundPoint)
        {
            if (stateMachine.curStateName == State.Infiltrated ||
                stateMachine.curStateName == State.Neutralized ||
                stateMachine.curStateName == State.Assault ||
                stateMachine.curStateName == State.Trace ||
                stateMachine.curStateName == State.Hide)
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
        #endregion
        #region OVERRIDE FUNCTIONS 
        public override void Notified(Vector3 centrePoint, int size, int index)
        {
            if (stateMachine.curStateName == State.Neutralized|| 
                stateMachine.curStateName == State.Alert ||
                stateMachine.curStateName == State.Assault ||
                stateMachine.curStateName == State.Hide ||
                stateMachine.curStateName == State.Trace)
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
        public override State CurState()
        {
            return curState;
        }
        public override void Hacked()
        {
            NeutralizedState neutralizeReason;
            if (stateMachine.CheckState(State.Neutralized))
            {
                neutralizeReason = stateMachine.RetrieveState(State.Neutralized) as NeutralizedState;
                neutralizeReason.SetDeathReason(NeutralizedState.DeathType.Hacked);
            }
            stateMachine.ChangeState(State.Neutralized);
            onDeath?.Invoke(Vector3.zero, Vector3.zero);
            StartCoroutine(DeathCycle(false));
        }
        public override void HackFailed(State prevState)
        {
            if (stateMachine.curStateName != State.Neutralized)
                stateMachine.ChangeState(prevState);
        }
        public override void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (stateMachine.curStateName == State.Neutralized)
                return;
            //GameManager.Resource.Instantiate<ParticleSystem>("Enemy/TakeDamage", hitPoint, Quaternion.LookRotation(hitNormal), true);
            base.TakeDamage(damage, hitPoint, hitNormal);
            anim.SetTrigger("TakeHit");
            if (CurrentHealth <= 0)
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

        public override void TakeStrike(Transform hitter, float damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            if (stateMachine.curStateName == State.Neutralized)
                return;
            base.TakeStrike(hitter, damage, hitPoint, hitNormal);
            anim.SetTrigger("TakeHit");
            if (CurrentHealth <= 0)
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
            int patrolCount;
            float distDelta;
            bool patrolFinished;
            bool hasPatrolPath;
            const float distThreshold = 2.5f;
            const float collectDist = 10f;
            const float newRegionThreshold = 90f; 
            Vector3 lastLeavingPlace; 
            Vector3 patrolDestination;
            LayerMask patrolPointer; 
            List<Collider> patrolPoints; 
            PriorityQueue<DestinationPoint> patrolQueue; 
            public PatrolState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                if (patrolFinished)
                    return;
                //Compute to find nearest patrol starting point based on robot's current position, get the nearest
                //
                if (UnderDifferentRegion())
                {
                    CollectPatrolPoints(out hasPatrolPath);
                    if (hasPatrolPath)
                        ProcessPatrolPoints();
                    else
                        patrolFinished = true;
                    return; 
                }
                else
                {
                    ProcessPatrolPoints();
                    return;
                }
                
            }

            public override void Exit()
            {
                patrolCount = 0;
                patrolDestination = Vector3.zero;
                lastLeavingPlace = owner.transform.position;
                if (patrolPoints.Count == 0)
                    return; 
                patrolFinished = false;
            }

            public override void Setup()
            {
                patrolPoints = new List<Collider>(); 
                patrolPointer = LayerMask.NameToLayer("Point"); 
                CollectPatrolPoints(out hasPatrolPath);
                if (!hasPatrolPath)
                {
                    patrolFinished = true;
                    return;
                }
                patrolFinished = false; 
                patrolCount = 0;
                lastLeavingPlace = Vector3.zero; 
                distDelta = 0f; 
                patrolQueue = new PriorityQueue<DestinationPoint> ();
                ProcessPatrolPoints();  
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
            public bool UnderDifferentRegion()
            {
                if (lastLeavingPlace == Vector3.zero) 
                    return false;
                float distance = Vector3.SqrMagnitude(owner.transform.position - lastLeavingPlace);
                return distance >= newRegionThreshold; 
            }
            public void CollectPatrolPoints(out bool hasPatrolPoints)
            {
                Collider[] colliders = Physics.OverlapSphere(owner.transform.position, collectDist, 1<<14);
                if (colliders.Length > 0)
                {
                    patrolPoints.Clear();
                    foreach (Collider collider in colliders)
                    {
                        patrolPoints.Add(collider);
                    }
                }
                hasPatrolPoints = colliders.Length > 0;
            }

            public void ProcessPatrolPoints()
            {
                for (int i = 0; i < patrolPoints.Count; i++)
                {
                    float tempDist = Vector3.SqrMagnitude(patrolPoints[i].transform.position - owner.transform.position);
                    DestinationPoint tempPoint = new DestinationPoint(patrolPoints[i].transform.position, tempDist);
                    patrolQueue.Enqueue(tempPoint);
                }
                patrolDestination = patrolQueue.Dequeue().destinationVectorPoint;
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
            Vector3 destPoint = Vector3.zero;
            bool abortGather; 
            float distDelta;
            const float holdPeriod = 5f; 
            const float distThreshold = 2.5f;
            public AlertState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                owner.agent.isStopped = false;
                owner.agent.updateRotation = true; 
                if (destPoint != Vector3.zero)
                    owner.agent.SetDestination(destPoint);
                else
                    abortGather = true; 
            }

            public override void Exit()
            {
                abortGather = false;
            }

            public override void Setup()
            {
                abortGather = false;
            }

            public void SetGatherPoint(Vector3 location)
            {
                destPoint = location;
            }

            public override void Transition()
            {
                if (distDelta <= distThreshold || abortGather)
                {
                    stateMachine.ChangeState(State.LookAround);
                }
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
            Vector3 lookDir;
            public AssaultState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
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
                owner.agent.isStopped = false;
            }

            public override void Setup()
            {
                robotGun = owner.robotGun;
                lookDir = Vector3.zero;
            }

            public override void Transition()
            {
                if (robotGun.OutOfAmmo)
                {
                    stateMachine.ChangeState(State.Hide); 
                }
            }

            public override void Update()
            {
                //owner.focusDir = (owner.playerBody.transform.position - owner.transform.position).normalized;
                RotateTowardPlayer();
                robotGun.AttemptFire(owner.playerBody);
            }

            public void RotateTowardPlayer()
            {
                LookDirToPlayer(owner.playerBody.transform.position, owner.transform, out lookDir); 
                owner.transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
        #endregion
        #region Hide State 
        /// <summary>
        /// Upon a Combat or where Enemy is run out of the ammo, 
        /// Enemy 
        /// </summary>
        public class HideState : GuardState
        {
            Transform player;
            //Sake of providing buffers for overlapsphere search. 
            //Smaller the buffer size, quicker the search shall be. 
            Collider[] Colliders;
            const int colliderSize = 10; 
            const int checkRadius = 8; 
            const float hideFrequency = .25f; 
            const float hideSensitivity = -.1f;
            const float minimumDistFromPlayer = 10f;
            Vector3 hidePosition; 
            Vector3 playerLookDir; 
            Vector3 initialPosition; 
            WaitForSeconds hideInterval;
            LayerMask hideableLayer;

            NavMeshPath samplePath = new NavMeshPath();
            float timeOut;
            bool searchLocFound; 
            bool stopCounting; 

            public HideState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
            {
            }

            public override void Enter()
            {
                if (player == null)
                    player = owner.playerBody;
                initialPosition = owner.transform.position; 
                owner.agent.updateRotation = false; 
                owner.StartCoroutine(HideRoutine(player)); 
            }

            public override void Exit()
            {
                owner.StopCoroutine(HideRoutine(player));
                owner.StopAllCoroutines(); 
                timeOut = 0f;
                stopCounting = false; 
            }

            public override void Setup()
            {
                playerLookDir = Vector3.zero; 
                hideInterval = new WaitForSeconds(hideFrequency);
                hideableLayer = 1 << 7;
                Colliders = new Collider[colliderSize];
                hidePosition = Vector3.zero;
                searchLocFound = false; 
                stopCounting = false; 
            }
            public override void Transition()
            {
                // If Enemy has finished reloading 
                // If Enemy has returned to the initial positions. 
                if (hidePosition != Vector3.zero)
                {
                    timeOut = 0f; 
                    stopCounting = true;
                    owner.StopAllCoroutines(); 
                    owner.StopCoroutine(HideRoutine(player));
                    owner.robotGun.Reload();
                }
                else if (!stopCounting && timeOut > 3f)
                {
                    owner.StopAllCoroutines();
                    owner.StopCoroutine(HideRoutine(player));
                    owner.robotGun.Reload(); 
                }
                if (!owner.robotGun.Reloading)
                {
                    owner.StopAllCoroutines();
                    owner.StopCoroutine(HideRoutine(player));
                    stateMachine.ChangeState(State.Trace);
                }
            }
            public override void Update()
            {
                LookDirToPlayer(player.position, owner.transform, out playerLookDir); 
                owner.transform.rotation = Quaternion.Lerp(owner.transform.rotation, 
                    Quaternion.LookRotation(playerLookDir), .3f);
                if (searchLocFound)
                    owner.StopAllCoroutines(); 
                if (stopCounting)
                    return;
                timeOut += Time.deltaTime;
            }

            
            IEnumerator HideRoutine(Transform Target)
            {
                while (true)
                {
                    for (int i = 0; i < Colliders.Length; i++)
                    {
                        Colliders[i] = null;
                    }
                    int hits = Physics.OverlapSphereNonAlloc(owner.transform.position, 
                        checkRadius, Colliders, hideableLayer);

                    int hitReduction = 0;
                    for (int i = 0; i < hits; i++)
                    {
                        if (Vector3.SqrMagnitude(Colliders[i].transform.position - Target.position) < minimumDistFromPlayer)
                        {
                            Colliders[i] = null;
                            hitReduction++;
                        }
                    }
                    hits -= hitReduction;

                    System.Array.Sort(Colliders, ColliderArraySortComparer);

                    for (int i = 0; i < hits; i++)
                    {
                        if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 2f, owner.agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit.position, out hit, owner.agent.areaMask))
                            {
                                continue; 
                            }

                            if (Vector3.Dot(hit.normal, (Target.position - hit.position).normalized) < hideSensitivity)
                            {
                                if (NavMesh.CalculatePath(owner.agent.transform.position, hit.position, 
                                    owner.agent.areaMask, samplePath))
                                {
                                    owner.agent.SetDestination(hit.position);
                                    searchLocFound = true;
                                    hidePosition = hit.position;
                                    yield break;
                                }
                                continue; 
                            }
                            else
                            {
                                // Since the previous spot wasn't facing "away" enough from teh target, we'll try on the other side of the object
                                if (NavMesh.SamplePosition(Colliders[i].transform.position - 
                                    (Target.position - hit.position).normalized * 2, 
                                    out NavMeshHit hit2, 2f, owner.agent.areaMask))
                                {
                                    if (!NavMesh.FindClosestEdge(hit2.position, out hit2, owner.agent.areaMask))
                                    {
                                        continue; 
                                    }

                                    if (Vector3.Dot(hit2.normal, (Target.position - hit2.position).normalized) < hideSensitivity)
                                    {
                                        if (NavMesh.CalculatePath(owner.agent.transform.position, hit2.position,
                                    owner.agent.areaMask, samplePath))
                                        {
                                            owner.agent.SetDestination(hit2.position);
                                            searchLocFound = true;
                                            hidePosition = hit2.position;
                                            yield break;
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                        else
                        {
                        }                 
                    }
                    yield return hideInterval;
                }
            }
            public int ColliderArraySortComparer(Collider A, Collider B)
            {
                if (A == null && B != null)
                {
                    return 1;
                }
                else if (A != null && B == null)
                {
                    return -1;
                }
                else if (A == null && B == null)
                {
                    return 0;
                }
                else
                {
                    return Vector3.SqrMagnitude(owner.agent.transform.position- A.transform.position).
                        CompareTo(Vector3.SqrMagnitude(owner.agent.transform.position - B.transform.position));
                }
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