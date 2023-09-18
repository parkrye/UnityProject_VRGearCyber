using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    // 해킹에 성공하면 연결된 문을 열고 실패하면 로봇들을 호출
    public class DoorController : MonoBehaviour, IHackable
    {
        [SerializeField] int pairCount;
        [SerializeField] int fixedPointPerPairCount;
        [SerializeField] int hp;
        [SerializeField] GameData.HackProgressState state;
        Terminal terminal;
        MaterialChange materialChange;

        private void Start()
        {
            materialChange = GetComponent<MaterialChange>();
            terminal = transform.parent.parent.parent.GetComponentInChildren<Terminal>();
        }
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
            if(terminal != null)
            {
                terminal.StartCoroutine(terminal.CallSecurity(transform.position));
            }
        }

        public virtual void Success()
        {
            StartCoroutine(transform.parent.GetComponentInChildren<SyberDoor>()?.OpenDoor());
        }

        public virtual IEnumerator WaitingHackResultRoutine()
        {
            yield return null;
            state = GameData.HackProgressState.Progress;
            materialChange.HackingStart();
            yield return new WaitUntil(() => state != GameData.HackProgressState.Progress);
            materialChange.HackingStop();
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

        public IEnumerator Break()
        {
            Destroy(gameObject);
            yield return null;
        }

        public void TakeDamage(int damage, Vector3 hitPoint, Vector3 hitNormal)
        {
            hp -= damage;
            if (hp <= 0)
                StartCoroutine(Break());
        }
        public (int, int) GetDifficulty()
        {
            return (pairCount, fixedPointPerPairCount);
        }
    }
}

