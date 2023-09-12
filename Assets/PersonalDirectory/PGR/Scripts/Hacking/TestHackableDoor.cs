using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PGR
{

    public class TestHackableDoor : XRExclusiveSocketInteractor, IHackable
    {
        [SerializeField] GameData.HackProgressState state;
        [SerializeField] UnityEvent SuccessEvent, FailEvent;

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
            FailEvent?.Invoke();
            StartCoroutine(DoneRoutine());
        }

        /// <summary>
        /// if hacking success run
        /// </summary>
        public virtual void Success()
        {
            SuccessEvent?.Invoke();
            StartCoroutine (DoneRoutine());
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

        IEnumerator DoneRoutine()
        {
            acceptedType = GameData.InteractableType.None;
            yield return new WaitForSeconds(3f);
            acceptedType = GameData.InteractableType.Cable;
        }
    }

}