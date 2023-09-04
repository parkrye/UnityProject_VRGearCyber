using System.Collections;
using UnityEngine;

namespace PGR
{
    public class CableObject : CustomGrabInteractable
    {
        [Header("Cable Object Parameters")]
        [SerializeField] Transform playerTransform;
        [SerializeField] Light pointLight;
        [SerializeField] IHackable hackingTarget;
        [SerializeField] bool state;
        public bool State { get { return state; } }
        [SerializeField] HackingPuzzle puzzle;

        protected override void Awake()
        {
            base.Awake();
            pointLight = GetComponent<Light>();

            Priority = 10;
            pointLight.enabled = false;
            state = false;
        }

        public void FixExit()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            pointLight.enabled = false;
            state = false;
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
        }

        public void ShotCable(Vector3 shotDirection, float shotPower)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            rb.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        }

        void OnTriggerEnter(Collider other)
        {
            if (state)
                return;

            hackingTarget = other.GetComponent<IHackable>();
            if (hackingTarget == null)
                return;

            XRExclusiveSocketInteractor socketInteractor = other.GetComponent<XRExclusiveSocketInteractor>();
            if (socketInteractor == null)
                return;

            state = true;
            hackingTarget.Hack();
            puzzle = GameManager.Resource.Instantiate<HackingPuzzle>("Hacking/HackingPuzzle", playerTransform.position, Quaternion.identity);
            puzzle.InitialPuzzle(hackingTarget, socketInteractor, this, 2, 3);
        }
    }
}
