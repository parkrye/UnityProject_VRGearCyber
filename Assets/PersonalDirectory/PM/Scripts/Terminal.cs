using PID;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace PM
{
    public class Terminal : MonoBehaviour, IHackable, IHitable
    {
        [SerializeField] GameData.HackProgressState state;
        public GuardEnemy[] guards;
        [SerializeField] int hp;

        public virtual void Hack()
        {
            StartCoroutine(WaitingHackResultRoutine());
        }

        public virtual void ChangeProgressState(GameData.HackProgressState value)
        {
            state = value;
        }

        public virtual void Failure()
        {
            
            StartCoroutine(CallSecurity(transform.position));
        }

        public virtual void Success()
        {
            StartCoroutine(Break());
        }

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

        public IEnumerator CallSecurity(Vector3 destination)
        {
            int index = 0;
            foreach (GuardEnemy guard in guards)
            {
                // 로봇의 위치를 추적하는 함수를 가져와서 플레이어의 위치를 변수로 값을 넘겨줌 guard.GetComponent<>
                guard.Notified(destination, guards.Length, index++);
                yield return null;
            }
            yield return null;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            hp -= damage;
            if (hp <= 0)
                StartCoroutine(Break());
        }

        public IEnumerator Break()
        {
            Destroy(this.gameObject);
            yield return null;
        }
    }
}

