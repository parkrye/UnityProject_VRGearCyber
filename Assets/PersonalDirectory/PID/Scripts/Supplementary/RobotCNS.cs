using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID;
using static PID.RobotHelper;
using static GameData;
using UnityEditor.AI;

namespace PID
{
    public class RobotCNS : MonoBehaviour, IHackable
    {
        [SerializeField] HackProgressState state;
        BaseEnemy enemy;
        State prevState;

        [SerializeField] int pairCount;
        [SerializeField] int fixedPointPerPairCount; 
        #region Hacking Region 
        private void Awake()
        {
            enemy = GetComponentInParent<BaseEnemy>();
            if (enemy.robotType == RobotType.Guard)
            {
                enemy = enemy as GuardEnemy;
            }
            else if (enemy.robotType == RobotType.Tackler)
                enemy = enemy as TackleEnemy;
            else
                Debug.Log("Enemy Not Defined"); 
        }
        public virtual void Hack()
        {
            StopAllCoroutines();
            prevState = enemy.CurState();
            StartCoroutine(WaitingHackResultRoutine());
        }

        public virtual void ChangeProgressState(GameData.HackProgressState value)
        {
            state = value;
        }

        /// <summary>
        /// if hacking failed run
        /// </summary>
        public virtual void Failure()
        {
            enemy.HackFailed(prevState);
            StopAllCoroutines();
        }

        /// <summary>
        /// if hacking success run
        /// </summary>
        public virtual void Success()
        {
            enemy.Hacked();
            StopAllCoroutines();
        }

        /// <summary>
        /// Wait until hack progress state be failure or success
        /// </summary>
        public virtual IEnumerator WaitingHackResultRoutine()
        {
            yield return null;
            state = GameData.HackProgressState.Progress;
            yield return new WaitUntil(() => state != GameData.HackProgressState.Progress);
            switch (state)
            {
                case GameData.HackProgressState.Failure:
                    Failure();
                    break;
                case GameData.HackProgressState.Success:
                    Success();
                    break;
            }
        }

        public (int, int) GetDifficulty()
        {
            //return (2, 3);
            return (pairCount, fixedPointPerPairCount);
        }
        #endregion
    }
}