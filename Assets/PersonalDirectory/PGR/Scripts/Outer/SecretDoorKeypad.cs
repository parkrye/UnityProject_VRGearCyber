using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PGR
{
    public class SecretDoorKeypad : MonoBehaviour, IHackable
    {
        [SerializeField] GameData.HackProgressState state;
        [SerializeField] int pairCount, fixedPointPerPairCount;
        [SerializeField] int[] passowrd;
        [SerializeField] Queue<int> inputQueue;
        [SerializeField] bool isOpen;
        public UnityEvent ResetEvent;
        [SerializeField] TMP_Text hintText;

        [SerializeField] GameObject doorOBject;

        void Awake()
        {
            passowrd = new int[4];
            for(int i = 0; i < passowrd.Length; i++)
            {
                passowrd[i] = Random.Range(1, 10);
                hintText.text += passowrd[i].ToString();
            }
            inputQueue = new Queue<int>();
        }

        public void InputPassword(int keyNum)
        {
            if (isOpen)
                return;
            inputQueue.Enqueue(keyNum);
            if (inputQueue.Count >= 4)
                CheckInput();
        }

        void CheckInput()
        {
            for(int i = 0; i < 4; i++)
            {
                if (passowrd[i] != inputQueue.Dequeue())
                {
                    inputQueue = new Queue<int>();
                    ResetEvent?.Invoke();
                    return;
                }
            }

            ResetEvent?.Invoke();
            StartCoroutine(OpenSecretDoorRoutine());
        }

        IEnumerator OpenSecretDoorRoutine()
        {
            yield return null;
            Debug.Log("Open");
            isOpen = true;
            doorOBject.SetActive(true);
        }

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
            state = GameData.HackProgressState.None;
        }

        /// <summary>
        /// if hacking success run
        /// </summary>
        public virtual void Success()
        {
            StartCoroutine(OpenSecretDoorRoutine());
        }

        /// <summary>
        /// Wait until hack progress state be failure or success
        /// </summary>
        public virtual IEnumerator WaitingHackResultRoutine()
        {
            yield return null;
            state = GameData.HackProgressState.Progress;
            yield return new WaitUntil(() => state != GameData.HackProgressState.Progress);
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

        public (int, int) GetDifficulty()
        {
            return (pairCount, fixedPointPerPairCount);
        }
    }

}