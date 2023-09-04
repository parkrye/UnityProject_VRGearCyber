using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class SyberDoor : MonoBehaviour, IHitable, IHackable
    {
        [SerializeField] int hp;
        [SerializeField] GameData.HackProgressState state;
        Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
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
            hp += 5;
        }

        public virtual void Success()
        {
            StartCoroutine(OpenDoor());
        }

        /// <summary>
        /// Wait until hack progress state be failure or success
        /// </summary>
        public virtual IEnumerator WaitingHackResultRoutine()
        {
            yield return null;
            state = GameData.HackProgressState.Progress;
            yield return new WaitUntil(() =>  state != GameData.HackProgressState.Progress );
            switch(state)
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

        IEnumerator OpenDoor()
        {
            animator.SetTrigger("Open");
            yield return null;
        }

    }
}
