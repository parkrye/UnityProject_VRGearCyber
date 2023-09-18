using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PGR
{
    public class CableObject : CustomGrabInteractable
    {
        [Header("Cable Object Parameters")]
        [SerializeField] Transform playerTransform, socketTransform;
        [SerializeField] Light pointLight;
        [SerializeField] IHackable hackingTarget;
        [SerializeField] HackingPuzzle puzzle;
        public UnityEvent<bool> cableStateEvent;
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] Collider coll;

        public enum CableState { Hide, Ready, Shot, Hack }
        [SerializeField] CableState state;
        public CableState State 
        { 
            get { return state; } 
            set 
            { 
                state = value;
                switch (state)
                {
                    case CableState.Hide:
                        StartCoroutine(HideRoutine());
                        pointLight.enabled = false;
                        trailRenderer.enabled = false;
                        break;
                    case CableState.Ready:
                        coll.enabled = true;
                        pointLight.enabled = true;
                        trailRenderer.enabled = true;
                        break;
                    case CableState.Shot:
                        coll.enabled = true;
                        pointLight.enabled = true;
                        trailRenderer.enabled = true;
                        break;
                    case CableState.Hack:
                        coll.enabled = false;
                        pointLight.enabled = true;
                        trailRenderer.enabled = true;
                        break;
                }
            } 
        }

        void Start()
        {
            StartCoroutine(AwakeWaitRoutine());
        }

        IEnumerator AwakeWaitRoutine()
        {
            yield return new WaitUntil(() => GameManager.Data != null);
            yield return new WaitUntil(() => GameManager.Data.Player != null);
            yield return new WaitUntil(() => GameManager.Data.Player.Display != null);

            pointLight = GetComponent<Light>();
            trailRenderer = GetComponent<TrailRenderer>();
            Priority = 10;
            cableStateEvent.AddListener(GameManager.Data.Player.Display.ModifyCable);

            yield return new WaitForSeconds(1f);
            State = CableState.Hide;
        }

        public void ReturnToHand()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            cableStateEvent?.Invoke(false);
            State = CableState.Hide;

            hackingTarget = null;
            puzzle = null;
        }

        public void ExitPuzzle()
        {
            if (hackingTarget == null || puzzle == null)
                return;

            puzzle.StopPuzzle(GameData.HackProgressState.Failure);
        }

        public void ReadyToShot()
        {
            State = CableState.Ready;
            cableStateEvent?.Invoke(true);
        }

        public void ShotCable(Vector3 shotDirection, float shotPower)
        {
            State = CableState.Shot;
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            rb.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        }

        void OnTriggerEnter(Collider other)
        {
            if (State != CableState.Shot)
                return;

            hackingTarget = other.GetComponent<IHackable>();
            if (hackingTarget == null)
                return;

            XRExclusiveSocketInteractor socketInteractor = other.GetComponent<XRExclusiveSocketInteractor>();
            if (socketInteractor == null)
                return;

            State = CableState.Hack;
            hackingTarget.Hack();
            (int, int) difficulty = hackingTarget.GetDifficulty();
            puzzle = GameManager.Resource.Instantiate<HackingPuzzle>("Hacking/HackingPuzzle", playerTransform.position, Quaternion.identity);
            puzzle.InitialPuzzle(hackingTarget, socketInteractor, this, difficulty.Item1, difficulty.Item2);
        }

        IEnumerator HideRoutine()
        {
            coll.enabled = true;
            transform.position = socketTransform.position;
            yield return new WaitUntil(() => rb.isKinematic);
            coll.enabled = false;
        }
    }
}
