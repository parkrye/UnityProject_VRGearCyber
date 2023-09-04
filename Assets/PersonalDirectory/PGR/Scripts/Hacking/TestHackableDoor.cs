using System.Collections;
using UnityEngine;

namespace PGR
{

    public class TestHackableDoor : XRExclusiveSocketInteractor, IHackable
    {
        [SerializeField] GameData.HackProgressState state;
        [SerializeField] AudioSource successAudio, failAudio;

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
            failAudio.Play();
            StartCoroutine(OpenRoutine());
        }

        /// <summary>
        /// if hacking success run
        /// </summary>
        public virtual void Success()
        {
            successAudio.Play();
            StartCoroutine (OpenRoutine());
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

        IEnumerator OpenRoutine()
        {
            yield return new WaitForSeconds(3f);
            acceptedType = GameData.InteractableType.Cable;
        }
    }

}