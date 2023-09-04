using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGR
{

    public class TestHackable : MonoBehaviour, IHackable
    {
        [SerializeField] GameData.HackProgressState state;

        /// <summary>
        /// Call by Hacking Cable
        /// </summary>
        public virtual void Hack()
        {
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
            Debug.Log("Failure");
            Destroy(this);
        }

        /// <summary>
        /// if hacking success run
        /// </summary>
        public virtual void Success()
        {
            Debug.Log("Success");
            Destroy(this);
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