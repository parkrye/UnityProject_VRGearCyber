using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PGR
{
    public class CableObject : CustomGrabInteractable
    {
        [Header("Cable Object Parameters")]
        [SerializeField] Transform playerTransform;
        [SerializeField] Light pointLight;
        [SerializeField] IHackable hackingTarget;
        [SerializeField] bool state;
        public bool State { get { return state; } set { state = value; } }
        [SerializeField] HackingPuzzle puzzle;
        public UnityEvent<bool> cableStateEvent;
        [SerializeField] TrailRenderer trailRenderer;

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
            pointLight.enabled = false;
            trailRenderer.enabled = false;
            cableStateEvent.AddListener(GameManager.Data.Player.Display.ModifyCable);
            State = false;
        }

        public void FixExit()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            pointLight.enabled = false;
            trailRenderer.enabled = false;
            cableStateEvent?.Invoke(false);
            State = false;
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
            pointLight.enabled = true;
            trailRenderer.enabled = true;
            cableStateEvent?.Invoke(true);
        }

        public void ShotCable(Vector3 shotDirection, float shotPower)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            rb.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        }

        void OnTriggerEnter(Collider other)
        {
            if (State)
                return;

            hackingTarget = other.GetComponent<IHackable>();
            if (hackingTarget == null)
                return;

            XRExclusiveSocketInteractor socketInteractor = other.GetComponent<XRExclusiveSocketInteractor>();
            if (socketInteractor == null)
                return;

            State = true;
            hackingTarget.Hack();
            (int, int) difficulty = hackingTarget.GetDifficulty();
            puzzle = GameManager.Resource.Instantiate<HackingPuzzle>("Hacking/HackingPuzzle", playerTransform.position, Quaternion.identity);
            puzzle.InitialPuzzle(hackingTarget, socketInteractor, this, difficulty.Item1, difficulty.Item2);
        }
    }
}
