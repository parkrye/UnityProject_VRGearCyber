using ildoo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(SightFunction))]
public class GuardEnemy : BaseEnemy
{
    public enum State
    {
        Idle,
        Patrol,
        Alert,
        Assault, 
        Death,
        Size
            //Possibly extending beyond for Gathering Abilities. 
    }
    //Properties 
    Animator anim;
    Rigidbody rigid; 
    NavMeshAgent agent;
    SightFunction guardSight;
    StateMachine <State, GuardEnemy> stateMachine;

    protected void Awake()
    {
        enemyStat = GameManager.Resource.Instantiate<EnemyStat>("Data/Guard"); 
        base.SetUp(enemyStat);
        stateMachine = new StateMachine<State, GuardEnemy>(this);

        stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
        stateMachine.AddState(State.Patrol, new PatrolState(this, stateMachine)); 
        stateMachine.AddState(State.Alert, new AlertState(this, stateMachine));
        stateMachine.AddState(State.Assault, new AssaultState(this, stateMachine));
        stateMachine.AddState(State.Death, new DeathState(this, stateMachine); 
    }
    private void Start()
    {
        
    }

    public abstract class GuardState : StateBase<State, GuardEnemy>
    {
        public Animator anim => owner.anim; 
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
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            throw new System.NotImplementedException();
        }

        public override void Transition()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
    #endregion

    #region Patrol State 
    public class PatrolState : GuardState
    {
        int lastPatrolPoint;
        int nextPatrolPoint; 

        public PatrolState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            lastPatrolPoint = 0;
        }

        public override void Transition()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
    #endregion

    #region Alert State
    //Activated when CCTV detects a Player, 
    // Main Function being, Each assigned Guard should head back to the designated CCTV regions. 
    public class AlertState : GuardState
    {
        SightFunction guardSight;
        float distDelta;
        const float distThreshhold = 3.5f; 
        public AlertState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            guardSight = owner.guardSight;
        }

        public override void Transition()
        {
            switch (guardSight.TargetFound)
            {
                case true:
                    break;
                case false:
                    stateMachine.ChangeState(State.Assault); 
                    break;
            }
            if (guardSight.TargetFound)
            {

            }
            else if (!guardSight.TargetFound)
            {

            }
        }

        public override void Update()
        {
            if (distDelta > distThreshhold)
                return; 
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
            owner.agent.isStopped = true;

        }

        public override void Exit()
        {
            owner.agent.isStopped = false;
        }

        public override void Setup()
        {
            throw new System.NotImplementedException();
        }

        public override void Transition()
        {
            
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
    #endregion

    #region AttackAssault 
    #endregion

    #region Death 
    public class DeathState : GuardState
    {
        public DeathState(GuardEnemy owner, StateMachine<State, GuardEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            //Should Notify CCTV to erase from the list 

        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void Setup()
        {
            throw new System.NotImplementedException();
        }

        public override void Transition()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
    #endregion
}
