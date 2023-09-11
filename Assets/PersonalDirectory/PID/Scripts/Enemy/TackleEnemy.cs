using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID; 

namespace PID
{
    public class TackleEnemy : BaseEnemy, IHitable
    {
        RobotBaton attackStick;
        EnemyStat enemyStat;
        Rigidbody rigid; 
        protected override void Awake()
        {
            enemyStat = GameManager.Resource.Load<EnemyStat>("Data/Tackler"); 
            base.Awake();
            base.SetUp(enemyStat); 
        }
    }
}