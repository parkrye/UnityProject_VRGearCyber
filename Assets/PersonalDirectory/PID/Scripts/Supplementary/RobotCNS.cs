using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PID;
using static GameData; 

namespace PID
{
    public class RobotCNS : MonoBehaviour, IHackable
    {
        HackProgressState state;
        GuardEnemy enemy;
        GuardEnemy.State prevState;

        private void Awake()
        {
            enemy = GetComponentInParent<GuardEnemy>();
        }
        public virtual void Hack()
        {
            StopAllCoroutines();
            prevState = enemy.curState;
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
    }
}

